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


        return next(context);
    }
}