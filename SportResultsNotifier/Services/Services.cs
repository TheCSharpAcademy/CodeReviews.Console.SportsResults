using Microsoft.Extensions.Hosting;
using Spectre.Console;
using SportResultsNotifier.Controllers;
using SportResultsNotifier.Models;

namespace SportResultsNotifier;

class Services : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken = default)
    {
        // real delay
        // TimeSpan delay = TimeSpan.FromDays(1);

        // tesing delay
        TimeSpan delay = TimeSpan.FromSeconds(10);

        await Wait(async () =>
        {
            Results results = ResultsController.GetResults();
            IUser user = Settings.Default.Type == "Simple" ?
            new SimpleUser() { Email = Settings.Default.EmailAddress, FirstName = Settings.Default.FirstName, LastName = Settings.Default.LastName, Type = Settings.Default.Type, AppPassword = Settings.Default.AppPassword, }
            : new GmailUser() { Email = Settings.Default.EmailAddress, FirstName = Settings.Default.FirstName, LastName = Settings.Default.LastName, Type = Settings.Default.Type, AppPassword = Settings.Default.AppPassword, };
            if (results.Body != null) await MailController.SendWithAllMethodsAsync(results, user);
        }, delay);
    }

    private async Task Wait(Action action, TimeSpan delay)
    {
        // if statement can be deleted to let the app running indefinitely
        if (ResultsController.AppIteration >= 11)
        {
            AnsiConsole.MarkupLine("\n\r[blue]All Mails[/] sent");
            Environment.Exit(0);
            return;
        }

        await Task.Delay(delay);

        action();

        Wait(action, delay);
    }
}