using System.Text;
using HtmlAgilityPack;

namespace SportsResultsNotifier;

public class BasketballScraper(EmailClient emailClient, string url)
{
    // The update check method that runs periodically and sends out the update when it's due.
    public void CheckForScoreUpdate(object? stateInfo)
    {
        Console.Clear();
        Console.WriteLine("Press enter to stop.\n");
        
        var state = (TimerState)stateInfo!;
        Console.WriteLine($"Last emailed... {state.LastFired:g}");
        
        // If an update was already sent today, go back to sleep.
        if (DateTime.Now.Day == state.LastFired.Day) return;
        
        state.LastFired = DateTime.Now;
        Console.WriteLine($"Sending out update ({state.LastFired:g})...");
        SendUpdate();
        Console.WriteLine("Update sent.");
    }
    
    private void SendUpdate()
    {
        var msgSubject = $"Hoop Update for {DateTime.Now.Date:M}";
        var msgBody = BuildEmailBody();
        emailClient.SendEmail(
            subject: msgSubject,
            body: msgBody);
    }

    private string BuildEmailBody()
    {
        var games = ScrapeGameNodes();
        var htmlBody = BuildHtmlBody(games);
        return htmlBody;
    }

    private IEnumerable<HtmlNode> ScrapeGameNodes()
    {
        var web = new HtmlWeb();
        var doc = web.Load(url);
        var games = doc.DocumentNode
            .Descendants("table")
            .Where(y => y.GetAttributeValue("class", "").Contains("teams"));
        return games;
    }

    private string BuildHtmlBody(IEnumerable<HtmlNode> games)
    {
        var str = new StringBuilder("");
        foreach (var gameNode in games)
        {
            var game = new BasketballGame(gameNode);
            str.AppendLine(game.ScoreSummary());
            str.AppendLine();
        }
        return str.ToString();
    }
}