using System.Net;
using System.Net.Mail;
using System.Text;
using SportsResults.Models;
using SportsResults.Utilities;

namespace SportsResults.Services;
public class EmailService
{
    private readonly ConfigReader _configReader;

    public EmailService(ConfigReader configReader)
    {
        _configReader = configReader;
    }

    public async Task SendSportsDataEmailAsync(List<SportData> data)
    {
        var emailSettings = _configReader.GetEmailSettings();
        var body = GenerateEmailBody(data);

        using var smtpClient = new SmtpClient(emailSettings.SmtpServer, emailSettings.SmtpPort)
        {
            Credentials = new NetworkCredential(emailSettings.FromAddress, emailSettings.Password),
            EnableSsl = true
        };

        using var message = new MailMessage(emailSettings.FromAddress, emailSettings.ToAddress)
        {
            Subject = "Daily Sport Results",
            Body = body,
            IsBodyHtml = true
        };

        await smtpClient.SendMailAsync(message);
    }

    private string GenerateEmailBody(List<SportData> data)
    {
        var sb = new StringBuilder();

        foreach (var sportData in data)
        {
            sb.AppendLine($"Sport: {sportData.Winner} ({sportData.WinnerScore}) vs. {sportData.Loser} ({sportData.LoserScore})");
            sb.AppendLine();
        }

        return sb.ToString();
    }
}