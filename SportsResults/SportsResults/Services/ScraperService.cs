using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using SportsResults.Models;

namespace SportsResults.Services;
internal class ScraperService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    public ScraperService(IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = new HttpClient();
    }

    public async Task<List<SportsResult>> Run ()
    {
        try
        {
            var response = await _httpClient.GetAsync(_configuration["ScrapingUrl"]);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);

            var resultsParentDiv = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'game_summaries')]");
            var resultsDivs = resultsParentDiv[0].SelectNodes(".//div[contains(@class, 'game_summary')]");

            string domain = new Uri(_configuration["ScrapingUrl"]).Host;

            List<SportsResult> results = new List<SportsResult>();

            foreach (var item in resultsDivs)
            {
                var mainTable = item.SelectNodes(".//table[contains(@class, 'teams')]")[0];

                var team1 = mainTable.SelectNodes(".//tr")[0];
                var team2 = mainTable.SelectNodes(".//tr")[1];

                var links = item.SelectSingleNode(".//p[contains(@class, 'links')]");
                string LinkToBoxScore = $"{domain}{links.SelectNodes(".//a")[0].Attributes["href"].Value}";
                string LinkToPlayByPlay = $"{domain}{links.SelectNodes(".//a")[1].Attributes["href"].Value}";
                string LinkToShotChart = $"{domain}{links.SelectNodes(".//a")[2].Attributes["href"].Value}";

                int scoreTeam1 = Int32.Parse(team1.SelectNodes(".//td[contains(@class, 'right')]")[0].InnerText);
                int scoreTeam2 = Int32.Parse(team2.SelectNodes(".//td[contains(@class, 'right')]")[0].InnerText);

                string result = scoreTeam1 > scoreTeam2 ? "Team 1 Won" : scoreTeam2 > scoreTeam1 ? "Team 2 Won" : "Draw";

                SportsResult matchResult = new();
                matchResult.Team1 = team1.SelectNodes(".//td")[0].InnerText;
                matchResult.Team2 = team2.SelectNodes(".//td")[0].InnerText;
                matchResult.ScoreTeam1 = scoreTeam1;
                matchResult.ScoreTeam2 = scoreTeam2;
                matchResult.Result = result;
                matchResult.LinkToBoxScore = LinkToBoxScore;
                matchResult.LinkToPlayByPlay = LinkToPlayByPlay;
                matchResult.LinkToShotChart = LinkToShotChart;

                results.Add(matchResult);
            }

            return results;
        }
        catch (Exception e) {
            throw new Exception(e.Message);
        }
    }
}
