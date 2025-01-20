using System.Net.Mail;
using System.Net;
using SportsResultNotifierCarDioLogic.Model;
using SportsResultNotifierCarDioLogic.Helpers;

namespace SportsResultNotifierCarDioLogic;

public class EmailService
{
    public (string subject, string body) PrepareContent(string date = "")
    {
        if(date == "")
        {
            date = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
        }

        WebScrapper webScrapper = new WebScrapper();

        (int year, int month, int day) = DateConverterHelper.ConvertDateForScrapper(date);
        List<Game> games = webScrapper.Scrapper(year, month, day);

        string subject = $"NBA games played on: {date}";
        string body = "CarDioLogic's Sports Notifier App:" + "\n";

        if (games != null)
        {
            foreach (Game game in games)
            {
                body += "Team: " + game.TeamA + "  Score: " + game.ScoreTeamA + "\n"
                        + "Team: " + game.TeamB + "  Score: " + game.ScoreTeamB + "\n"
                        + "----------------------------" + "\n";
            }

            body += "Remember: You can deactivate the automatic emails trough the app or by going to Windows Task Scheduler and deleting the task!";
        }
        else
        {
            body += $"No games were played on {date}!";

        }

        return (subject, body);
    }

    public void SendEmailService(string subject, string body)
    {
        (string email, string encryptedPassword, bool isAuto) = NBAconfigHelpers.ReadFromNBAconfig();

        string decryptedPassword = Encryptor.DecryptPassword(encryptedPassword);

        string emailDomain = email.Split('@')[1];

        SmtpClient smtpClient = SetEmailClient(emailDomain, email, decryptedPassword);

        if (emailDomain == "gmail.com" || emailDomain == "hotmail.com")
        {
            try
            {
                MailMessage mailMessage = new MailMessage(email, email)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false,
                };

                smtpClient.Send(mailMessage);
            }
            catch
            {
                Console.Clear();

                Console.WriteLine(@"Sorry, it was not possible to establish a connection to the service!
Try setting up email configurations again!
Keep in mind that the service only supports the gmail and hotmail domains!");

                Console.ReadLine();
            }
        }
    }

    internal static SmtpClient SetEmailClient(string emailDomain, string emailSender, string emailSenderPassword)
    {
        SmtpClient smtpClient = new SmtpClient();

        if (emailDomain == "gmail.com")
        {
            smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(emailSender, emailSenderPassword),
                EnableSsl = true,
            };
        }
        else if (emailDomain == "hotmail.com")
        {
            smtpClient = new SmtpClient("smtp.office365.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(emailSender, emailSenderPassword),
                EnableSsl = true,
            };
        }
        else
        {
            Console.WriteLine("Sorry! Email domain not supported!");
            Console.ReadLine();
        }

        return smtpClient;
    }
}
