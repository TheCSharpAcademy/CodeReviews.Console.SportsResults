using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportsResults;
using SportsResults.Service;

class Program
{
    static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<Service>();
                services.AddSingleton<IEmailSender, EmailSender>();
                services.AddSingleton<Scraper> ();
            })
            .Build();

        host.Run();
    }
}