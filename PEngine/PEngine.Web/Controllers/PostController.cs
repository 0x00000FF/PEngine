using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        
        [HttpGet("/[controller]/[action]/{category?}")]
        public IActionResult List(string? category)
        {
            return View();
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
        public async Task<IActionResult> Write(string category, string title, string content)
        {
            var result = _context.Posts.Add(new ()
            {
                WrittenBy = UserId!.Value,
                Category = category,
                Title = title,
                Content = content,
                WrittenAt = DateTime.Now
            });
            
            return await _context.SaveChangesAsync() > 0 ?
                    RedirectToAction("View", new { Id = result.Entity.Id }) :
                    View("WriteFailed");
        }

        [Authorize]
        public IActionResult Modify(int id)
        {
            return View("Editor");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Modify(int id, object post)
        {
            return Json(null);
        }
        
        [Authorize]
        public IActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, string message)
        {
            return Json(null);
        }
    }
}
