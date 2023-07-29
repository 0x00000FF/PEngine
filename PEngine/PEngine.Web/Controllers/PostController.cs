using Microsoft.AspNetCore.Mvc;

namespace PEngine.Web.Controllers
{
    public class PostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
