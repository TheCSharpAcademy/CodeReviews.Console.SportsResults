using SportsResults.Models;
using System.Net.Mail;
using System.Text;

namespace SportsResults.Service;

internal static class Email
{

    public static void Send(List<Game> games)
    {
        var from = "noreply@basketball-reference.com";
        var to = "user@yahoo.com";
        var subject = "Basketball Results";
        var host = "localhost";
        var port = 25;
        string body;

        if (games != null && games.Count > 0)
        {
            StringBuilder bodyBuilder = new StringBuilder();
            foreach (var game in games)
            {
                bodyBuilder.AppendLine($"Matchup: {game.Matchup}");
                bodyBuilder.AppendLine($"Winner: {game.Winner} with score {game.WinnerScore}");
                bodyBuilder.AppendLine($"Loser: {game.Loser} with score {game.LoserScore}");
                bodyBuilder.AppendLine();
            }

            body = bodyBuilder.ToString(); 
        }
        else
        {
            body = "There was a problem getting the sports results from the website.";
        }

        MailMessage mail = new(from, to, subject, body);
        SmtpClient client = new(host, port);
        client.Send(mail);
    }
}
