using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SportsResults.KamilKolanowski.Models;

namespace SportsResults.KamilKolanowski.Services;

internal class MailService
{
    private readonly WebScrapperService _webScrapper = new();

    internal void SendMail(string recipient)
    {
        var smtp = CreateSmtpSettings();

        var message = CreateMail(smtp.Username, recipient);

        using (var client = new SmtpClient())
        {
            client.Connect(smtp.Host, smtp.Port, smtp.UseSsl);
            client.Authenticate(smtp.Username, smtp.Password);
            client.Send(message);
            client.Disconnect(true);
        }

        Console.ReadKey();
        Console.Clear();
    }

    private SmtpSettings CreateSmtpSettings()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var smtpSettings = configuration.GetSection("Smtp").Get<SmtpSettings>();
        return smtpSettings;
    }

    private MimeMessage CreateMail(string sender, string recipient)
    {
        var message = new MimeMessage();
        var newMail = CreateMessage();
        message.From.Add(new MailboxAddress("SportsNotifier", sender));
        message.To.Add(new MailboxAddress("Receiver", recipient));
        message.Subject = newMail.Subject;
        message.Body = newMail.Body;

        return message;
    }

    private Mail CreateMessage()
    {
        var subject = "Sports Update";
        var htmlBody = _webScrapper.ScrapWebsite("https://www.basketball-reference.com/boxscores/");
        var body = new TextPart("html") { Text = htmlBody };

        var mail = new Mail { Subject = subject, Body = body };

        return mail;
    }
}
