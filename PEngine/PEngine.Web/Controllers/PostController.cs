using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PEngine.Web.Controllers
{
    public class PostController : Controller
    {

        [HttpGet("/[controller]/[action]/{category?}")]
        public IActionResult List(string? category)
        {
            return View();
        }

        public IActionResult View(int id)
        {
            return View();
        }

        [Authorize]
        public IActionResult Write()
        {
            return View("Editor");
        }
        
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Write(object post)
        {
            return Json(null);
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
