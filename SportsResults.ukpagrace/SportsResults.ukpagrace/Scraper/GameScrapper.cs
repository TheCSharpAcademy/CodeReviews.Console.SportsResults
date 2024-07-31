using HtmlAgilityPack;
using SportsResults.ukpagrace.Interfaces;
using SportsResults.ukpagrace.Model;

namespace SportsResults.ukpagrace.Scraper
{
    public class GameScraper : IGameScraper
    {
        public List<Game> ScrapGames(string url) 
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument = htmlWeb.Load(url);
            var gameScores = htmlDocument.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[@class='game_summaries']/div/table[@class='teams']/tbody");
            var games = new List<Game>();

            if(gameScores == null)
            {
                return games;
            }
            foreach (var gameScore in gameScores)
            {
                var game = new Game()
                {
                    Winner = gameScore.SelectSingleNode("tr[@class='winner']/td[1]").InnerText,
                    WinnerScore = Convert.ToInt32(gameScore.SelectSingleNode("tr[@class='winner']/td[@class='right']").InnerText),
                    Loser = gameScore.SelectSingleNode("tr[@class='loser']/td[1]").InnerText,
                    LoserScore = Convert.ToInt32(gameScore.SelectSingleNode("tr[@class='loser']/td[@class='right']").InnerText)
                };

                games.Add(game);
            };
            return games;
        }
    }
}




