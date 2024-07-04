using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportsResultsApp.kalsson;
using SportsResultsApp.kalsson.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHttpClient();
        services.AddSingleton<DataScraper>();
        services.AddSingleton<EmailSender>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            return new EmailSender(
                smtpServer: configuration["Email:SmtpServer"],
                smtpPort: int.Parse(configuration["Email:SmtpPort"]),
                senderEmail: configuration["Email:SenderEmail"],
                senderPassword: configuration["Email:SenderPassword"],
                recipientEmail: configuration["Email:RecipientEmail"]);
        });
        services.AddHostedService<ScoreResultsService>();
    })
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .Build();

await host.RunAsync();