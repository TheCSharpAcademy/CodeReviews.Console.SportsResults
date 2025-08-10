using HtmlAgilityPack;

namespace SportsResultsNotifier.UI.Services;

public class ScraperService
{
    private readonly HttpClient _httpClient;

    public ScraperService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetResultsAsync()
    {
        var url = "https://www.basketball-reference.com/boxscores/";
        var html = await _httpClient.GetStringAsync(url);

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var gameNodes = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'game_summary')]");
        if (gameNodes == null || gameNodes.Count == 0)
        {
            return "No games found.";
        }

        var results = new List<string>();

        foreach (var game in gameNodes)
        {
            var teams = game.SelectNodes(".//tr");
            if (teams == null || teams.Count < 2) continue;

            var team1 = teams[0].SelectSingleNode(".//a").InnerText.Trim();
            var score1 = teams[0].SelectSingleNode(".//td[@class='right']").InnerText.Trim();

            var team2 = teams[1].SelectSingleNode(".//a").InnerText.Trim();
            var score2 = teams[1].SelectSingleNode(".//td[@class='right']").InnerText.Trim();

            results.Add($"{team1} {score1} - {team2} {score2}");
        }

        return string.Join("\n", results);
    }
}
