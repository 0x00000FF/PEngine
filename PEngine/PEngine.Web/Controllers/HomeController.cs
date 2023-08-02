using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace PEngine.Web.Controllers
{
    public class HomeController : CommonControllerBase<HomeController>
    {
        public HomeController(ILogger<HomeController> logger) : base(logger)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/Misc/Mdd")]
        public IActionResult Mdd()
        {
            return View();
        }
    }
}