using SportsResult.Vocalnight;
using System.Configuration;
using System.Net.Mail;
using System.Net;

partial class Program
{
    static void SendEmail( string title, List<Game> matches )
    {

        Console.WriteLine("Sending Email...");

        var smtpAddress = ConfigurationManager.AppSettings.Get("SMTPAdress");
        int portNumber = 587;
        bool enableSSL = true;
        var emailFromAddress = ConfigurationManager.AppSettings.Get("SentEmail");
        var password = ConfigurationManager.AppSettings.Get("ReceiveEmail");
        var emailToAddress = ConfigurationManager.AppSettings.Get("Password");
        string subject = title;
        string body = SendBody(matches);

        Console.WriteLine(body);


        using (MailMessage mail = new MailMessage())
        {
            mail.From = new MailAddress(emailFromAddress);
            mail.To.Add(emailToAddress);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
            {
                smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                smtp.EnableSsl = enableSSL;
                smtp.Send(mail);
            }
        }

        Console.WriteLine("Email Sent!");
    }

    static string SendBody( List<Game> matches )
    {
        string message = "<hr />";

        foreach (Game match in matches)
        {
            message += $"<p>{match.MatchResults()}</p>";
        }

        message += "<p>That's all for today. Thanks for subscribing!</p>";
        return message;
    }
}



