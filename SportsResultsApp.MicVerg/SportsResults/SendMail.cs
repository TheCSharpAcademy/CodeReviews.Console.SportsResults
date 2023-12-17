using System.Net;
using System.Net.Mail;

namespace SportsResults
{
    internal class SendMail
    {
        static string smtpAddress = "smtp.gmail.com";
        static int portNumber = 587;
        static bool enableSSL = true;
        static string emailFromAddress = "*"; //Sender Email Address  
        static string password = "*"; //Sender Password  
        static string emailToAddress = "*"; //Receiver Email Address  
        static string subject = "Winning NBA teams";
        static string body = "Hello, This is Email sending test using gmail.";
        internal static void SendEmail(string mailBody)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFromAddress);
                mail.To.Add(emailToAddress);
                mail.Subject = subject;
                mail.Body = mailBody;
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
