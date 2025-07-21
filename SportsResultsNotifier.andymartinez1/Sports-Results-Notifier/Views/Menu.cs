using Spectre.Console;
using Sports_Results_Notifier.Controllers;

namespace Sports_Results_Notifier.Views;

public class Menu
{
    private readonly ScraperController _controller;

    public Menu(ScraperController controller)
    {
        _controller = controller;
    }

    public void MainMenu()
    {
        AnsiConsole.Write(new FigletText("Sports Results"));

        var game = _controller.GetGameInfo();

        _controller.SendEmail(game);
    }
}
