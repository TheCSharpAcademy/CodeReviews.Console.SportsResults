using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportResultsNotifier.Controllers;
using System.Configuration;

namespace SportResultsNotifier;

internal class Program
{
    static void Main(string[] args)
    {
        IHostBuilder builder = Host.CreateDefaultBuilder(args).ConfigureServices((h, s) =>
        {
            s.AddHostedService<Services>();
            s.AddSingleton<MailController>();
            s.AddSingleton<ResultsController>();
        }
        );
        IHost host = builder.Build();
        host.Run();
    }
}