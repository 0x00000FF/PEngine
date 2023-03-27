using PEngine.ViewModels;
using System.Reflection;
using PEngine.Persistence;
using PEngine.Repositories;
using PEngine.Services;
using PEngine.States;

namespace PEngine
{
    public static class Program
    {
        public static string? CurrentAssemblyPath { get; }
        public static string CurrentGitHash { get; }
        public static IConfiguration? WebsiteConfiguration { get; set; }

        private static readonly DirectoryInfo PEngineRoot;
        private static DirectoryInfo? DataDirRoot { get; set; }

        private const string DataDirectoryEnvName = "PENGINE_DATA_DIRECTORY";
        private const string DataDirectoryDefault = "PData";

        static Program()
        {
            PEngineRoot = new DirectoryInfo(Environment.CurrentDirectory);
            CurrentAssemblyPath = new DirectoryInfo(Assembly.GetExecutingAssembly().Location).Parent?.FullName;

            if (CurrentAssemblyPath is not null)
            {
                CurrentGitHash = File.ReadAllText(CurrentAssemblyPath + "/githash.txt");
            }
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

        public static IServiceCollection ConfigureStateModels(this IServiceCollection services)
        {
            return services.AddScoped<UserContext>();
        }
        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            return services.AddSingleton<UserRepository>();
        }
        
        public static IServiceCollection ConfigureBackendServices(this IServiceCollection services)
        {
            return services.AddScoped<PostService>()
                           .AddScoped<CommentService>()
                           .AddScoped<AttachmentService>()
                           .AddScoped<UserService>();
        }
        public static IServiceCollection ConfigureViewModels(this IServiceCollection services)
        {
            return services.AddScoped<MainLayoutViewModel>()
                           .AddScoped<ExplorerViewModel>();
        }

        public static void Main(string[] args)
        {
            LoadConfiguration();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = UserContext.TOKEN_COOKIE;
                options.Cookie.IsEssential = true;
            });
            
            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();

            builder.Services
                .ConfigureStateModels()
                .ConfigureRepositories()
                .ConfigureViewModels()
                .ConfigureBackendServices();
            
            builder.Services.AddLogging();
            builder.Services.AddSingleton<DatabaseContext>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseSession();
            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}