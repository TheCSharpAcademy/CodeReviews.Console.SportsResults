using System.Configuration;
using System.Net;
using System.Net.Mail;
using WebScraper.Interfaces;
using WebScraper.Models;

namespace WebScraper.Services;

internal class BasketballEmailService : IEmailService
{
    public string CreateBody(List<BasketballGame> games)
    {
        var date = games[0].Date.ToString("MMMM dd, yyyy");
        var header = $"Games played on {date}:\n\n";

        var body = "";

        foreach (var game in games)
        {
            var gameEntry = @$"{game.AwayTeam}: {game.AwayTeamScore}
at 
{game.HomeTeam}: {game.HomeTeamScore}

";

            body += gameEntry;
        }

        return header + body;
    }

    public MailMessage CreateEmail(string emailBody)
    {
        var email = new MailMessage();
        var fromEmail = new MailAddress(ConfigurationManager.AppSettings.Get("fromEmail"));
        var toEmail = new MailAddress(ConfigurationManager.AppSettings.Get("toEmail"));

        email.From = fromEmail;
        email.To.Add(toEmail);

        email.Subject = $"Daily NBA Report for {DateTime.Now.AddDays(-1).ToString("MMMM dd, yyyy")}";
        email.Body = emailBody;

        return email;
    }

    public void SendEmail(MailMessage email)
    {
        var fromEmail = ConfigurationManager.AppSettings.Get("fromEmail");
        var fromPassword = ConfigurationManager.AppSettings.Get("fromPassword");

        // sets up the Smtp Client
        SmtpClient smtp = new SmtpClient();
        smtp.Host = "smtp.mail.yahoo.com";
        smtp.Port = 587;
        // tells Smtp Client that specific credentials will be used
        smtp.UseDefaultCredentials = false;

        // required, email needs to be secure/encrypted
        smtp.EnableSsl = true;
        // fromEmail credentials set
        smtp.Credentials = new NetworkCredential(fromEmail, fromPassword);

        try
        {
            smtp.Send(email);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex}");
        }
    }
}
