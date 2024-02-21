using MailKit.Net.Smtp;
using MimeKit;

namespace MailSender;

public class Demo
{
    public static void SendEmail(MailMessage message)
    {
        string emailSender = message.sender;
        string senderPassword = message.passwordSender;
        string emailReceiver = message.reciver;

        var email = new MimeMessage();

        email.From.Add(new MailboxAddress("Sender Name", emailSender));
        email.To.Add(new MailboxAddress("Receiver Name", emailReceiver));

        email.Subject = "Testing out email sending";
        email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
        {
            Text = message.message
        };
        using (var smtp = new SmtpClient())
        {
            smtp.Connect("smtp.gmail.com", 587, false);

            smtp.Authenticate(emailSender, senderPassword);

            smtp.Send(email);
            smtp.Disconnect(true);
        }
        Console.WriteLine("Done!");
    }
}
 
