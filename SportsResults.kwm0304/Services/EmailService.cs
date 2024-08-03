using MailKit.Net.Smtp;
using MimeKit;
using Quartz;
using Spectre.Console;

namespace SportsResults.kwm0304.Services;

public class EmailService : IJob
{
  private readonly ScraperService _scraperService;

  public EmailService(ScraperService scraperService)
  {
    
    _scraperService = scraperService;
  }

  public async Task Execute(IJobExecutionContext context)
  {
    await CreateAndSendMessage();
  }

  private async Task CreateAndSendMessage()
  {
    var server = Environment.GetEnvironmentVariable("Server");
    var port = Environment.GetEnvironmentVariable("PORT");
    int portNum = int.Parse(port!);
    var from = Environment.GetEnvironmentVariable("EMAIL_FROM");
    var fromPassword = Environment.GetEnvironmentVariable("EMAIL_FROM_PASSWORD");
    var to = Environment.GetEnvironmentVariable("EMAIL_TO");

    var message = new MimeMessage();
    message.From.Add(new MailboxAddress("", from));
    message.To.Add(new MailboxAddress("", to));
    message.Subject = $"Games for {DateTime.Today.ToShortDateString()}";
    message.Body = new TextPart("plain")
    {
      Text = _scraperService.Scrape()
    };

    using var client = new SmtpClient();
    try
    {
      await client.ConnectAsync(server, portNum, MailKit.Security.SecureSocketOptions.StartTls);
      await client.AuthenticateAsync(from, fromPassword);
      await client.SendAsync(message);
      await client.DisconnectAsync(true);
    }
    catch (Exception e)
    {
      AnsiConsole.WriteException(e);
    }
  }
}