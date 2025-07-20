using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace SportResults.Mailer;

public class Mailer
{

  private static IConfiguration GetConfiguration()
  {
    return new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();
  }

  public static void SendEmail(string body)
  {
    var config = GetConfiguration();
    var mail = config.GetSection("Mail");
    
    // Extract all config values first
    var fromAddress = mail["From"] ?? throw new ArgumentException("From address not configured");
    var toAddress = mail["To"] ?? throw new ArgumentException("To address not configured");
    var subject = mail["Subject"] ?? "NBA Game Results";
    var host = mail["Host"] ?? "smtp.gmail.com";
    var password = mail["Password"] ?? throw new ArgumentException("Password not configured");
    var port = int.TryParse(mail["Port"], out int p) ? p : 587;
    var enableSsl = bool.TryParse(mail["EnableSsl"], out bool ssl) ? ssl : true;

    using MailMessage mailMessage = new(fromAddress, toAddress, subject, body)
    {
        IsBodyHtml = true
    };

    Console.WriteLine("Sending email...");

    using SmtpClient smtpClient = new(host, port)
    {
        Credentials = new NetworkCredential(fromAddress, password),
        EnableSsl = enableSsl
    };
    smtpClient.Send(mailMessage);
  }
}