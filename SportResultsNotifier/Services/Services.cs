using Spectre.Console;
using SportResultsNotifier.Controllers;
using SportResultsNotifier.Models;
using System.Configuration;
using static SportResultsNotifier.Enums;

namespace SportResultsNotifier.Services;

internal class UserServices
{
    internal static IUser GetUser(IUser? user,Configuration config)
    {
        if (config.AppSettings.Settings["EmailAddress"].Value == "") user = SetUser(user, config);
        else
        {
            user = config.AppSettings.Settings["Type"].Value == "Simple" ?
                new SimpleUser(config.AppSettings.Settings["EmailAddress"].Value, config.AppSettings.Settings["FirstName"].Value,
                config.AppSettings.Settings["LastName"].Value, config.AppSettings.Settings["Type"].Value) :
                new GmailUser(config.AppSettings.Settings["EmailAddress"].Value, config.AppSettings.Settings["FirstName"].Value,
                config.AppSettings.Settings["LastName"].Value, config.AppSettings.Settings["Type"].Value, config.AppSettings.Settings["AppPassword"].Value);
        }
        return user;
    }

    internal static IUser SetUser(IUser? user,Configuration config)
    {
        bool option = AnsiConsole.Prompt(new SelectionPrompt<bool>().Title("What kind of user do you want to choose?").AddChoices(true, false)
            .UseConverter(x => x == true ? "Simple User" : "Gmail User"));

        AnsiConsole.MarkupLine("To simplify the tests, the sender and receiver will be the same person");
        string emailAddress = UserInputs.GetEmailAddress();
        string firstName = UserInputs.GetName("Enter a First Name:");
        string lastName = UserInputs.GetName("Enter a Last Name:");
        if (option) user = new SimpleUser(emailAddress,firstName,lastName,"Simple");
        else
        {
            string appPassword = UserInputs.GetAppPassword();
            user = new GmailUser(emailAddress,firstName,lastName,"Gmail",appPassword);
        }
        UserController.SetUser(user,config);
        return user;
    }
}

internal class ResultServices
{
    internal static async Task SendResults(IUser user,ResultMenuOption option)
    {
        Results results = ResultsController.GetResults();
        if (results.Body == null) AnsiConsole.MarkupLine("[red]No[/] results found.");
        else switch (option)
                {
                    case ResultMenuOption.LocalFolder:
                        await MailController.SendLocalFolderAsync(results, user);
                        break;
                    case ResultMenuOption.PaperCutServer:
                        await MailController.SendPapercutAsync(results, user);
                        break;
                    case ResultMenuOption.GmailSmtp:
                        await MailController.SendGmailSmtpAsync(results, user);
                        break;
                    case ResultMenuOption.AllPossibleMethods:
                        await MailController.SendWithAllMethodsAsync(results, user);
                        break;
                    default: break;
                }
    }
}