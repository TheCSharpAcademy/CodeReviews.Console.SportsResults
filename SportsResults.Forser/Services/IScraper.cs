using HtmlAgilityPack;
using SportsResults.Forser.Models;

namespace SportsResults.Forser.Services
{
    internal interface IScraper
    {
        HtmlDocument LoadWebsite(string url);
        string GetDateOfResults(HtmlDocument htmlDoc);
        List<HtmlNode> GetAllNodes(HtmlDocument htmlDoc);
        List<GameModel> GenerateGameModel(List<HtmlNode> htmlNodes);
        string GenerateEmail(List<GameModel> gameModels, string title);
    }
}