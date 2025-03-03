using System.Net;
using System.Net.Mail;

namespace SportsResults;

public class EmailService
{
    private ConfigurationService _configurationService;

    public EmailService(ConfigurationService configurationService)
    {
        _configurationService = configurationService;
    }

    public void SendEmail(string subject, string body)
    {
        _configurationService.GetConfiguration();

        try
        {
            SmtpClient client = new SmtpClient(_configurationService.SmtpAddress, _configurationService.PortNumber)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_configurationService.EmailFromAddress, _configurationService.Password),
                EnableSsl = _configurationService.EnableSSL
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(_configurationService.EmailFromAddress),
                Subject = subject,
                Body = body
            };

            mailMessage.To.Add(_configurationService.EmailToAddress);
            client.Send(mailMessage);
            Console.WriteLine("Email sent successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error sending email: " + ex.Message);
        }
    }
}