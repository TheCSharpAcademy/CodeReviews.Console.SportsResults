using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;

namespace SportsResultsNotifier;

internal class EmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(EmailSettings emailSettings)
    {
        _emailSettings = emailSettings;
    }

    public void SendEmail(string subject, string body, [Optional] string attachmentPath)
    {
        try
        {
            using (var mail = new MailMessage())
            {
                mail.From = new MailAddress(_emailSettings.EmailFromAddress);
                mail.To.Add(_emailSettings.EmailToAddress);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                if (attachmentPath != null)
                {
                    if (File.Exists(attachmentPath))
                        mail.Attachments.Add(new Attachment(attachmentPath));
                }

                using (var smtp = new SmtpClient(_emailSettings.SmtpAddress, _emailSettings.PortNumber))
                {
                    smtp.Credentials = new NetworkCredential(_emailSettings.EmailFromAddress, _emailSettings.Password);
                    smtp.EnableSsl = _emailSettings.EnableSSL;
                    smtp.Send(mail);
                }
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Failed to send email: {ex.Message}");
            Console.ResetColor();
        }
    }
}
