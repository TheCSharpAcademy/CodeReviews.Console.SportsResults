using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace SportsResultsNotifier;

public class EmailSender
{
    private readonly DateTime currentDate = DateTime.Today;
    private readonly IConfiguration _config;
    private readonly string _smtpHost;
    private readonly int _port;
    private readonly string _fromAddress;
    private readonly string _toAddress;
    private readonly string _smtpPass;
    private readonly string _smtpUser;

    public EmailSender(IConfiguration config)
    {
        _config = config;
        _smtpHost = _config["EmailSettings:SmtpHost"]!;
        _port = int.Parse(_config["EmailSettings:SmtpPort"]!);
        _fromAddress = _config["EmailSettings:FromAddress"]!;
        _toAddress = _config["EmailSettings:ToAddress"]!;
        _smtpPass = _config["EmailSettings:SmtpPass"]!;
        _smtpUser = _config["EmailSettings:SmtpUser"]!;
    }

    public void SendEmail(string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Martin - Daily Basketball News", _fromAddress));
        message.To.Add(new MailboxAddress("mxrt0", _toAddress));
        message.Subject = $"Daily NBA Games Report ({currentDate.ToString("dd-MM-yyyy")})";
        message.Body = new TextPart("html")
        {
            Text = body,

        };

        try
        {
            using var client = new SmtpClient();
            client.Connect(_smtpHost, _port, MailKit.Security.SecureSocketOptions.StartTls);
            client.Authenticate(_smtpUser, _smtpPass);
            client.Send(message);
            client.Disconnect(true);
            Console.WriteLine($"\nSuccessfully sent email to: {_toAddress}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError while sending email: {ex.Message}!");
        }
    }
}
