using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace PEngine.Web.Controllers;

public abstract class CommonControllerBase<T> : Controller
{
    protected ILogger<T> Logger { get; }
    protected new HttpContext HttpContext { get; }

    protected string Method { get; }
    protected string UserAgent { get; set; }
    protected string ClientAddress { get; }

    protected bool IsAuthenticated { get; set; }
    protected string? ClaimData(string key)
    {
        return HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == key)?.Value;
    }
        
    protected Guid? UserId { get; set; }
    protected string? UserName { get; set; }

    public CommonControllerBase(ILogger<T> logger)
    {
        Logger = logger;
        
        var accessor = Program.App.Services.GetRequiredService<IHttpContextAccessor>();
        HttpContext = accessor.HttpContext ?? throw new InvalidOperationException();

        Method = HttpContext.Request.Method;
        UserAgent = HttpContext.Request.Headers.UserAgent.ToString().Trim();
        ClientAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (string.IsNullOrEmpty(UserAgent) || (Method != "POST" && Method != "GET"))
        {
            context.Result = new EmptyResult();
        }
        else
        {
            IsAuthenticated = HttpContext?.User?.Identity?.IsAuthenticated ?? false;

            // Kill Authentication When IPSEC engaged & ip mismatch
            if (IsAuthenticated)
            {
                var isIpSec = ClaimData("IPSec") == "y";

                if (isIpSec && ClaimData("IPAddress") != ClientAddress)
                {
                    context.Result = new SignOutResult();
                }

                UserId = new Guid(ClaimData("Id") ?? "");
                UserName = ClaimData("Name");
            }
        }
    }
    
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is ViewResult)
        {
            ViewBag.Authenticated = IsAuthenticated;
            
            ViewData.Add("Id", UserId);
            ViewData.Add("Name", UserName);

            var isIpSec = ClaimData("IPSec") == "y";

            if (isIpSec)
            {
                ViewData.Add("IPSec", isIpSec);
                ViewData.Add("IPAddress", ClaimData("IPAddress"));
            }

            var urlRoot = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            ViewData.Add("UrlRoot", urlRoot);
        }

        base.OnActionExecuted(context);
    }
}