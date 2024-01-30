using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SportsNotifier;

internal class Program
{
    static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
        {
            services.AddHostedService<ScoreResultsService>();
            services.AddSingleton<WebScrape>();
            services.AddSingleton<EmailSender>();
            services.AddSingleton<MessageBuilder>();
        }).Build();
        host.Run();
    }
}
