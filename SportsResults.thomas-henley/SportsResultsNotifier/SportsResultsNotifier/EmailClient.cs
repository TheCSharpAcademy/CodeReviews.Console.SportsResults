using MailKit.Net.Smtp;
using MimeKit;

namespace SportsResultsNotifier;

public class EmailClient(string email, string password)
{
    public void SendEmail(string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Nothing but .NET", "t4.crow@gmail.com"));
        message.To.Add (new MailboxAddress("Big Dog", "thomas.e.henley@gmail.com"));
        message.Subject = subject;
        message.Body = new TextPart ("html") { Text = body };

        using var client = new SmtpClient();
        client.Connect("smtp.gmail.com", 587, false);
        client.Authenticate(email, password);
        var response = client.Send(message);
        Console.WriteLine($"Server response:\n{response}\n\n");
        client.Disconnect(true);
    }
}