using HtmlAgilityPack;

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
}
