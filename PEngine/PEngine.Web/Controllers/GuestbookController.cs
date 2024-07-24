using Microsoft.AspNetCore.Mvc;

namespace PEngine.Web.Controllers
{
    public class GuestbookController : CommonControllerBase<GuestbookController>
    {
        public GuestbookController(ILogger<GuestbookController> logger) : base(logger)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Write()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Modify()
        {
            return View();
        }
    }
}
