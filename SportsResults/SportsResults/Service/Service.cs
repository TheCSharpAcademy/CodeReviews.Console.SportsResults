using Microsoft.Extensions.Hosting;
using SportsResults.Model;

namespace SportsResults.Service;

public class Service : BackgroundService
{
    IEmailSender sender {  get; set; }
    Scraper scraper { get; set; }
    public bool mailSentToday = false;

    public Service(IEmailSender _sender, Scraper _scraper) 
    {
        sender = _sender;
        scraper = _scraper;
        mailSentToday = false;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (mailSentToday)
        {
            return;
        }
        List<Result> results = scraper.WebScraper();
        string message = results.Count > 0 ? GenerateMessage(results) : "No games were played yesterday. Sad day for the lovers of sports. Sad day I say.";
        try
        {
            sender.SendEmail(EmailSender.target, EmailSender.subject, message);
            mailSentToday = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private string GenerateMessage(List<Result> results)
    {
        string message = string.Empty;
        foreach (var result in results)
        {
            message += $"\n{result.winner} played with {result.loser}. The end score was {result.winnerScore}:{result.loserScore}. {result.winner} won.";
        }
        return message;
    }
}