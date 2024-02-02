using HtmlAgilityPack;
using SportsResults.StevieTV.Models;

namespace SportsResults.StevieTV.Scraper;

public class GameScraper : IGameScraper
{
    public List<Game> GetGames(string url)
    {
        var games = new List<Game>();

        var webpage = new HtmlWeb().Load(url);

        var nodes = webpage.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[@class='game_summaries']/div/table[@class='teams']/tbody");

        foreach (var node in nodes)
        {
            var game = new Game()
            {
                Winner = node.SelectSingleNode("tr[@class='winner']/td[1]").InnerText,
                WinnerScore = Int32.Parse(node.SelectSingleNode("tr[@class='winner']/td[2]").InnerText),
                Loser = node.SelectSingleNode("tr[@class='loser']/td[1]").InnerText,
                LoserScore = Int32.Parse(node.SelectSingleNode("tr[@class='loser']/td[2]").InnerText)
            };
            
            games.Add(game);
        }
        
        return games;
    }
}