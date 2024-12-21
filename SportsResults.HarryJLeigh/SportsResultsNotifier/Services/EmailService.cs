using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Configuration;
using SportsResultsNotifier.Models;
using SportsResultsNotifier.Util;

namespace SportsResultsNotifier.Services;

public class EmailService
{
    private readonly EmailSettings _emailSettings = EmailExtensions.LoadEmailSettings();
    
    public void Send(List<GameResult> results)
    {
        MailMessage mail = new MailMessage
        {
            From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
            Subject = _emailSettings.Subject,
            Body = CreateBody(results),
            IsBodyHtml = true
        };
        mail.To.Add(_emailSettings.ReceiverEmail);

        using SmtpClient client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port);
        client.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.Password);
        client.EnableSsl = _emailSettings.UseSsL;
        client.Send(mail);
    }

    private string CreateBody(List<GameResult> results)
    {
        if (results.Count == 0)
            return "<p>No results found.</p>";

        var bodyBuilder = new StringBuilder();

        foreach (GameResult result in results)
        {
            bodyBuilder.AppendLine($"<p>Winner: {result.Winner} - Score: {result.WinnerScore}<br>");
            bodyBuilder.AppendLine($"Loser: {result.Loser} - Score: {result.LoserScore}</p>");
        }

        return bodyBuilder.ToString();
    }
}