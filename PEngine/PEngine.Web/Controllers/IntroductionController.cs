using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PEngine.Web.Helper;
using PEngine.Web.Models;

namespace PEngine.Web.Controllers;

public class IntroductionController : CommonControllerBase<IntroductionController>
{
    private Introduction? _introduction;
    
    public IntroductionController(ILogger<IntroductionController> logger) : base(logger)
    {
        
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _introduction = FileHelper.LoadFromJson<Introduction>(BasePath.IntroductionBase, "introduction.json");
        base.OnActionExecuting(context);
    }

    public IActionResult Index()
    {
        return View(_introduction);
    }

    [Authorize]
    public IActionResult Edit()
    {
        return View(_introduction);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Edit(Introduction intro)
    {
        var newFilename = $"introduction_{DateTime.Now:yyyyMMdd}.json";

        try
        {
            FileHelper.SaveToJson(BasePath.IntroductionBase, newFilename, intro);
            
            if (!FileHelper.UpdateSymbolLink(BasePath.IntroductionBase, newFilename, "introduction.json"))
            {
                Logger.Log(LogLevel.Error,  "Symbolic Link {} -> introduction.json Failed", newFilename);
                throw new FileNotFoundException("Symbolic Link failed");
            }
        }
        catch (Exception e)
        {
            Logger.Log(LogLevel.Error, "introduction save failed, reason: {}, {}", e.GetType().Name, e.Message);
        }

        return RedirectToAction("Index");
    }

}