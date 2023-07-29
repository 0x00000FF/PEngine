using Microsoft.AspNetCore.Mvc;

namespace PEngine.Web.Controllers
{
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
