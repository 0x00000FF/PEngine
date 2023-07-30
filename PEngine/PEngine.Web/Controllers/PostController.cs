using Microsoft.AspNetCore.Mvc;

namespace PEngine.Web.Controllers
{
    public class PostController : Controller
    {
        public IActionResult Index()
        {
            return List("");
        }

        [HttpGet("/[controller]/[action]/{category?}")]
        public IActionResult List(string? category)
        {
            return View();
        }

        public IActionResult View(int id)
        {
            return View();
        }

        public IActionResult Write()
        {
            return View("Editor");
        }

        public IActionResult Modify(int id)
        {
            return View("Editor");
        }

        public IActionResult Delete(int id)
        {
            return View();
        }
    }
}
