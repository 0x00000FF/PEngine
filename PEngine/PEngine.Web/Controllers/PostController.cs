using System.Text.RegularExpressions;
using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PEngine.Web.Models;
using PEngine.Web.Models.ViewModels;

namespace PEngine.Web.Controllers
{
    public class PostController : CommonControllerBase<PostController>
    {
        private readonly BlogContext _context;
        
        public PostController(ILogger<PostController> logger, BlogContext context) : base(logger)
        {
            _context = context;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            
            ViewData.Add("PostArea", true);
            ViewData.Add("Categories", GetCategoryList());
        }

        private List<Category> GetCategoryList()
        {
            var categories = _context.Categories.OrderBy(c => c.Name).ToList();
            categories[0].Count = _context.Categories.Sum(c => c.Count);

            return categories;
        }
        
        [HttpGet("/[controller]/[action]/{category?}")]
        public IActionResult List(string? category)
        {
            var posts = _context.Posts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
            {
                posts = posts.Where(p => p.Category == category);
            }

            posts = posts.OrderByDescending(p => p.Id)
                    .Take(30);
            
            return View(posts.ToList());
        }


        private async Task HitPost(long id)
        {
            await _context.Database.ExecuteSqlRawAsync(
                $"UPDATE {nameof(BlogContext.Posts)} SET Hits = Hits + 1 WHERE Id = {{0}}",
                id);
            await _context.SaveChangesAsync();
        }

        private async Task UpdateCategoryCount(string? name, int count)
        {
            await _context.Database.ExecuteSqlRawAsync(
                $"UPDATE {nameof(BlogContext.Categories)} SET Count = Count + {{0}} WHERE Name = {{1}}",
                count, name ?? "");
        }

        private Guid? ExtractFirstImageSrc(string html)
        {
            var imgRegex = new Regex(@"<img[^>]*src\s*=\s*['""]([^'""]+)['""][^>]*>");
            var matches = imgRegex.Matches(html);

            if (matches.Count == 0)
            {
                return null;
            }
            
            var firstMatch = matches.FirstOrDefault(m => 
                m.Groups.Count == 2 && m.Groups[1].Value.StartsWith("/File/Download/"));

            if (firstMatch is null)
            {
                return null;
            }

            var guid = firstMatch.Groups[1].Value.Split('/')[3];
            var guidRegex =
                new Regex("(?:[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12})");
            
            return guidRegex.IsMatch(guid) ? new Guid(guid) : null;
        }
        
        public async Task<IActionResult> View(long id)
        {
            var post = await _context.Posts.Where(p => p.Id == id)
                .Join(_context.Users, p => p.WrittenBy, u => u.Id, 
                    (p, u) => new PostVM()
                    {
                        Id = p.Id,
                        WriterName = u.Name,
                        Category = p.Category,
                        Title = p.Title,
                        Content = p.Content,
                        WrittenAt = p.WrittenAt,
                        Hits = p.Hits
                    })
                .FirstOrDefaultAsync();

            if (post is null)
            {
                return NotFound();
            }

            await HitPost(post.Id);
            
            return View(post);
        }

        [Authorize]
        public IActionResult Write()
        {
            return View("Editor");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Write(Post formData, 
            [FromServices] IHtmlSanitizer sanitizer)
        {
            return await Modify(formData, sanitizer);
        }

        [Authorize]
        public IActionResult Modify(int id)
        {
            GetCategoryList();
            
            var post = _context.Posts.FirstOrDefault(p => p.Id == id);

            if (post is null)
            {
                return NotFound();
            }
            
            return View("Editor", post);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Modify(Post formData, 
            [FromServices] IHtmlSanitizer sanitizer)
        {
            Func<Post, EntityEntry<Post>> updateProc = _context.Posts.Add;
            Post? post = null;
            
            var sanitizedContent = sanitizer.SanitizeDocument(formData.Content!);

            await using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                if (formData.Id != default)
                {
                    updateProc = _context.Posts.Update;
                    post = _context.Posts.FirstOrDefault(p => p.Id == formData.Id && 
                                                              p.WrittenBy == UserId);

                    if (post is null)
                    {
                        return Forbid();
                    }

                    if (formData.Category != post.Category)
                    {
                        await UpdateCategoryCount(post.Category, -1);
                        await UpdateCategoryCount(formData.Category, 1);
                    }

                    post.Title = formData.Title;
                    post.Category = formData.Category ?? "";
                    post.Content = sanitizedContent;
                    post.Thumbnail = ExtractFirstImageSrc(sanitizedContent);
                }
                else
                {
                    await UpdateCategoryCount(formData.Category, 1);
                }
                
                var result = updateProc(post ?? new()
                    {
                        Id = formData.Id,
                        WrittenBy = UserId!.Value,
                        Category = formData.Category ?? "",
                        Title = formData.Title,
                        Content = sanitizedContent,
                        Thumbnail = ExtractFirstImageSrc(sanitizedContent),
                        WrittenAt = DateTime.Now
                    });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return RedirectToAction("View", new { result.Entity.Id });
            }
            catch (Exception e)
            {
                Logger.Log(LogLevel.Error, "at {} from {}\n{}", 
                    nameof(Modify),
                    e.Source, e.Message);
                
                return View("UpdateFailed");
            }
        }
        
        [Authorize]
        public IActionResult Delete(int id)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == id && p.WrittenBy == UserId);

            if (post is null)
            {
                return NotFound();
            }
            
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, string message)
        {
            if (int.TryParse(message, out var messageValue))
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                var post = _context.Posts
                    .FirstOrDefault(p => p.Id == id && p.WrittenBy == UserId &&
                                         p.Id == messageValue);

                if (post is null)
                {
                    return NotFound();
                }

                try
                {
                    _context.Posts.Remove(post);

                    await UpdateCategoryCount(post.Category, -1);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    
                    return RedirectToAction("List");
                }
                catch (Exception e)
                {
                    Logger.Log(LogLevel.Error, "at {} from {}\n{}", 
                        nameof(Modify),
                        e.Source, e.Message);
                    
                    return View("DeleteFailed");
                }
            }

            return View("Delete", new { message = "Code Check Failed..." });
        }

        [Authorize]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult AddCategory(string category)
        {
            _context.Categories.Add(new() { Name = category, Count = default });
            
            return _context.SaveChanges() > 0 ? 
                RedirectToAction("List", "Post") : 
                View();
        }
    }
}
