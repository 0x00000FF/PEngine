using Microsoft.AspNetCore.Mvc;
using PEngine.Web.Repositories;

namespace PEngine.Web.Controllers
{
    public class CommentController : CommonControllerBase<CommentController>
    {
        private const int COUNT_PER_PAGE = 30;
        private readonly BlogContext _context;
        private readonly CommentRepository _repository;

        public CommentController(ILogger<CommentController> logger, BlogContext context) : base(logger)
        {
            _context = context;
            _repository = new(_context);
        }


        public IActionResult List(long postId, int page)
        {
            if (page < 0)
            {
                return List(postId, 1);
            }

            var comments = _repository.GetTopNodes(postId, page, 40, false);
            
            ViewData.Add("PostId", postId);
            return View(comments);
        }
        
        public IActionResult List(long postId)
        {
            return List(postId, page: 1);
        }
    }
}
