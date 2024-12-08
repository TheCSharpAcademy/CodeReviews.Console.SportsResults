using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportsResults.TwilightSaw.Builders;
using SportsResults.TwilightSaw.Controllers;
using SportsResults.TwilightSaw.Senders;

var scrapperController = new ScrapperController();
var emailController = new EmailController();
var messageBuilder = new MessageBuilder(scrapperController);
var scrapperService = new EmailSender(emailController, messageBuilder);

Host.CreateDefaultBuilder(args).ConfigureServices((context, services) =>
{
    services.AddHostedService<EmailSender>();
    services.AddSingleton<ScrapperController>();
    services.AddSingleton<EmailController>();
    services.AddSingleton<MessageBuilder>();
})
    .Build()
    .Run();
