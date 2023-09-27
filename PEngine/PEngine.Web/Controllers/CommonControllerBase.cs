using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PEngine.Web.Controllers;

public abstract class CommonControllerBase<T> : Controller
{
    protected ILogger<T> Logger { get; }
    protected new HttpContext HttpContext { get; }


    protected bool IsAuthenticated { get; set; }
    protected Guid? UserId { get; set; }
    protected string? UserName { get; set; }

    public CommonControllerBase(ILogger<T> logger)
    {
        Logger = logger;
        
        var accessor = Program.App.Services.GetRequiredService<IHttpContextAccessor>();
        HttpContext = accessor.HttpContext ?? throw new InvalidOperationException();
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (HttpContext.Request.Headers.UserAgent.ToString().Trim() == "" || 
            (HttpContext.Request.Method.ToUpper() != "POST" &&
            HttpContext.Request.Method.ToUpper() != "GET"))
        {
            context.Result = new StatusCodeResult(404);
            return;
        }
        
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

            var urlRoot = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            ViewData.Add("UrlRoot", urlRoot);
        }

        base.OnActionExecuted(context);
    }
}