namespace SportsResults.Call911plz;
using Spectre.Console;

class Program
{
    // Change these on use
    private readonly static string EMAIL = "htruong6219@gmail.com";
    private readonly static string SMTPGMAILPASSWORD = "";
    static void Main(string[] args)
    {
        BasketBallInfoForTheWeek(DateTime.Now - new TimeSpan(7, 0, 0, 0), DateTime.Now);
    }

    static string BasketBallInfoForTheWeek(DateTime start, DateTime end)
    {
        DateTime dayForInfo = start;
        List<List<GameSummary>?> gamesOverWeek = [];
        while (dayForInfo < end)
        {
            gamesOverWeek.Add(Scraper.GetSummaries(dayForInfo));
            dayForInfo += new TimeSpan(24, 0, 0);
        }

        foreach(var game in gamesOverWeek)
        {
            PrintGame(game);
        }

        return default;
    }

    static void PrintGame(List<GameSummary>? games)
    {
        if (games == null)
        {
            AnsiConsole.WriteLine("No games played this day");
            return;
        }

        var table = new Table
        {
            Title = new TableTitle(games[0].Date.ToString())
        };
        table.AddColumns(["Team", "Q1", "Q2", "Q3", "Q4", "OT", "Total"]);
        table.ShowRowSeparators();

        foreach (var game in games)
        {
            if (game.Winner.Scores.Count > 4)
                table.AddRow([
                    $"[bold yellow]{game.Winner.Name}[/]\n[red]{game.Loser.Name}[/]",
                    $"{game.Winner.Scores[0]}\n{game.Loser.Scores[0]}",
                    $"{game.Winner.Scores[1]}\n{game.Loser.Scores[1]}",
                    $"{game.Winner.Scores[2]}\n{game.Loser.Scores[2]}",
                    $"{game.Winner.Scores[3]}\n{game.Loser.Scores[3]}",
                    $"{game.Winner.Scores[4]}\n{game.Loser.Scores[4]}",
                    $"{game.Winner.Scores.Sum()}\n{game.Loser.Scores.Sum()}",
                ]);
            else
                table.AddRow([
                    $"[bold yellow]{game.Winner.Name}[/]\n[red]{game.Loser.Name}[/]",
                    $"{game.Winner.Scores[0]}\n{game.Loser.Scores[0]}",
                    $"{game.Winner.Scores[1]}\n{game.Loser.Scores[1]}",
                    $"{game.Winner.Scores[2]}\n{game.Loser.Scores[2]}",
                    $"{game.Winner.Scores[3]}\n{game.Loser.Scores[3]}",
                    $"",
                    $"{game.Winner.Scores.Sum()}\n{game.Loser.Scores.Sum()}",
                ]);
        }

        AnsiConsole.Write(table);
    }
}
