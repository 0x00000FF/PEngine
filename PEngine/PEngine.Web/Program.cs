using Ganss.Xss;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace PEngine.Web
{
    public class Program
    {
        public static WebApplication App { get; private set; } = null!;
        public static bool DevMode { get; private set; }

        private static HtmlSanitizer InitSanitizer(IServiceProvider provider)
        {
            var sanitizer = new HtmlSanitizer();

            sanitizer.AllowedTags.Add("iframe");
            
            return sanitizer;
        }

        private static void ConfigureReverseProxy(ForwardedHeadersOptions options)
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        }

        private static void ConfigureAuthCookies(CookieAuthenticationOptions options)
        {
            options.LoginPath = "/User/Login";
            options.LogoutPath = "/User/Logout";

            options.Cookie.Name = "_PEngineAuth_";
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        }
        
        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            DevMode = builder.Environment.IsDevelopment();
            
            var mvcBuilder = builder.Services.AddControllersWithViews();

            builder.Services.AddHttpContextAccessor();
            
            builder.Services.AddAntiforgery();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(ConfigureAuthCookies);

            BlogContext.SetConnectionString(DevMode ? 
                builder.Configuration.GetConnectionString("Development") :
                builder.Configuration.GetConnectionString("Production"));

            builder.Services.AddDbContext<BlogContext>();
            builder.Services.AddSingleton<IHtmlSanitizer, HtmlSanitizer>(InitSanitizer);
            
            if (DevMode)
            {
                mvcBuilder.AddRazorRuntimeCompilation();
            }
            
            builder.Services.Configure<ForwardedHeadersOptions>(ConfigureReverseProxy);
        }

        private static void Configure(WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            
            app.UseForwardedHeaders();
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);

            var app = builder.Build();
            Configure(app);

            (App = app).Run();
        }
    }
}