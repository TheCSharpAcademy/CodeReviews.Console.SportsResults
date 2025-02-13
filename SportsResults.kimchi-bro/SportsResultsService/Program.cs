using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using System.Reflection;

namespace SportsResultsService;

public class Program
{
    public static async Task Main(string[] args)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var stream = assembly.GetManifestResourceStream($"SportsResultsService.appsettings.json");

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                if (stream != null)
                {
                    config.AddJsonStream(stream);
                }
                else
                {
                    throw new FileNotFoundException("appsettings.json file not found");
                }
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddWindowsService(options =>
                {
                    options.ServiceName = ".NET Sports Results Emailing Service";
                });
                services.AddSingleton<EmailingService>();
                services.AddHostedService<WindowsBackgroundService>();

                LoggerProviderOptions.RegisterProviderOptions<
                    EventLogSettings, EventLogLoggerProvider>(services);
            })
            .Build();

        stream?.Dispose();

        await host.RunAsync();
    }
}
