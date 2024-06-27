using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using SportsResults;

namespace Phonebook.Services;

public class EmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
        ValidateEmailSetting();
    }

    private void ValidateEmailSetting()
    {
        if(string.IsNullOrEmpty(_emailSettings.RecipientEmail) 
            || string.IsNullOrEmpty(_emailSettings.SenderEmail)
            || string.IsNullOrEmpty(_emailSettings.SenderPassword)
            || string.IsNullOrEmpty(_emailSettings.SmtpHost))
        {
            throw new ArgumentException(@"-------------
            Please set the below to run the app:
            dotnet user-secrets set EmailSettings:SenderEmail your-email@example.com
            dotnet user-secrets set EmailSettings:SenderPassword your-password
            dotnet user-secrets set EmailSettings:RecipientEmail recipient-email@example.com
            dotnet user-secrets set EmailSettings:SmtpHost smtp.example.com
            dotnet user-secrets set EmailSettings:SmtpPort 587 ");
        }
    }

    public void SendEmailAsPlainText(string subject, string body)
    {
        MailMessage mail = new MailMessage(_emailSettings.SenderEmail, _emailSettings.RecipientEmail)
        {
            Subject = subject,
            Body = body,
        };

        SmtpClient smtpClient = new SmtpClient(_emailSettings.SmtpHost, _emailSettings.SmtpPort)
        {
            Credentials = new System.Net.NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword),
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

    public async Task SendEmailAsHtml(string subject, string htmlBody)
    {
        using (var message = new MailMessage())
        {
            message.From = new MailAddress(_emailSettings.SenderEmail);
            message.To.Add(new MailAddress(_emailSettings.RecipientEmail));
            message.Subject = subject;
            message.Body = htmlBody;
            message.IsBodyHtml = true;

            try
            {
                using (var client = new SmtpClient(_emailSettings.SmtpHost, _emailSettings.SmtpPort))
                {
                    client.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
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