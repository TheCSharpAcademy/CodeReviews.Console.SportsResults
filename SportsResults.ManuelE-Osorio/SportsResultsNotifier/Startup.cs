using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportsResultsNotifier.Controllers;
using SportsResultsNotifier.Models;
using SportsResultsNotifier.Services;
using SportsResultsNotifier.UI;

namespace SportsResultsNotifier;

public class StartUp
{
    public static IHost AppInit()
    {
        MainUI.LoadingMessage();

        var appBuilder = new HostBuilder();
        appBuilder.ConfigureAppConfiguration(p =>
            p.AddJsonFile("appsettings.json").Build());

        appBuilder.ConfigureServices((host, services) =>
        {
            var appVars = host.Configuration.GetSection("Settings").Get<AppVars>() ?? 
                throw new Exception();
            services.AddSingleton(appVars);
            services.AddScoped<EmailBuilderService>();
            services.AddScoped<WebCrawlerService>();
            services.AddScoped<DataController>();
        });

        return appBuilder.Build();
    }
}