namespace SportsResults.Call911plz;

using System.Threading.Tasks;
using Spectre.Console;

class Program
{
    // Change these on use
    private readonly static string EMAIL = "";
    private readonly static string SMTPGMAILPASSWORD = "";
    static async Task Main(string[] args)
    {
        // Sending email
        string content = BasketBallInfoForTheWeek(DateTime.Now - new TimeSpan(7, 0, 0, 0), DateTime.Now - new TimeSpan(24, 0, 0));
        EmailSender sender = new(EMAIL, SMTPGMAILPASSWORD);
        sender.SendToSelf("", 
            $"<p style=\"font-family:fixed width\">{content}</p>"
        );

        // Do again in 7 days
        await Task.Delay(TimeSpan.FromDays(7));
    }

    static string BasketBallInfoForTheWeek(DateTime start, DateTime end)
    {
        // Scrapes info over the past week
        DateTime dayForInfo = start;
        List<List<GameSummary>?> gamesOverWeek = [];

        while (dayForInfo < end)
        {
            gamesOverWeek.Add(Scraper.GetSummaries(dayForInfo));
            dayForInfo += new TimeSpan(24, 0, 0);
        }

        // Formats info
        var result = "";
        foreach(var gamesForDay in gamesOverWeek)
        {
            result += FormatGame(gamesForDay);
            result += "<br>";
        }
        return result;
    }

    // Formatting for gmail css (NOT WORTH IT)
    static string FormatGame(List<GameSummary>? games)
    {
        if (games == null)
        {
            return "No games played this day";
        }

        string table = string.Format(
            "{0,-40} {1,5} {2,5} {3,5} {4,5} {5,5} {6,6}<br>",
            "Team", "Q1", "Q2", "Q3", "Q4", "OT", "Total"
        );
        

        foreach (var game in games)
        {
            game.Winner.Name = $"<b style=\"color:green\">{game.Winner.Name}</b>";
            game.Loser.Name = $"<b style=\"color:red\">{game.Loser.Name}</b>";
            table += FormatTeam(game.Winner);
            table += FormatTeam(game.Loser);
            table += "<br>";
        }
        return table;
    }

    static string FormatTeam(Team team)
    {
        var table = string.Format("{0,-20} ", $"{team.Name}");
        for (int i = 0; i < team.Scores.Count; i++)
        {
            table += string.Format("{0,5} ",$"{team.Scores[i]}");
        }
        if (team.Scores.Count < 5)
            table += string.Format("{0,6}", "");
        table += string.Format("{0,6}<br>", $"{team.Scores.Sum()}");

        return table;
    }
}
