using Microsoft.Extensions.Primitives;
using PEngine.States;

namespace PEngine.Middlewares;

public class VisitorMiddleware : IMiddleware
{
    public bool FirstVisitor(HttpContext context)
    {
        return false;
    }
    public void RecordExternalInflow(StringValues values)
    {
        
    }
    
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var accessIp = context.Connection.RemoteIpAddress;
        var referer = context.Request.Headers.Referer;

        if (!context.Session.Keys.Contains("Expired"))
        {
            context.Session.SetInt32("Expired", 0);
        }
        else if (context.Session.GetInt32("Expired") == 1) 
        {
            context.Response.Cookies.Delete(UserContext.TOKEN_COOKIE);
            context.Response.Redirect("/");

            return Task.CompletedTask;
        }

        return next(context);
    }
}