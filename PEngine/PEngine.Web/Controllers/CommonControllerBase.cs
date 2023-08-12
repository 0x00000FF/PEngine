using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PEngine.Web.Controllers;

public abstract class CommonControllerBase<T> : Controller
{
    protected ILogger<T> Logger { get; }
    protected new HttpContext? HttpContext { get; }
    

    protected bool IsAuthenticated { get; set; }
    protected Guid? UserId { get; set; }
    protected string? UserName { get; set; }

    public CommonControllerBase(ILogger<T> logger)
    {
        Logger = logger;
        
        var accessor = Program.App.Services.GetRequiredService<IHttpContextAccessor>();
        HttpContext = accessor.HttpContext;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        IsAuthenticated = HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        
        var userId = HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == "Id")?.Value ?? "";
        
        UserId = IsAuthenticated ? new Guid(userId) : null;
        UserName = IsAuthenticated ? 
                    HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == "Name")?.Value
                    : null;
    }
    
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is ViewResult)
        {
            ViewBag.Authenticated = IsAuthenticated;
            
            ViewData.Add("Id", UserId);
            ViewData.Add("Name", UserName);
        }

        base.OnActionExecuted(context);
    }
}