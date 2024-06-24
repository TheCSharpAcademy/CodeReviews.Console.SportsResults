using System.Net;
using System.Net.Mail;

namespace Phonebook.Services;

internal class EmailService
{
    private string _senderEmail;
    private string _senderPassword;
    private string _smtpHost;
    private int _smtpPort;

    public EmailService(string senderEmail, string senderPassword, string smtpHost, int smtpPort)
    {
        _senderEmail = senderEmail;
        _senderPassword = senderPassword;
        _smtpHost = smtpHost;
        _smtpPort = smtpPort;
    }

    internal void SendEmailAsPlainText(string subject, string body, string recipientEmail)
    {
        MailMessage mail = new MailMessage(_senderEmail, recipientEmail)
        {
            Subject = subject,
            Body = body,
        };

        SmtpClient smtpClient = new SmtpClient(_smtpHost, _smtpPort)
        {
            Credentials = new System.Net.NetworkCredential(_senderEmail, _senderPassword),
            EnableSsl = true,
        };

        try
        {
            smtpClient.Send(mail);
            Console.WriteLine("Email sent successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send email: {ex.Message}\n");
        }

    }

    internal async Task SendEmailAsHtml(string subject, string htmlBody, string recipientEmail)
    {
        using (var message = new MailMessage())
        {
            message.From = new MailAddress(_senderEmail);
            message.To.Add(new MailAddress(recipientEmail));
            message.Subject = subject;
            message.Body = htmlBody;
            message.IsBodyHtml = true;

            try
            {
                using (var client = new SmtpClient(_smtpHost, _smtpPort))
                {
                    client.Credentials = new NetworkCredential(_senderEmail, _senderPassword);
                    client.EnableSsl = true;
    
                    await client.SendMailAsync(message);
                    Console.WriteLine("Email sent successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}\n");
            }
        }
    }

}