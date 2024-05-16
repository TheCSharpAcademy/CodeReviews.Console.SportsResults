using SportsResults.Models;
using System.Net.Mail;

namespace SportsResults
{
    internal class Mail
    {
        internal static MailMessage BuildMailMessage(List<Game> games, string fromEmail)
        {
            MailMessage notification = new MailMessage();
            notification.From = new MailAddress(fromEmail);
            notification.IsBodyHtml = true;

            var dateNow = DateOnly.FromDateTime(DateTime.Now);
            string subjectString = $@"BasketBall notification {dateNow}";
            notification.Subject = subjectString;

            string body = string.Empty;

            if (games.Count == 0)
            {
                body = $@"<html><body> No Games Today</html></body>";
            }

            else
            {
                body = "<html><body>";

                int i = 1;

                foreach (var game in games)
                {
                    body = body + $@"<p>Game = {i}</p><p>{game.Winner} vs {game.Loser}</p> <p>Winner: {game.Winner}</p><p>Score:{game.WinnerScore} to {game.LoserScore}</p>";
                    i++;
                }
                body = body + "</html></body>";
            }
            notification.Body = body;

            return notification;
        }


        internal static int ParsePort(string portString)
        {
            int parsedPortString;
            int port;

            if (int.TryParse(portString, out parsedPortString))
            {
                port = parsedPortString;
            }
            else
            {
                port = 4200;
            }
            return port;
        }


        internal static void SendNotifications(List<string> emails, SmtpClient smtpClient, MailMessage notification)
        {
            foreach (string email in emails)
            {
                MailAddress toAddress = new MailAddress(email);
                notification.To.Add(toAddress);

                try
                {
                    smtpClient.Send(notification);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to send notification to: " + email + ". Error: " + ex.Message);

                }

                notification.To.Clear();
            }
        }

    }
}
