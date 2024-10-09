using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace SportsResultsNotifier.Services;

public class MailService
{
    private readonly ILogger<MailService> _logger;
    private readonly string _sender;
    private readonly string _password;
    private readonly string _recipient;
    private readonly string _smtpAddress;
    private readonly int _port;

    public MailService(IConfiguration config, ILogger<MailService> logger)
    {
        _logger = logger;

        var section = config.GetSection("MailConfiguration");
        _sender = section["SenderEmail"];
        _password = section["SenderPassword"];
        _recipient = section["RecipientEmail"];
        _smtpAddress = section["SmtpAddress"];
        _port = int.Parse(section["SmtpPort"]);

        if (_sender is null || _password is null || _recipient is null || _smtpAddress is null)
        {
            var ex = new ArgumentNullException(nameof(config), "Config is invalid. Please edit appsettings.json with desired mail config.");
            _logger.LogError(ex, "ArgumentNullException. Argument: {config}", ex.ParamName);
            throw ex;
        }
    }

    public string BuildEmailBody(List<List<string>> boxScores)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<h1>Scores update:</h1>");
        sb.AppendLine("<small>This is an automated email. Please do not reply.</small>\n");

        for (int i = 0; i < boxScores.Count; i++)
        {
            var conference = i == 0 ? "East" : "West";
            sb.AppendLine($"<h2>{conference} conference scores:</h2>\n");

            foreach (var score in boxScores[i])
            {
                sb.AppendLine($"<p>{score}</p>");
            }

            sb.AppendLine();
        }
        sb.AppendLine("<small>* Playoff teams</small>");

        return $"<html><body>\n{sb}</body></html>";
    }

    public MailMessage CreateEmail(string body)
    {
        var mail = new MailMessage();

        mail.Subject = $"{DateTime.Now:d} Score update";
        mail.Body = body;
        mail.Sender = new MailAddress(_sender);
        mail.From = mail.Sender;
        mail.To.Add(new MailAddress(_recipient));
        mail.IsBodyHtml = true;

        return mail;
    }
    
    public void SendEmail(MailMessage mail)
    {
        try
        {
            using var client = new SmtpClient(_smtpAddress, _port);
            client.Credentials = new NetworkCredential(_sender, _password);
            client.EnableSsl = true;
            client.Send(mail);
            _logger.LogInformation("Email sent successfully");
        }
        catch (SmtpException smtpEx)
        {
            _logger.LogError(smtpEx, "SmtpException: Unable to send mail from address {sender}.", _sender);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "");
            throw;
        }
    }

    public void SendSportsUpdateAsync(List<List<string>> boxScores)
    {
        var body = BuildEmailBody(boxScores);
        var mail = CreateEmail(body);
        SendEmail(mail);
    }
}