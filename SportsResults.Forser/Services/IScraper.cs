using HtmlAgilityPack;
using SportsResults.Forser.Models;

namespace SportsResults.Forser.Services
{
    internal interface IScraper
    {
        HtmlDocument LoadWebsite(string url);
        string GetDateOfResults(HtmlDocument htmlDoc);
        List<HtmlNode> GetAllNodes(HtmlDocument htmlDoc);
        GameModel GenerateGameModel(HtmlDocument htmlDoc);
        string GenerateEmail(GameModel gameModel);
    }
}