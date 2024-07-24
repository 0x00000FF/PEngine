using Microsoft.AspNetCore.Mvc;

namespace PEngine.Web.Controllers
{
    public class CommentController : CommonControllerBase<CommentController>
    {
        
        
        public CommentController(ILogger<CommentController> logger) : base(logger)
        {
            
        }

        public IActionResult List(long postId)
        {
            var comments = _context.Comments.Where(c => c.Post == postId).ToList();
            
            ViewData.Add("PostId", postId);
            return View(comments);
        }
    }
}
