using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace SportsResults;

public class Mailer
{
    private readonly string SenderEmail;
    private readonly string SenderPassword;
    private readonly string ReceiverEmail;

    public Mailer()
    {
        IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        SenderEmail = configuration.GetSection("Mailer")["SenderEmail"] ?? string.Empty;
        SenderPassword = configuration.GetSection("Mailer")["SenderPassword"] ?? string.Empty;
        ReceiverEmail = configuration.GetSection("Mailer")["ReceiverEmail"] ?? string.Empty;
    }

    public void SendEmail(string subject, string body)
    {
        using MailMessage email = new();
        email.From = new MailAddress(SenderEmail);
        email.To.Add(ReceiverEmail);
        email.Subject = subject;
        email.Body = body;
        email.IsBodyHtml = true;

        using SmtpClient smtp = new("smtp.gmail.com", 587);
        smtp.Credentials = new NetworkCredential(SenderEmail, SenderPassword);
        smtp.EnableSsl = true;

        try
        {
            smtp.Send(email);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
        }
    }
}