using Microsoft.AspNetCore.Mvc;

namespace PEngine.Web.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
