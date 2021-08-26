using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PEngine.Middlewares
{
    public class WebsocketMiddleware
    {

    }

    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UsePEngineWebsocketMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware(typeof(WebsocketMiddleware));
            return app;
        }
    }
}
