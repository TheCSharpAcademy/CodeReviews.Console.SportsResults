using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using Spectre.Console;
using Sports_Results_Notifier.Models;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Sports_Results_Notifier.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetFromEmailName()
    {
        return _configuration["EmailService:FromEmailName"];
    }

    public string GetFromEmailAddress()
    {
        return _configuration["EmailService:FromEmailAddress"];
    }

    public string GetPassword()
    {
        return _configuration["EmailService:Password"];
    }

    public string GetToEmailAddress()
    {
        return _configuration["EmailService:ToEmailAddress"];
    }

    public string GetSmtpAddress()
    {
        return _configuration["EmailService:SmtpAddress"];
    }

    public int GetSmtpPort()
    {
        return int.Parse(_configuration["EmailService:SmtpPort"]);
    }

    public void SendEmail(Game game)
    {
        var email = new MimeMessage();
        var date = DateTime.Now.ToString("yyyy-MM-dd");
        var emailRecipient = GetToEmailAddress();

        email.From.Add(new MailboxAddress(GetFromEmailName(), GetFromEmailAddress()));
        email.To.Add(MailboxAddress.Parse(emailRecipient));

        var emailSubject = $"Latest Game Results for {date}";
        var emailBody =
            $"<p>{game.WinningTeam} defeats {game.LosingTeam} with the final score: {game.WinnerScore} - {game.LoserScore}</p>";

        email.Subject = emailSubject;
        email.Body = new TextPart(TextFormat.Html) { Text = emailBody };

        using (var smtp = new SmtpClient())
        {
            smtp.Connect(GetSmtpAddress(), GetSmtpPort());

            var smtpUsername = GetFromEmailAddress();
            var smtpPassword = GetPassword();
            smtp.Authenticate(smtpUsername, smtpPassword);

            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
