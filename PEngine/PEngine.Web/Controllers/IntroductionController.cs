using Microsoft.AspNetCore.Mvc;

namespace PEngine.Web.Controllers;

public class IntroductionController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}