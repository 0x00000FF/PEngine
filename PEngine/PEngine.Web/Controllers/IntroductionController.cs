using Microsoft.AspNetCore.Mvc;

namespace PEngine.Web.Controllers;

public class IntroductionController : CommonControllerBase<IntroductionController>
{
    public IntroductionController(ILogger<IntroductionController> logger) : base(logger)
    {
    }
    
    public IActionResult Index()
    {
        return View();
    }

}