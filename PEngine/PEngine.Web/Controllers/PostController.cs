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
        }

        [HttpGet("/[controller]/[action]/{category?}")]
        public IActionResult List(string? category)
        {
            var posts = _context.Posts.AsQueryable();

            if (category is not null)
            {
                posts = posts.Where(p => p.Category == category);
            }

            posts = posts.OrderByDescending(p => p.Id)
                    .Take(30);
            
            return View(posts.ToList());
        }


        public async Task HitPost(long id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post is not null)
            {
                post.Hits++;
                _context.Posts.Update(post);
                
                await _context.SaveChangesAsync();
            }
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

            if (formData.Id != default)
            {
                updateProc = _context.Posts.Update;
                post = _context.Posts.FirstOrDefault(p => p.Id == formData.Id && 
                                                          p.WrittenBy == UserId);

                if (post is null)
                {
                    return Forbid();
                }

                post.Title = formData.Title;
                post.Category = formData.Category;
                post.Content = sanitizedContent;
            }
            
            var result = updateProc(post ?? new ()
            {
                Id = formData.Id,
                WrittenBy = UserId!.Value,
                Category = formData.Category,
                Title = formData.Title,
                Content = sanitizedContent,
                WrittenAt = DateTime.Now
            });
            
            return await _context.SaveChangesAsync() > 0 ?
                RedirectToAction("View", new { Id = result.Entity.Id }) :
                View("UpdateFailed");
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
                var post = _context.Posts
                    .FirstOrDefault(p => p.Id == id && p.WrittenBy == UserId &&
                                         p.Id == messageValue);

                if (post is null)
                {
                    return NotFound();
                }

                _context.Posts.Remove(post);
                
                return await _context.SaveChangesAsync() > 0 ?
                    RedirectToAction("List") : 
                    View("DeleteFailed");
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
            return View();
        }
    }
}
