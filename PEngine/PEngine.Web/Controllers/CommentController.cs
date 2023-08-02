using Microsoft.AspNetCore.Mvc;

namespace PEngine.Web.Controllers
{
    public class CommentController : CommonControllerBase<CommentController>
    {
        public CommentController(ILogger<CommentController> logger) : base(logger)
        {
        }
    }
}
