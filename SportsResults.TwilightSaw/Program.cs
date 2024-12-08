using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportsResults.TwilightSaw.Builders;
using SportsResults.TwilightSaw.Controllers;
using SportsResults.TwilightSaw.Senders;

Host.CreateDefaultBuilder(args).ConfigureServices((context, services) =>
{
    services.AddHostedService<EmailSender>();
    services.AddSingleton<ScrapperController>();
    services.AddSingleton<EmailController>();
    services.AddSingleton<MessageBuilder>();
})
    .Build()
    .Run();
