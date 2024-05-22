using System.Net;
using System.Net.Mail;

namespace SportsResults.samggannon;

internal class EmailClient
{
    private string _SmtpAddress;
    private string _SmtpPassword;
    private int _SmtpPort;
    private string _FromAddress;
    private string _ToAddress;
    private string _Subject;
    private string _Body;
    private bool _EnableSSL = true;

    public EmailClient(string smtpAddress, string smtpPassword, int smtpPort, string from, string to, string subject, string body)
    {
        _SmtpAddress = smtpAddress;
        _SmtpPassword = smtpPassword;
        _SmtpPort = smtpPort;
        _FromAddress = from;
        _ToAddress = to;
        _Subject = subject;
        _Body = body;
        _EnableSSL = true;
    }

    public void SendEmail()
    {
        using (MailMessage mail = new MailMessage())
        {
            mail.From = new MailAddress(_FromAddress);
            mail.To.Add(new MailAddress(_ToAddress));
            mail.Subject = _Subject;
            mail.Body = _Body;
            mail.IsBodyHtml = true;

            using (SmtpClient smtp = new SmtpClient(_SmtpAddress, _SmtpPort))
            {
                smtp.Credentials = new NetworkCredential(_FromAddress, _SmtpPassword);
                smtp.EnableSsl = _EnableSSL;
                smtp.Send(mail);
            }
        }
    }

}
