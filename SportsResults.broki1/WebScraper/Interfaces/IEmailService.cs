using System.Net.Mail;
using WebScraper.Models;

namespace WebScraper.Interfaces;

public interface IEmailService
{
    public string CreateBody(List<BasketballGame> games);

    public MailMessage CreateEmail(string emailBody);

    public void SendEmail(MailMessage email);
}
