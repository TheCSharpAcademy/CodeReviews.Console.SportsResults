using HtmlAgilityPack;

namespace SportsResults.jollejonas.Services;
public class Scraper
{
    public string Scrape()
    {
        var url = @"https://www.basketball-reference.com/boxscores/";
        HtmlWeb web = new HtmlWeb();
        var htmlDoc = web.Load(url);
        var parentDiv = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='game_summaries']");
        string message = "";
        if (parentDiv == null)
        {
            Console.WriteLine("Could not find the 'game_summaries' div.");
            Console.WriteLine("HTML content:");
            Console.WriteLine(htmlDoc.DocumentNode.OuterHtml);
            return "";
        }

        var childDivs = parentDiv.SelectNodes(".//div[contains(@class, 'game_summary')]");

        if (childDivs == null)
        {
            Console.WriteLine("No 'game_summary' divs found.");
            return "";
        }

        foreach (var child in childDivs)
        {
            var homeTeam = child.SelectNodes(".//a")[0].InnerText;
            var awayTeam = child.SelectNodes(".//a")[2].InnerText;
            var homeScore = child.SelectNodes(".//td")[1].InnerText;
            var awayScore = child.SelectNodes(".//td")[4].InnerText;
            message += ($"{homeTeam.PadRight(15)} {homeScore} - {awayScore} {awayTeam.PadLeft(15)} \n");
        }

        return message;
    }
}

