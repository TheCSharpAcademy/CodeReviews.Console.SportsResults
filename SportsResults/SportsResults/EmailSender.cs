using SportsResults.Service;
using System.Net.Mail;

namespace SportsResults;

public class EmailSender : IEmailSender
{
    string smtpAddress = "smtp.gmail.com";
    int portNumber = 587;
    bool enableSSL = true;
    public static string sender = "";
    public static string password = ""; //your password here (if you have active 2FA use an app password!)
    public static string target = ""; //receiver mail address
    public static string subject = "Daily Sport News";

    public void SendEmail(string to, string subject, string message)
    {
        try
        {
            using var smtpClient = new SmtpClient(smtpAddress)
            {
                Port = portNumber,
                Credentials = new System.Net.NetworkCredential(sender, password),
                EnableSsl = enableSSL
            };
            var mailMessage = new MailMessage(sender, to, subject, message);
            smtpClient.Send(mailMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}