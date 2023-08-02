using Microsoft.AspNetCore.Mvc;

namespace PEngine.Web.Controllers
{
    public class CommentController : CommonControllerBase<CommentController>
    {
        private readonly BlogContext _context;
        
        public CommentController(ILogger<CommentController> logger, BlogContext context) : base(logger)
        {
            _context = context;
        }

        public IActionResult List(long postId)
        {
            var comments = _context.Comments.Where(c => c.Post == postId).ToList();
            
            ViewData.Add("PostId", postId);
            return View(comments);
        }
    }
}
