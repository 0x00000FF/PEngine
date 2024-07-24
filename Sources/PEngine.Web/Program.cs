using Ganss.Xss;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Npgsql;
using PEngine.Web.Controllers;
using PEngine.Web.Data;
using PEngine.Web.Helper;
using System.Data;

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

        private static void ConfigureLocalStorage()
        {
            FileHelper.InitializeStorage();
        }

        private static void ConfigureMasterSecret()
        {
            // TODO: Query TPM Availability or Master Secret generated in the Storage
        }

        private static void ConfigureAuthCookies(CookieAuthenticationOptions options)
        {
            var controllerFragment = nameof(MemberController).Replace("Controller", string.Empty);

            options.LoginPath = $"/{controllerFragment}/Login";
            options.LogoutPath = $"/{controllerFragment}/Logout";

            options.Cookie.Name = "_PEngineAuth_";
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        }
        
        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            DevMode = builder.Environment.IsDevelopment();
            
            var mvcBuilder = builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();

            // ------------------- SECURITY

            builder.Services.AddAntiforgery();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(ConfigureAuthCookies);
            builder.Services.AddSingleton<IHtmlSanitizer, HtmlSanitizer>(InitSanitizer);

            // ------------------- DATABASE CONFIGURATION

            var connectionString = DevMode ?
                builder.Configuration.GetConnectionString("Development") :
                builder.Configuration.GetConnectionString("Production");

            var factory = new DataConnectionFactory<NpgsqlConnection>(connectionString!);

            builder.Services.AddTransient<IDbConnection, NpgsqlConnection>(factory.Create);

            // ------------------- MISC

            if (DevMode)
            {
                mvcBuilder.AddRazorRuntimeCompilation();
            }
            
            builder.Services.Configure<ForwardedHeadersOptions>(ConfigureReverseProxy);

            ConfigureLocalStorage();
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