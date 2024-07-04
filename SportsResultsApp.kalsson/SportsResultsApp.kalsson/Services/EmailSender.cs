using MimeKit;
using MailKit.Net.Smtp;
using SportsResultsApp.kalsson.Models;
using System.Text;
using SportsResultsApp.kalsson.Utils;

namespace SportsResultsApp.kalsson.Services;

public class EmailSender
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _senderEmail;
    private readonly string _senderPassword;
    private readonly string _recipientEmail;

    public EmailSender(string smtpServer, int smtpPort, string senderEmail, string senderPassword,
        string recipientEmail)
    {
        _smtpServer = smtpServer ?? throw new ArgumentNullException(nameof(smtpServer));
        _smtpPort = smtpPort;
        _senderEmail = senderEmail ?? throw new ArgumentNullException(nameof(senderEmail));
        _senderPassword = senderPassword ?? throw new ArgumentNullException(nameof(senderPassword));
        _recipientEmail = recipientEmail ?? throw new ArgumentNullException(nameof(recipientEmail));
    }

    public async Task SendEmailAsync(List<BasketballGameModel> games)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Sports Results Notifier", _senderEmail));
        message.To.Add(new MailboxAddress("Recipient", _recipientEmail));
        message.Subject = "Daily Basketball Game Results";

        var bodyBuilder = new BodyBuilder();
        var sb = new StringBuilder();
        sb.AppendLine("Here are the results of the basketball games played today:\n");

        foreach (var game in games)
        {
            sb.AppendLine(game.ToString());
        }

        bodyBuilder.TextBody = sb.ToString();
        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_senderEmail, _senderPassword);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}