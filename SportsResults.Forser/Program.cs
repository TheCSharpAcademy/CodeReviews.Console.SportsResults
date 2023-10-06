using SportsResults.Forser.Models;
using SportsResults.Forser.Services;

namespace SportsResults.Forser;

internal static class Program
{
    private static readonly IConfiguration configuration;

    static Program()
    {
        configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    }

    static void Main(string[] args)
    {
        using IHost host = CreateHostBuilder(args).Build();

        IServiceProvider resultServiceProvider = host.Services;
        host.Run();
    }
    static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args).ConfigureServices(services =>
        {
            services.AddWindowsService(options => { options.ServiceName = "SportsResults Background Worker"; });
            services.Configure<SettingsModel>(configuration.GetSection("AppSettings"));
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IScraper, Scraper>();
            services.AddSingleton<Notifier>();
            services.AddHostedService<BackgroundServiceWorker>();
        });
    }
}