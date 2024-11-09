using System.Net;
using System.Net.Mail;
using SportsNotifier.hasona23.Config;

namespace SportsNotifier.hasona23.EmailSender;

public class EmailSender : IDisposable
{
    private readonly ILogger<EmailSender> _logger;
    private readonly SmtpClient _smtpClient;

    public EmailSender(ILogger<EmailSender> logger)
    {
        _logger = logger;
        _smtpClient = new SmtpClient(AppSetting.SmtpHost)
        {
            Port = AppSetting.SmtpPort,
            Credentials = new NetworkCredential(AppSetting.SmtpUsername, AppSetting.SmtpPassword),
            EnableSsl = true,
        };
        _logger.LogInformation("Email Sender started");
    }
    public void SendEmail(string toMailAdd, string subject, string body)
    {
        MailAddress from = new(AppSetting.SmtpUsername, "SportsNotifier");
        MailAddress to = new(toMailAdd);
        using MailMessage mailMessage = new MailMessage(from,to);
        mailMessage.IsBodyHtml = false;
        mailMessage.Subject = subject;
        mailMessage.Body = body;
        try
        {
            _smtpClient.Send(mailMessage);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
    }

    public void Dispose()
    {
        _smtpClient.Dispose();
        _logger.LogInformation("Email Sender disposed");
    }
}