using System.Text;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SportsResults.Models;

namespace SportsResults.Services;
internal class EmailService
{
    private readonly IConfiguration _configuration;
    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendMessage(List<SportsResult> results)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Sports Notifier", _configuration["Email:Username"]));
            message.To.Add(new MailboxAddress("Recipient", _configuration["Email:Recipient"]));

            message.Subject = "Sport Results";

            var builder = new StringBuilder();

            foreach (SportsResult result in results)
            {
                builder.AppendLine($"<p>{result.Team1} - {result.Team2}</p>");
                builder.AppendLine($"<p>{result.ScoreTeam1} - {result.ScoreTeam2}</p>");
                builder.AppendLine($"<p>{result.Result}</p>");
                builder.AppendLine($"<a href='{result.LinkToBoxScore}'>Link to Box Score</a>");
                builder.AppendLine($"<span> | </span>");
                builder.AppendLine($"<a href='{result.LinkToPlayByPlay}'>Link to Play by Play</a>");
                builder.AppendLine($"<span> | </span>");
                builder.AppendLine($"<a href='{result.LinkToShotChart}'>Link to Shot Chart</a>");
                builder.AppendLine($"<div> =================================== </div>");
            }

            message.Body = new TextPart("html")
            {
                Text = builder.ToString()
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_configuration["Email:SmtpServer"], int.Parse(_configuration["Email:Port"]), MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_configuration["Email:Username"], _configuration["Email:Password"]);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
        catch (Exception ex) {
            await SendErrorMessage(ex.Message);
        }
    }

    public async Task SendErrorMessage(string text)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Sports Notifier", _configuration["Email:Username"]));
            message.To.Add(new MailboxAddress("Recipient", _configuration["Email:Recipient"]));

            message.Subject = "Sport Results - Error";

            var builder = new StringBuilder();
            builder.AppendLine($"<p>There has been an error while executing the script: {text}</p>");

            message.Body = new TextPart("html")
            {
                Text = builder.ToString()
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_configuration["Email:SmtpServer"], int.Parse(_configuration["Email:Port"]), MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_configuration["Email:Username"], _configuration["Email:Password"]);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        } catch (Exception ex) {
            throw new Exception($"Error {ex.Message}");
        }
    }
}
