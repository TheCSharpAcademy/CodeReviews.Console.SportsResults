using System.Net;
using System.Net.Mail;

namespace SportsResultsNotifier.UI.Services;

public class EmailService
{
    private readonly string _smtpServer;
    private readonly int _port;
    private readonly string _username;
    private readonly string _password;
    private readonly string _from;
    private readonly bool _enableSsl;

    public EmailService(string smtpServer, int port, string username, string password, string from, bool enableSsl)
    {
        _smtpServer = smtpServer;
        _port = port;
        _username = username;
        _password = password;
        _from = from;
        _enableSsl = enableSsl;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        using var smtp = new SmtpClient(_smtpServer, _port)
        {
            Credentials = new NetworkCredential(_username, _password),
            EnableSsl = _enableSsl
        };

        var mail = new MailMessage(_from, to, subject, body);

        await smtp.SendMailAsync(mail);
    }
}
