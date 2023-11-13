using HtmlAgilityPack;
using SportsResultsNotifier.Models;

namespace SportsResultsNotifier.Services;
public class ScrapeSite
{

    public readonly string URL = "https://www.basketball-reference.com/boxscores/";

    HtmlWeb web = new HtmlWeb();

    public void GetData()
    {
        var htmlDoc = web.Load(URL);

        var table = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[3]/div/table[1]");

        var team1 = table.SelectSingleNode("//*[@id=\"content\"]/div[3]/div/table[1]/tbody/tr[1]/td[1]/a").InnerText;
        int team1Score = Int32.Parse(table.SelectSingleNode("//*[@id=\"content\"]/div[3]/div/table[1]/tbody/tr[1]/td[2]").InnerText);

        var team2 = table.SelectSingleNode("//*[@id=\"content\"]/div[3]/div/table[1]/tbody/tr[2]/td[1]/a").InnerText;
        int team2Score = Int32.Parse(table.SelectSingleNode("//*[@id=\"content\"]/div[3]/div/table[1]/tbody/tr[2]/td[2]").InnerText);

        Console.WriteLine(team1);
        Console.WriteLine(team1Score);

        Console.WriteLine(team2);
        Console.WriteLine(team2Score);

        var match = new Match
        {
            Team1 = team1,
            Team2 = team2,
            Team1Score = team1Score,
            Team2Score = team2Score

        };

        Console.WriteLine($"{match.Team1} {match.Team1Score}");
        Console.WriteLine($"{match.Team2} {match.Team2Score}");

        string body = $"{match.Team1} {match.Team1Score}<br>{match.Team2} {match.Team2Score}";
        string from = "marius.gravningsmyhr@gmail.com";
        string to = "marius.gravningsmyhr@gmail.com";
        string subject = "Match result";

        MailService mailService = new MailService(from, to, subject, body);
        mailService.SendEmail();
    }

}

