using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using PEngine.ViewModels;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using PEngine.Services;

namespace PEngine
{
    public static class Program
    {
        public static IConfiguration? WebsiteConfiguration { get; set; }

        private static readonly DirectoryInfo PEngineRoot;
        private static DirectoryInfo? DataDirRoot { get; set; }

        private const string DataDirectoryEnvName = "PENGINE_DATA_DIRECTORY";
        private const string DataDirectoryDefault = "PData";

        static Program()
        {
            PEngineRoot = new DirectoryInfo(Environment.CurrentDirectory);
        }

        private static void LoadConfiguration()
        {
            var dataDir = Environment.GetEnvironmentVariable(DataDirectoryEnvName) ?? DataDirectoryDefault;

            try
            {
                DataDirRoot = new DirectoryInfo(dataDir);
                var settingFilePath = $"{DataDirRoot.FullName}/appsettings.json";

                if (!DataDirRoot.Exists && DataDirRoot.FullName.Contains(PEngineRoot.FullName, StringComparison.InvariantCultureIgnoreCase))
                {
                    Directory.CreateDirectory(dataDir);
                    File.Copy("Resources/DefaultAppSettings.json", settingFilePath, true);
                }

                WebsiteConfiguration = new ConfigurationBuilder()
                                            .AddJsonFile(settingFilePath)
                                            .Build();
            }
            catch (Exception)
            {
                // TODO: Handle Configuration Loading Error;
                Environment.Exit(-1);
            }
        }

        public static IServiceCollection ConfigureBackendServices(this IServiceCollection services)
        {
            services.AddSingleton<PostService>()
                    .AddSingleton<CommentService>()
                    .AddSingleton<AttachmentService>()
                    .AddSingleton<UserService>();

            return services;
        }
        public static IServiceCollection ConfigureViewModels(this IServiceCollection services)
        {
            services.AddScoped<MainLayoutViewModel>();

            return services;
        }

        public static void Main(string[] args)
        {
            LoadConfiguration();

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.ConfigureViewModels()
                            .ConfigureBackendServices();
            builder.Services.AddLogging();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}