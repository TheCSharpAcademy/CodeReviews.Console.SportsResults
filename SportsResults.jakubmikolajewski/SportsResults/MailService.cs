using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;

namespace SportsResults;
internal class MailService
{
    public void SendMailMessage(MimeMessage mailMessage)
    {
        using (var client = new SmtpClient())
        {
            client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            client.Authenticate("test.mailkit.jakub@gmail.com", "txpyzrrimltksywv");
            client.Send(mailMessage);
            client.Disconnect(true);
        }
    }

    public MimeMessage SetMessageProperties(string toName, string toEmail, string subject, string body)
    {
        var mailMessage = new MimeMessage();
        mailMessage.From.Add(new MailboxAddress("test account", "test.mailkit.jakub@gmail.com"));
        mailMessage.To.Add(new MailboxAddress(toName, toEmail));
        mailMessage.Subject = subject;
        mailMessage.Body = new TextPart("plain")
        {
            Text = body
        };

        return mailMessage;
    }
}
