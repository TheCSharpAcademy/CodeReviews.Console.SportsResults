using HtmlAgilityPack;
using SportsResults.Model;

namespace SportsResults.Service;

public class Scraper
{
    public List<Result> WebScraper()
    {
        var html = @"https://www.basketball-reference.com/boxscores/";

        HtmlWeb web = new HtmlWeb();

        var htmlDoc = web.Load(html);
        List<Result> results = new List<Result>();
        var gameResults = htmlDoc.DocumentNode.SelectNodes("//div[@class='game_summary expanded nohover ']");

        if(gameResults.Count > 0 )
        {
            foreach (var gameResult in gameResults)
            {
                results.Add(new Result
                {
                    winner = gameResult.SelectSingleNode(".//table/tbody/tr[2]/td[1]").InnerText,
                    winnerScore = gameResult.SelectSingleNode("//table/tbody/tr[2]/td[2]").InnerText,
                    loser = gameResult.SelectSingleNode("//table/tbody/tr[1]/td[1]").InnerText,
                    loserScore = gameResult.SelectSingleNode("//table/tbody/tr[1]/td[2]").InnerText
                });
            }
        }
        else
        {
            return null;
        }
        return results;
    }
}