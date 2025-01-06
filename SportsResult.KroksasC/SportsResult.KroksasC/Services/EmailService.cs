using System.Net.Mail;
using System.Net;
using System.Configuration;
using System.Text;
using SportsResult.KroksasC.Models;

namespace SportsResult.KroksasC.Services
{
    internal class EmailService
    {
        public static async Task SendEmail()
        {
            var fromEmail = ConfigurationManager.AppSettings.Get("email");
            var fromEmailPassword = ConfigurationManager.AppSettings.Get("emailPassword");

            // sets up the Smtp Client
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            // tells Smtp Client that specific credentials will be used
            smtp.UseDefaultCredentials = false;

            // required, email needs to be secure/encrypted
            smtp.EnableSsl = true;
            // fromEmail credentials set
            smtp.Credentials = new NetworkCredential(fromEmail, fromEmailPassword);
            var email = await BuildEmail();
            try
            {
                smtp.Send(email);
                Console.WriteLine("Email sent successfully!");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                Console.ReadLine();
            }
        }
        public static async Task<MailMessage> BuildEmail()
        {
            List<SportResult> sportResult = await ScraperService.GetSportResults("https://www.basketball-reference.com/boxscores/");

            var fromEmail = new MailAddress(ConfigurationManager.AppSettings.Get("email"));

            var email = new MailMessage();
            email.From = fromEmail;
            email.To.Add(ConfigurationManager.AppSettings.Get("emailToSendTo"));

            email.Subject = "NBA basketball result!";
            StringBuilder sb = new StringBuilder();
            foreach (var result in sportResult)
            {
                sb.Append(@$"Winner: {result.Winner}, Points: {result.WinnerPoints}
Loser: {result.Loser}, Points: {result.LoserPoints}

");
            }

            email.Body = sb.ToString();

            return email;
        }
    }
}
