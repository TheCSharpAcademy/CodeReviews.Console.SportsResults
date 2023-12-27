using System.Net.Mail;
using SportsResults.UgniusFalze.Utils;

namespace SportsResults.UgniusFalze.Services;

public class EmailService(EmailConfig config)
{
    public void SendEmail(string body)
    {
        var subject = DateTime.Today.ToShortDateString() + " nba games";
        using var mail = new MailMessage();
        mail.From = new MailAddress(config.EmailFrom);  
        mail.To.Add(config.EmailTo);  
        mail.Subject = subject;  
        mail.Body = body;  
        mail.IsBodyHtml = true;
        using var smtp = new SmtpClient(config.SMTP, config.PortNumber);
        smtp.Credentials = new System.Net.NetworkCredential(config.EmailFrom, config.AppPassword);  
        smtp.EnableSsl = true;  
        smtp.Send(mail);
    }
}