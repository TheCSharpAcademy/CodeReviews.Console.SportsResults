using HtmlAgilityPack;
using SportsResults.frockett.Models;

namespace SportsResults.frockett;

internal class ScraperService
{
    internal async Task<List<GameResult>> GetScoresAsync()
    {
        List<GameResult> results = new List<GameResult>();

        HtmlWeb web = new HtmlWeb();
        HtmlDocument document = await web.LoadFromWebAsync("https://www.basketball-reference.com/boxscores/");
        var gameSummaries = document.DocumentNode.SelectNodes("//div[contains(@class, 'game_summary expanded nohover')]");

        foreach ( var summary in gameSummaries )
        {
            var teams = summary.SelectNodes(".//table[@class='teams']/tbody/tr/td[1]/a");
            var scores = summary.SelectNodes(".//table[@class='teams']/tbody/tr/td[@class='right'][1]");

            GameResult result = new GameResult();

            for (int i = 0; i < teams.Count; i++)
            {
                string teamName = teams[i].InnerText;
                string score = scores[i].InnerText;

                // Home team is listed second in the US, so in a given pair "i" should always be 1
                if (i == 1)
                {
                    result.HomeTeam = teamName;
                    result.HomeFinalScore = score;
                }
                else
                {
                    result.AwayTeam = teamName;
                    result.AwayFinalScore = score;
                }
            }
            results.Add(result);
        }
        return results;
    }
}
