using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PEngine.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PEngine
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor()
                    .AddDistributedMemoryCache()
                    .AddSession(configure => {
                        configure.Cookie.Name = "PEngineSessId";
                        configure.Cookie.HttpOnly = true;
                        configure.Cookie.IsEssential = true;
                        configure.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                        configure.Cookie.SameSite = SameSiteMode.Strict;
                    })
                    .AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UsePEngineWebsocketMiddleware();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "manage-area",
                    areaName: "manage",
                    pattern: "Manage/{controller}/{action}/{id?}"
                );

                endpoints.MapAreaControllerRoute(
                    name: "api-area",
                    areaName: "api",
                    pattern: "Api/{controller}/{action}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
