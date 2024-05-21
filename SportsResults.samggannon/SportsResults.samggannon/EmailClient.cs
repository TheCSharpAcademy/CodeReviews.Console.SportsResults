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

    public static void SendEmail()
    {
        using (MailMessage mail = new MailMessage())
        {
            mail.From = new MailAddress(email)
        }
    }

}
