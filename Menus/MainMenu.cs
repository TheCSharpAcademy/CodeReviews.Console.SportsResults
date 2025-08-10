using Microsoft.Extensions.Configuration;
using Spectre.Console;
using SportsResultsNotifier.UI.Services;
using SportsResultsNotifier.UI.Validator;
using System.ComponentModel.DataAnnotations;

public static class MainMenu
{
    private static ScraperService _scraper;
    private static EmailService _emailService;
    private static string _to;

    public static async Task Show()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        string host = config["Smtp:Host"];
        int port = int.Parse(config["Smtp:Port"]);
        bool enableSsl = bool.Parse(config["Smtp:EnableSsl"]);
        string user = config["Smtp:User"];
        string pass = config["Smtp:Password"];
        string from = config["Smtp:From"];

        using var httpClient = new HttpClient();
        _scraper = new ScraperService(httpClient);
        _emailService = new EmailService(host, port, user, pass, from, enableSsl);

        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[blue]Sports Results Notifier[/]")
                .AddChoices(new[]
                {
                    "Send Email",
                    "Read Data",
                    "Exit"
                }));

            switch (option)
            {
                case "Send Email":
                    await SendEmail();
                    break;

                case "Read Data":
                    await ReadData();
                    break;

                case "Exit":
                    exit = true;
                    break;
            }
        }
    }

    private static async Task ReadData()
    {
        var results = await _scraper.GetResultsAsync();
        AnsiConsole.MarkupLine($"[green]{results}[/]");
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private static async Task SendEmail()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("Write the email you want to send to: ");
        var to = Console.ReadLine();

        if (SportsResultsNotifier.UI.Validator.Validator.IsValidEmail(to))
        {
            _to = to;
            var results = await _scraper.GetResultsAsync();
            await _emailService.SendEmailAsync(_to, "Sports Results Notifier", results);
            AnsiConsole.MarkupLine("\n[yellow]Email sent successfully![/]");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        else
        {
            AnsiConsole.MarkupLine("\n[red]Email could not be sent[/]");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
