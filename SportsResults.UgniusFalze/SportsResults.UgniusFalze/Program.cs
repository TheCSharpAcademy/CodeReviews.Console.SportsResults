using Microsoft.Extensions.Configuration;
using SportsResults.UgniusFalze.Services;
using SportsResults.UgniusFalze.Utils;

try
{
    var manager = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    var emailConfig = manager.GetSection("Email").Get<EmailConfig>();
    if (emailConfig == null)
    {
        throw new Exception("Failed to get email config from appsettings.json");
    }
    var scrapper = new ScrapperService();
    var games = scrapper.GetGames();
    if (games == null)
    {
        return;
    }

    var emailService = new EmailService(emailConfig);
    emailService.SendEmail(Formatter.FormatGames(games));
}
catch (Exception ex)
{
    Console.Error.WriteLine(ex.Message);
    return;
}

Console.WriteLine("Email sent!");