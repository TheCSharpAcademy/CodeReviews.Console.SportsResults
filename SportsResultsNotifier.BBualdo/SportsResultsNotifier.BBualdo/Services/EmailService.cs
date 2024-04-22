using System.Net;
using System.Net.Mail;

namespace SportsResultsNotifier.BBualdo.Services;

internal class EmailService : IEmailService
{
  public Task SendEmailAsync(string receiverMail, string subject, string message)
  {
    string email = System.Configuration.ConfigurationManager.AppSettings.Get("Email")!;
    string password = System.Configuration.ConfigurationManager.AppSettings.Get("Password")!;

    SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
    {
      EnableSsl = true,
      Credentials = new NetworkCredential(email, password)
    };

    return client.SendMailAsync(
    new MailMessage(from: email,
                    to: receiverMail,
                    subject,
                    message));
  }
}