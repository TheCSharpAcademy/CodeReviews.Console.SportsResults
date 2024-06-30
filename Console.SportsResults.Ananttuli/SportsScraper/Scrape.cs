using HtmlAgilityPack;
using SportsScraper.Models;

namespace SportsScraper;


public class Scraper
{
    public static List<GameSummaryDto> Scrape()
    {
        var url = ConfigManager.Config.ScrapeUrl;
        var web = new HtmlWeb();
        var doc = web.Load(url);

        return GetGameSummaries(doc);
    }

    private static List<GameSummaryDto> GetGameSummaries(HtmlDocument documentNode)
    {
        var gameSummaryPath = "//*[@id=\"content\"]/div[3]/div";

        var summaryNodes = documentNode.DocumentNode
            .SelectNodes(gameSummaryPath).ToList();

        return summaryNodes
            .Select(ExtractGameSummary)
            .Where(n => n != null)
            .ToList();
    }

    private static GameSummaryDto ExtractGameSummary(HtmlNode? node)
    {
        var defaultTeam = new Team(["", "", "", ""]);

        if (node == null) return new GameSummaryDto(defaultTeam, defaultTeam);

        var totalScoresPath = ".//table[1]/tbody";

        var teamOneName = node.SelectSingleNode($"{totalScoresPath}/tr[1]/td[1]").InnerText;
        var teamOneScore = node.SelectSingleNode($"{totalScoresPath}/tr[1]/td[2]").InnerText;

        var teamTwoName = node.SelectSingleNode($"{totalScoresPath}/tr[2]/td[1]").InnerText;
        var teamTwoScore = node.SelectSingleNode($"{totalScoresPath}/tr[2]/td[2]").InnerText;

        var quarterBasePath = ".//table[2]/tbody";

        static List<string> extractQuarterScores(HtmlNodeCollection nodes) => nodes.ToList().Where((_, i) => i > 0 && i < 5)
            .Select(n => n.InnerText)
            .ToList();

        var teamOneQuarterScores = extractQuarterScores(node.SelectNodes($"{quarterBasePath}/tr[1]/td"));
        var teamTwoQuarterScores = extractQuarterScores(node.SelectNodes($"{quarterBasePath}/tr[2]/td"));

        return new GameSummaryDto(
            new Team(teamOneQuarterScores, teamOneName, teamOneScore),
            new Team(teamTwoQuarterScores, teamTwoName, teamTwoScore)
        );
    }
}