using Microsoft.AspNetCore.Mvc;
using PEngine.Web.Models;

namespace PEngine.Web.Controllers;

public class IntroductionController : CommonControllerBase<IntroductionController>
{
    private readonly BlogContext _context;
    
    public IntroductionController(ILogger<IntroductionController> logger, BlogContext context) : base(logger)
    {
        _context = context;
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