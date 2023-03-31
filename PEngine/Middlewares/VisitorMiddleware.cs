using Microsoft.Extensions.Primitives;

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

        return next(context);
    }
}