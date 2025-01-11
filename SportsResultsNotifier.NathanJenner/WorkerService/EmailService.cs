using System.Net;
using System.Net.Mail;

namespace WorkerService;

internal class EmailService
{
    MailMessage MailMsg = new();

    static string smtpAddress = "smtp.gmail.com";
    static int portNumber = 587;
    static bool enableSSL = true;
    static string emailFromAddress = ""; //Sender Email Address  
    static string password = ""; //Sender Password  
    static string emailToAddress = ""; //Receiver Email Address  
    static string subject = "Today's Game Info";
    static string body = "";

    public static void GenerateEmailContent(List<GameModel> games)
    {
        foreach (var game in games)
        {
            body += $"{game.WinnerName}-{game.WinnerScore} vs {game.LoserName}-{game.LoserScore} || ";
        }
    }

    public static void SendEmail()
    {
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
        body = "";
    }
}