using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sports_Result_Notifier.Lawang;

var host = Host.CreateDefaultBuilder(args).ConfigureServices((hostcontext, services) => {
    services.AddHostedService<NotifyBackgroundService>();
}).Build();

await host.RunAsync();




