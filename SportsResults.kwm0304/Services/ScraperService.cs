using HtmlAgilityPack;

namespace SportsResults.kwm0304.Services;

public class ScraperService
{
  public string Scrape()
  {
    string games = "";
    HtmlWeb web = new();
    HtmlDocument document = web.Load("https://www.basketball-reference.com/boxscores/");
    var gameSummariesDiv = document.DocumentNode.SelectSingleNode("//div[@class='game_summaries']");
    if (gameSummariesDiv != null)
    {
      var gameSummaries = gameSummariesDiv.SelectNodes(".//div[@class='game_summary expanded nohover ']");
      if (gameSummaries != null)
      {
        foreach (var gameSummary in gameSummaries)
        {
          var loserRow = gameSummary.SelectSingleNode(".//tr[@class='loser']");
          if (loserRow != null)
          {
            var loserCity = loserRow.SelectSingleNode(".//a").InnerText;
            var loserScore = loserRow.SelectSingleNode(".//td[@class='right']").InnerText;
            string losingLine = loserCity.ToString() + " " + loserScore.ToString();
            games += losingLine + "\n";
          }
          else if (loserRow is null)
          {
            Console.WriteLine("Loser row is null");
          }
          var winnerRow = gameSummary.SelectSingleNode(".//tr[@class='winner']");
          if (winnerRow != null)
          {
            var winnerCity = winnerRow.SelectSingleNode(".//a").InnerText;
            var winnerScore = winnerRow.SelectSingleNode(".//td[@class='right']").InnerText;
            string winningLine = winnerCity.ToString() + winnerScore.ToString();
            games += winningLine + "\n\n";
            Console.WriteLine($"Winner: {winnerCity}, Score: {winnerScore}");
          }
        }
      }
      else
      {
        Console.WriteLine("Game summaries is null");
      }
    }
    else
    {
      Console.WriteLine("Game summaries div is null");
    }
    return games;
  }
}