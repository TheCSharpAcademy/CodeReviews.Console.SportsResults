using System.Net;
using System.Net.Mail;

namespace SportsResults.frockett;

internal class EmailService
{
    // SMPT Address and port number are examples
    static string smptAddress = "smtp.gmail.com";
    static int portNumber = 587;
    static bool enableSSL = true;
    static string emailFromAddress = "";
    static string password = "";
    static string emailToAddress = "";

    public static void SendEmail(string emailBody)
    {
        try
        {
            using MailMessage mail = new MailMessage();
            mail.From = new MailAddress(emailFromAddress);
            mail.To.Add(emailToAddress);
            mail.Subject = $"{DateTime.Now.ToString("MMM dd, yyyy")} Basketball Results";

            mail.Body = emailBody;
            mail.IsBodyHtml = true;

            using SmtpClient smtp = new SmtpClient(smptAddress, portNumber);
            smtp.Credentials = new NetworkCredential(emailFromAddress, password);
            smtp.EnableSsl = enableSSL;
 
            smtp.Send(mail);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send email. Exception: {ex.Message}");
            Console.WriteLine(ex.ToString());
        }
    }
}
