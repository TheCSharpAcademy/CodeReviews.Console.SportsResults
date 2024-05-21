using HtmlAgilityPack;
using SportsResults.samggannon.Models;

namespace SportsResults.samggannon;

public class Scraper
{
    public static void Scrape()
    {
        HtmlWeb web = new HtmlWeb();
        HtmlDocument document = web.Load("https://example.com");

        var title = document.DocumentNode.SelectNodes("//div/h1").First().InnerHtml;
        var description = document.DocumentNode.SelectNodes("//div/p").First().InnerHtml;

        Console.WriteLine(title);
        Console.WriteLine(description);
        Console.ReadLine();
    }

    public static List<BoxScore> ScrapeScores()
    {
        List<BoxScore> scores = new List<BoxScore>();

        HtmlWeb web = new HtmlWeb();
        HtmlDocument document = web.Load("https://www.basketball-reference.com/boxscores/");

        var gameSummaries = document.DocumentNode.SelectNodes("//div[@class='game_summary expanded nohover ']").ToList();

        foreach (var node in gameSummaries)
        {
            scores.Add(
                new BoxScore
                {
                    Winner = node.SelectSingleNode(".//table/tbody/tr[1]/td[1]").InnerText,
                    Loser = node.SelectSingleNode(".//table/tbody/tr[2]/td[1]").InnerText,
                    WinningScore = Int32.Parse(node.SelectSingleNode(".//table/tbody/tr[1]/td[2]").InnerText),
                    LosingScore = Int32.Parse(node.SelectSingleNode(".//table/tbody/tr[2]/td[2]").InnerText),
                });
        }

        return scores;
    }
}
