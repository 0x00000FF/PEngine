using Microsoft.AspNetCore.Mvc;
using PEngine.Web.Models;

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

    public IActionResult Edit()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Edit(Introduction intro)
    {
        return View();
    }

}