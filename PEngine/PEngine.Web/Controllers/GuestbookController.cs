using Microsoft.AspNetCore.Mvc;

namespace PEngine.Web.Controllers
{
    public class GuestbookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
