using MimeKit;
using SportsResults;
using SportsResults.Models;
using System.Text;

internal class Program
{
    static MailService mailService = new();
    static readonly string toName = "Jakub";
    static readonly string toEmail = "test.mailkit.jakub@gmail.com";
    static readonly string subject = "Test subject";

    private static void Main(string[] args)
    {     
        (List<Game> games, List<List<BoxScore>> boxScores) = WebScraper.GetGames("https://www.basketball-reference.com/boxscores/");

        if (games.Count == 0)
        {
            MimeMessage noGames = mailService.SetMessageProperties(toName, toEmail, subject, "No games were played today.");
            mailService.SendMailMessage(noGames);
            return;
        }

        StringBuilder sb = new();

        for (int i = 0; i < games.Count; i++)
        {
            sb.AppendLine($"{games.ElementAt(i).Winner} [{games.ElementAt(i).WinnerScore}] WINS vs {games.ElementAt(i).Loser} [{games.ElementAt(i).LoserScore}]\n");

            foreach (var score in boxScores.ElementAt(i))
            {
                sb.AppendLine($"{score.Starters}, {score.PTS}, {score.TRB} ,{score.AST}, {score.BLK}, {score.STL}");
            }
            sb.AppendLine("\n");
            sb.AppendLine("============================");
        }
        MimeMessage mailMessage = mailService.SetMessageProperties(toName, toEmail, subject, sb.ToString());
        mailService.SendMailMessage(mailMessage);
    }
}