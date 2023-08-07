using System.Net.Mail;

namespace Kmakai.SportsNotifier;

public class Mailer
{
    private string Email { get; set; }
    private string Password { get; set; }
    private string Subject { get; set; } = "Sports Results";

    public Mailer(string email, string password)
    {
        Email = email;
        Password = password;
    }


    public async void SendEmail(string toEmail, string body)
    {
        var client = new SmtpClient("smtp.gmail.com", 587)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new System.Net.NetworkCredential(Email, Password)
        };

        var message = new MailMessage(Email, toEmail)
        {
            Subject = Subject,
            Body = body
        };

        await client.SendMailAsync(message);
    }
}
