using FluentEmail.Core;
using FluentEmail.Razor;
using FluentEmail.Smtp;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Timers;

namespace SportsResults;

internal class Program
{
    private static StringBuilder template;
    private static readonly int DAY = 1000 * 60 * 60 * 24;
    private static IConfigurationRoot configurationRoot;
    static ManualResetEvent quitEvent = new(false);
    static async Task Main()
    {
        configurationRoot = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        SetCancelEvent();

        template = SetupEmail();

        System.Timers.Timer timer = new(DAY);
        timer.Elapsed += OnTimedEvent;
        timer.AutoReset = true;
        timer.Enabled = true;
        quitEvent.WaitOne();
    }

    private static void SetCancelEvent()
    {
        Console.CancelKeyPress += (_, eArgs) =>
        {
            quitEvent.Set();
            eArgs.Cancel = true;
        };
    }

    private static void OnTimedEvent(object? sender, ElapsedEventArgs e)
    {
        Console.WriteLine("Attempting to send email...");
        try
        {
            SendEmail(template);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private static StringBuilder SetupEmail()
    {
        var sender = new SmtpSender(() => new SmtpClient("smtp.gmail.com")
        {
            UseDefaultCredentials = false,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Port = Convert.ToInt32(configurationRoot.GetSection("Gmail")["Port"]),
            Credentials = new NetworkCredential(configurationRoot.GetSection("Gmail")["Sender"], configurationRoot.GetSection("Gmail")["Password"])
        });

        template = new();

        template.AppendLine("Hi @Model.FirstName,");
        template.AppendLine("<p>Here are the results for the NBA games today:</p>");
        GetSportsResults(template);

        Email.DefaultSender = sender;
        Email.DefaultRenderer = new RazorRenderer();
        return template;
    }

    private static async Task SendEmail(StringBuilder template)
    {
        await Email
            .From(configurationRoot.GetSection("Mail")["From"])
            .To(configurationRoot.GetSection("Mail")["ToEmail"], configurationRoot.GetSection("Mail")["ToName"])
            .Subject($"{DateTime.Now.ToLongDateString()} - NBA Results")
            .UsingTemplate(template.ToString(), new { FirstName = "Ed" })
            .SendAsync();
        Console.WriteLine("Email sent.");
    }

    private static void GetSportsResults(StringBuilder template)
    {
        var url = "https://www.basketball-reference.com/boxscores/";
        var web = new HtmlWeb();
        var doc = web.Load(url);
        var gameTables = doc.DocumentNode.SelectNodes("//div[@class='game_summary expanded nohover ']");

        List<Game> games = GameFactory.CreateGamesFromHtmlNodeCollection(gameTables);

        foreach (Game game in games)
        {
            template.AppendLine($"<p>{game.ToString()}</p>");
        }
    }
}