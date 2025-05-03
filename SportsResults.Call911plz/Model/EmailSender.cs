using System.Net;
using System.Net.Mail;

public class EmailSender(string email, string password)
{
    private readonly string _email = email;
    private readonly string _password = password;

    public void SendToSelf(string subject, string content)
    {
        SmtpClient smtp = new("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential(_email, _password),
            EnableSsl = true,
        };
        MailAddress from = new(_email);
        MailAddress to = new(_email);

        MailMessage message = new(from, to)
        {
            Subject = subject,
            Body = content,
            IsBodyHtml = true,
        };

        try { smtp.Send(message); }
        catch (Exception e) { Console.WriteLine(e); }
    }
}