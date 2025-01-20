using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SportsResults.StevieTV.Models;
using SportsResults.StevieTV.Scraper;

namespace SportsResults.StevieTV.EmailService;

class EmailService : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IGameScraper _gameScraper;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, IGameScraper gameScraper, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _gameScraper = gameScraper;
        _logger = logger;
    }

    public void SendEmail(string subject = "default subject", string content = "default content")
    {
        var emailConfig = _configuration.GetSection("Email");
        _logger.LogInformation("Sending Mail");

        using (var mail = new MailMessage())
        {
            mail.From = new MailAddress(emailConfig.GetValue<string>("Sender"));
            mail.To.Add(emailConfig.GetValue<string>("To"));
            mail.Subject = subject;
            mail.Body = content;
            mail.IsBodyHtml = true;
            
            _logger.LogDebug($"Sending mail to {mail.To} from {mail.From} with subject {mail.Subject}");

            using (var smtpClient = new SmtpClient(
                       emailConfig.GetValue<string>("SmtpServer"),
                       emailConfig.GetValue<int>("PortNumber")))
            {
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(
                    emailConfig.GetValue<string>("Sender"),
                    emailConfig.GetValue<string>("Password"));

                smtpClient.EnableSsl = emailConfig.GetValue<bool>("EnableSSL");
                
                _logger.LogTrace($"connecting to {smtpClient.Host}:{smtpClient.Port}");
                
                smtpClient.Send(mail);
            }
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("starting service");
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            _logger.LogInformation("Gathering Game Information");

            var url = _configuration.GetValue<string>("GameSource");
            var games = _gameScraper.GetGames(url);

            _logger.LogInformation($"{games.Count} games gathered");
            SendEmail($"Daily Games Update - {DateTime.Now.ToShortDateString()}", FormatEmailBody(games));
        }
    }

    private string FormatEmailBody(List<Game> games)
    {
        var intro =
            $@"<html>
                <body>
                <h1>Daily Baseball Games Update for {DateTime.Now.ToShortDateString()}</h1>
                <br />";

        const string tableIntro = @"<table>
                <thead>
                  <tr>
                    <th>Winner</th>
                    <th>Score</th>
                    <th>-</th>
                    <th>Score</th>
                    <th>Loser</th>
                  </tr>
                </thead>
                <tbody>
                ";
        var tableContent = "";

        foreach (var game in games)
        {
            tableContent +=
                $@"<tr>
                    <td>{game.Winner}</td>
                    <td>{game.WinnerScore}</td>
                    <td>-</td>
                    <td>{game.LoserScore}</td>
                    <td>{game.Loser}</td>
                  </tr>";
        }
        
        const string tableOutro = "</tbody></table>";
        
        const string outro = "</body></html>";

        var formattedMail = intro + tableIntro + tableContent + tableOutro + outro;
        return formattedMail;
    }
}