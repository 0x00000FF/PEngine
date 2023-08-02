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

        public static HtmlSanitizer InitSanitizer(IServiceProvider provider)
        {
            var sanitizer = new HtmlSanitizer();

            sanitizer.AllowedTags.Add("iframe");
            
            return sanitizer;
        }
        
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var devMode = builder.Environment.IsDevelopment();
            var mvcBuilder = builder.Services.AddControllersWithViews();

            builder.Services.AddHttpContextAccessor();
            
            builder.Services.AddAntiforgery();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/User/Login";
                    options.LogoutPath = "/User/Logout";

                    options.Cookie.Name = "_PEngineAuth_";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.IsEssential = true;
                });

            BlogContext.SetConnectionString(devMode ? 
                builder.Configuration.GetConnectionString("Development") :
                builder.Configuration.GetConnectionString("Production"));

            builder.Services.AddDbContext<BlogContext>();
            builder.Services.AddSingleton<IHtmlSanitizer, HtmlSanitizer>(InitSanitizer);
            
            if (devMode)
            {
                mvcBuilder.AddRazorRuntimeCompilation();
            }

            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            App = builder.Build();

            // Configure the HTTP request pipeline.
            if (!App.Environment.IsDevelopment())
            {
                App.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                App.UseHsts();
            }
            
            App.UseForwardedHeaders();
            
            App.UseHttpsRedirection();
            App.UseStaticFiles();
            App.UseStatusCodePagesWithReExecute("/Error/{0}");

            App.UseRouting();

            App.UseAuthentication();
            App.UseAuthorization();

            App.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            App.Run();
        }
    }
}