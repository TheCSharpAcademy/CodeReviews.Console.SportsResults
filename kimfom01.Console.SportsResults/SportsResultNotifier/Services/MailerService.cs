using System.Net;
using System.Net.Mail;
using System.Security;
using SportsResultNotifier.Models;

namespace SportsResultNotifier.Services;

public class MailerService : IMailerService
{
    private readonly string _userName;
    private SecureString _password;
    private SmtpClient _client;

    public MailerService(string userName, string password)
    {
        _userName = userName;
        _password = GetSecurePassword(password);
        _client = GetClient();
    }

    private SmtpClient GetClient()
    {
        var client = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential(_userName, _password),
            EnableSsl = true
        };

        return client;
    }

    private SecureString GetSecurePassword(string password)
    {
        var secureString = new SecureString();

        foreach (var character in password)
        {
            secureString.AppendChar(character);
        }

        return secureString;
    }

    public bool SendEmail(Message message, string recipient)
    {
        _client.Send(_userName, recipient, message.Title, message.MessageBody);

        return true;
    }
}
