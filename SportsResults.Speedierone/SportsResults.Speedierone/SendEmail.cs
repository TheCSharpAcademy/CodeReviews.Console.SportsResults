using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsResults.Speedierone
{
    public class SendEmail
    {
        static string smtpAddress = "smtp.gmail.com";
        static int portNumber = 587;
        static bool enableSSL = true;
        static string emailFromAddress = Helpers.GetUserInput("\nPlease enter email to send from");
        static string password = Helpers.GetUserInput("\nPlease enter password\n");
        static string emailToAddress = Helpers.GetUserInput("\nPlease enter email address to send to.");
        static string subject = "Todays NBA Results";

        public static void SendResultEmail(string body)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFromAddress);
                mail.To.Add(new MailAddress(emailToAddress));
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
        }
    }
}
