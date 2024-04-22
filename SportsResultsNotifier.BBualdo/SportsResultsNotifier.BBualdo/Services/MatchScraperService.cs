using HtmlAgilityPack;
using SportsResultsNotifier.BBualdo.Models;

namespace SportsResultsNotifier.BBualdo.Services;

internal class MatchScraperService : IMatchScraperService
{
  public List<Match> GetMatches(string url)
  {
    List<Match> matches = [];

    HtmlWeb web = new();
    HtmlDocument document = web.Load(url);

    HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[@class='game_summaries']/div/table[@class='teams']/tbody");

    foreach (HtmlNode node in nodes)
    {
      Team winner = new(
        node.SelectSingleNode("tr[@class='winner']/td[1]").InnerText,
        node.SelectSingleNode("tr[@class='winner']/td[2]").InnerText);

      Team loser = new(
        node.SelectSingleNode("tr[@class='loser']/td[1]").InnerText,
        node.SelectSingleNode("tr[@class='loser']/td[2]").InnerText);

      Match match = new(winner, loser);
      matches.Add(match);
    }

    return matches;
  }
}