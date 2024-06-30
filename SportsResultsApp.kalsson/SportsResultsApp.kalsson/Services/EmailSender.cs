using MimeKit;
using MailKit.Net.Smtp;
using SportsResultsApp.kalsson.Models;
using System.Text;
using SportsResultsApp.kalsson.Utils;

namespace SportsResultsApp.kalsson.Services;

public class EmailSender
{
    /// <summary>
    /// Asynchronously sends an email containing the basketball games results.
    /// </summary>
    /// <param name="games">The list of basketball games to include in the email.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SendEmailAsync(List<BasketballGameModel> games)
    {
        var config = ConfigurationHelper.GetEmailSettings();
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Sports Results Notifier", config.SenderEmail));
        message.To.Add(new MailboxAddress("Recipient", config.RecipientEmail));
        message.Subject = "Daily Basketball Game Results";

        var bodyBuilder = new BodyBuilder();
        var sb = new StringBuilder();

        sb.AppendLine("Here are the results of the basketball games played today:\n");

        foreach (var game in games)
        {
            sb.AppendLine($"{game.Team1} {game.Team1Score} - {game.Team2Score} {game.Team2}");
            Console.WriteLine($"Adding game to email: {game.Team1} {game.Team1Score} - {game.Team2Score} {game.Team2}");
        }

        bodyBuilder.TextBody = sb.ToString();

        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(config.SmtpServer, config.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(config.SenderEmail, config.SenderPassword);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}