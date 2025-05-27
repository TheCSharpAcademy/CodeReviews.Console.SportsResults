using Spectre.Console;
using SportResultsNotifier.Models;
using SportResultsNotifier.Services;
using System.Configuration;
using static SportResultsNotifier.Enums;

namespace SportResultsNotifier;

class Menu
{
    private static Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
    private static IUser? User { get; set; }
    internal static void MainMenu()
    {
        User = UserServices.GetUser(User, config);
        bool run = true;
        while (run)
        {
            AnsiConsole.Clear();
            MainMenuOption option = AnsiConsole.Prompt(new SelectionPrompt<MainMenuOption>().Title("Welcome To Sport Results Notifier by P13.")
                .AddChoices(MainMenuOption.SetUser, MainMenuOption.SendResults, MainMenuOption.Exit));
            switch (option)
            {
                case MainMenuOption.SetUser:
                    UserMenu();
                    break;
                case MainMenuOption.SendResults:
                    ResultMenu();
                    break;
                default:
                    run = false; break;
            }
            AnsiConsole.MarkupLine("Press [yellow]any[/] key to continue.");
            Console.ReadKey();
        }
        AnsiConsole.MarkupLine("[deeppink3_1]Thank you[/] for using Sport results Notifier.\n\nP13");
    }

    private static void UserMenu()
    {
        UserMenuOption option = AnsiConsole.Prompt(new SelectionPrompt<UserMenuOption>().Title("Choose an option below:")
            .AddChoices(UserMenuOption.SetUser, UserMenuOption.Return));
        switch (option)
        {
            case UserMenuOption.SetUser:
                UserServices.SetUser(User, config);
                break;
            default:
                break;
        }
    }

    private static void ResultMenu()
    {
        SelectionPrompt<ResultMenuOption> optionPrompt = new();
        optionPrompt.Title("Choose an option below:").AddChoices(ResultMenuOption.LocalFolder, ResultMenuOption.PaperCutServer);
        if (User.Type == "Gmail") optionPrompt.AddChoice(ResultMenuOption.GmailSmtp);
        optionPrompt.AddChoices(ResultMenuOption.AllPossibleMethods, ResultMenuOption.Return);
        ResultMenuOption option = AnsiConsole.Prompt(optionPrompt);
        Task.WaitAny(ResultServices.SendResults(User, option));
    }
}
