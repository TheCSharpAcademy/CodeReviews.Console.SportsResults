using HtmlAgilityPack;
using SportsResults.Forser.Models;

namespace SportsResults.Forser.Services
{
    internal class Scraper : IScraper
    {
        public Scraper() { }
        public HtmlDocument LoadWebsite(string url)
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            return htmlWeb.Load(url);
        }

        public string GetDateOfResults(HtmlDocument htmlDoc)
        {
            throw new NotImplementedException();
        }

        public List<HtmlNode> GetAllNodes(HtmlDocument htmlDoc)
        {
            throw new NotImplementedException();
        }

        public GameModel GenerateGameModel(HtmlDocument htmlDoc)
        {
            throw new NotImplementedException();
        }
        public string GenerateEmail(GameModel gameModel)
        {
            throw new NotImplementedException();
        }
    }
}