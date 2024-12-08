using Microsoft.Extensions.Configuration;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace SportsResults.TwilightSaw.Controllers;

public class EmailController
{
    public void SendEmail(string Name, string Email, string emailText, string head)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(Name, configuration["CustomSettings:YourEmail"]));
        message.To.Add(new MailboxAddress("Me", Email));
        message.Subject = head;

        message.Body = new TextPart("plain")
        {
            Text = emailText
        };

        using var client = new SmtpClient();
        client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
        client.Authenticate(configuration["CustomSettings:YourEmail"], configuration["CustomSettings:YourAppPassword"]);
        client.Send(message);
        client.Disconnect(true);
    }
}