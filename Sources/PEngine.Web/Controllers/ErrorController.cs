using Microsoft.AspNetCore.Mvc;

namespace PEngine.Web.Controllers
{
	[Route("/Error/{statusCode:int}")]
	public class ErrorController : CommonControllerBase<ErrorController>
	{
		public ErrorController(ILogger<ErrorController> logger) : base(logger)
		{
		}
		
		[HttpGet]
		public IActionResult Index(int statusCode)
		{
			switch(statusCode)
			{
				case 404:
					return NotFound();
				
				case 500:
					return ServerError();

				default:
					return Unknown();
			}
		}

		private new IActionResult NotFound()
		{
			return View("NotFound");
		}

		private IActionResult ServerError()
		{
			return View("ServerError");
		}

		private IActionResult Unknown()
		{
			return View("Unknown");
		}

	}
}
