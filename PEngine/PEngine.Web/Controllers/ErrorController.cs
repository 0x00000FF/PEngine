using Microsoft.AspNetCore.Mvc;

namespace PEngine.Web.Controllers
{
	[Route("/Error/{statusCode:int}")]
	public class ErrorController : Controller
	{
		[HttpGet]
		public IActionResult Index(int statusCode)
		{
			switch(statusCode)
			{
				case 404:
					return NotFound();

				default:
					return Unknown();
			}
		}

		private new IActionResult NotFound()
		{
			return View("NotFound");
		}

		private IActionResult Unknown()
		{
			return View("Unknown");
		}
	}
}
