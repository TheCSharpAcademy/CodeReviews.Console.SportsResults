using System.Net;
using System.Net.Mail;

namespace SportsScraper;

public class Mailer
{
    static bool enableSSL = true;

    public static void SendEmail(string body)
    {
        using (MailMessage mail = new MailMessage())
        {
            mail.From = new MailAddress(ConfigManager.Config.FromEmailAddress);
            mail.To.Add(ConfigManager.Config.OutboundEmailAddress);
            mail.Subject = "Your daily NBA box scores update";
            mail.Body = body;
            mail.IsBodyHtml = true;
            using (SmtpClient smtp = new SmtpClient(
                    ConfigManager.Config.SmtpAddress,
                    int.Parse(ConfigManager.Config.SmtpPort)
                )
            )
            {
                smtp.UseDefaultCredentials = false;

                smtp.Credentials = new NetworkCredential(
                    ConfigManager.Config.OutboundEmailAddress,
                    ConfigManager.Config.FromEmailPassword
                );
                smtp.EnableSsl = enableSSL;
                smtp.Send(mail);
            }
        }
    }
}