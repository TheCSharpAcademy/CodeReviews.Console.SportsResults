using SportsResult.KroksasC.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SportsResult.KroksasC
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) => 
                {
                    services.AddHostedService<DailyTaskService>();
                }).Build();
           
            await host.RunAsync();
          
        }
    }
}