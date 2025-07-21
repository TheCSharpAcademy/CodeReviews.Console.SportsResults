using Spectre.Console;
using Sports_Results_Notifier.Models;

namespace Sports_Results_Notifier.Views;

public class UserInterface
{
    public static void ViewTable(Game game)
    {
        var table = new Table();
        table.AddColumn("Date");
        table.AddColumn("Winning Team");
        table.AddColumn("Winning Team Score");
        table.AddColumn("Losing Team");
        table.AddColumn("Losing Team Score");

        table.AddRow(
            game.Date.ToLongDateString(),
            game.WinningTeam,
            game.WinnerScore.ToString(),
            game.LosingTeam,
            game.LoserScore.ToString()
        );

        table.Expand();
        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("[blue]Press any key to exit[/]");
        Console.ReadKey(true);
    }
}
