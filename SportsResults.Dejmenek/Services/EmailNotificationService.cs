
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace SportsResults.Dejmenek.Services;
public class EmailNotificationService : IEmailNotificationService
{
    private readonly SmtpClient _smtpClient;
    private readonly MailMessage _mailMessage;

    public EmailNotificationService(IConfiguration configuration)
    {
        _smtpClient = new SmtpClient(
            configuration["SmtpConfiguration:Server"],
            int.Parse(configuration["SmtpConfiguration:Port"])
        );
        _smtpClient.EnableSsl = true;
        _smtpClient.UseDefaultCredentials = false;
        _smtpClient.Credentials = new NetworkCredential(
            configuration["SmtpConfiguration:SenderUsername"],
            configuration["SmtpConfiguration:SenderPassword"]
        );
        _mailMessage = new MailMessage(
            configuration["SmtpConfiguration:SenderUsername"],
            configuration["SmtpConfiguration:ReceiverEmail"]
        );
        _mailMessage.Subject = "Latest NBA Updates";
        _mailMessage.IsBodyHtml = true;
    }

    public void SendEmail(string body)
    {
        _mailMessage.Body = body;

        try
        {
            _smtpClient.Send(_mailMessage);
            Console.WriteLine("Email sent");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send email: {ex.Message}");
        }
    }
}
