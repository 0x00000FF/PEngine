using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace PEngine.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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