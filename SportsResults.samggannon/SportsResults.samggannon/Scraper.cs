using HtmlAgilityPack;
using SportsResults.samggannon.Models;

namespace SportsResults.samggannon;

public class Scraper
{
    public static void Scrape()
    {
        HtmlWeb web = new();
        HtmlDocument document = web.Load("https://example.com");

        var title = document.DocumentNode.SelectNodes("//div/h1").First().InnerHtml;
        var description = document.DocumentNode.SelectNodes("//div/p").First().InnerHtml;

        Console.WriteLine(title);
        Console.WriteLine(description);
        Console.ReadLine();
    }

    public static List<BoxScore> ScrapeScores()
    {
        List<BoxScore> scores = new();

        HtmlWeb web = new();
        HtmlDocument document = web.Load("https://www.basketball-reference.com/boxscores/");

        var gameSummaries = document.DocumentNode.SelectNodes("//div[@class='game_summary expanded nohover ']").ToList();

        foreach (var node in gameSummaries)
        {
            scores.Add(
                new BoxScore
                {
                    AwayTeam = node.SelectSingleNode(".//table/tbody/tr[1]/td[1]").InnerText,
                    HomeTeam = node.SelectSingleNode(".//table/tbody/tr[2]/td[1]").InnerText,
                    AwayScore = node.SelectSingleNode(".//table/tbody/tr[1]/td[2]").InnerText,
                    HomeScore = node.SelectSingleNode(".//table/tbody/tr[2]/td[2]").InnerText,
                });
        }

        return scores;
    }

    public static Stock GetStockPrice()
    {
        Stock stock = new();

        HtmlWeb web = new();
        HtmlDocument document = web.Load("https://finance.yahoo.com/quote/AAPL/");

        stock.Ticker = document.DocumentNode.SelectNodes("//*[@id=\"nimbus-app\"]/section/section/section/article/section[1]/div[1]/div/section/h1").FirstOrDefault().InnerText;
        stock.Price = document.DocumentNode.SelectSingleNode("//*[@id=\"nimbus-app\"]/section/section/section/article/section[1]/div[2]/div[1]/section/div/section[1]/div[1]/fin-streamer[1]/span").InnerText;

        return stock;
    }
}