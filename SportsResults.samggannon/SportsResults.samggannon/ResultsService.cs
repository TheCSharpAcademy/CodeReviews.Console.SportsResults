using Microsoft.Extensions.Hosting;
using SportsResults.samggannon.Models;
using System.Configuration;
using System.Text;

namespace SportsResults.samggannon;

internal class ResultsService : BackgroundService
{
    private readonly EmailSettings _EmailSettings;
    private Timer? _timer;

    public ResultsService()
    {
        var emailSettings = new EmailSettings
        {
            SmtpAddress = ConfigurationManager.AppSettings["SmtpAddress"],
            SmtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]),
            FromAddress = ConfigurationManager.AppSettings["FromAddress"],
            SmtpPassword = ConfigurationManager.AppSettings["SmtpPassword"],
            ToAddress = ConfigurationManager.AppSettings["ToAddress"]
        };

        _EmailSettings = emailSettings;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(Run, null, TimeSpan.Zero, TimeSpan.FromMinutes(24)); // Adjust the interval as needed
        return Task.CompletedTask;
    }

    public void Run(object state)
    {
        List<BoxScore> scores = Scraper.ScrapeScores();
        Stock stock = Scraper.GetStockPrice();

        string gameInfo = BuildGameInfo(scores);
        string stockInfo = BuildStockInfo(stock);
        string emailBody = gameInfo + stockInfo;

        EmailResults(emailBody);
        Console.WriteLine("email sent");
    }

    private string BuildStockInfo(Stock stock)
    {
        StringBuilder body = new();
        body.Append("=========================");
        body.Append("</br>");
        body.Append($"<h1>Stock: {stock.Ticker}</h1>");
        body.Append($"{stock.Ticker} : {stock.Price} ");

        return body.ToString();
        
    }

    private string BuildGameInfo(List<BoxScore> scores)
    {
        StringBuilder body = new();
        body.Append("<h1>Game Scores</h1>");
        body.Append("<ul>");

        foreach (var score in scores)
        {
            body.Append("<li>");
            body.Append($"{score.AwayTeam} : {score.AwayScore} " + "</br>" +
                $"{score.HomeTeam}  :  {score.HomeScore} ");
            body.Append("</li>");
        }

        body.Append("</ul>");

        return body.ToString();
    }

    private void EmailResults(string emailBody)
    {
        EmailClient emailClient = new(_EmailSettings, "Game Scores", emailBody, true);
        emailClient.SendEmail();
    }

    public override void Dispose()
    {
        _timer?.Dispose();
        base.Dispose();
    }
}
