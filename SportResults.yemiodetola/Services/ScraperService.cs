using HtmlAgilityPack;
using SportResults.Models;

namespace SportResults.Services;

public class ScraperService
{
  private readonly string _scrapeUrl = String.Empty;
  public ScraperService(string scrapeUrl)
  {
    _scrapeUrl = scrapeUrl;
  }

  public List<Result> ScrapGameData()
  {
    var web = new HtmlWeb();
    var document = web.Load(_scrapeUrl);

    var gameData = document.DocumentNode.SelectNodes("//div[contains(@class, 'game_summary expanded nohover ')]");
    Console.WriteLine(gameData);

    var results = new List<Result>();

    if (gameData == null)
    {
      return results;
    }


    foreach (var data in gameData)
    {
      string WinnerScoreString = data.SelectSingleNode(".//tr[@class='winner']/td[2]")?.InnerText ?? "0";
      string LoserScoreString = data.SelectSingleNode(".//tr[@class='loser']/td[2]")?.InnerText ?? "0";
      var result = new Result()
      {
        WinnerScore = int.TryParse(WinnerScoreString, out int winnerScore) ? winnerScore : 0,
        LoserScore = int.TryParse(LoserScoreString, out int loserScore) ? loserScore : 0,
        Winner = data.SelectSingleNode(".//tr[@class='winner']/td[1]")?.InnerText ?? "Team A",
        Loser = data.SelectSingleNode(".//tr[@class='loser']/td[1]")?.InnerText ?? "Team B"
      };

      results.Add(result);
    }

    return results;
  }
}