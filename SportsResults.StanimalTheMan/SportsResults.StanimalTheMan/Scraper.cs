using HtmlAgilityPack;

namespace SportsResults.StanimalTheMan;

internal static class Scraper
{
    public static List<Game> ScrapeGames(string url)
    {

        List<Game> games = new List<Game>();

        HtmlWeb web = new HtmlWeb();

        var doc = web.Load(url);

        var parentDiv = doc.DocumentNode.SelectSingleNode("//div[@class='game_summaries']");

        var gameSummaries = parentDiv.SelectNodes(".//div[contains(@class, 'game_summary')]");

        if (gameSummaries != null)
        {
            foreach ( var gameSummary in gameSummaries )
            {
                //Console.WriteLine(gameSummary.InnerHtml);
                var gameSummaryDetails = gameSummary.SelectSingleNode(".//table[@class='teams']");

                var winnerDetails = gameSummaryDetails.SelectSingleNode(".//tr[@class='winner']");
                var loserDetails = gameSummaryDetails.SelectSingleNode(".//tr[@class='loser']");

                var tdNodes = winnerDetails.SelectNodes(".//td");
                var winningTeam = tdNodes[0].InnerText.Trim();
                var winningTeamPoints = int.Parse(tdNodes[1].InnerText.Trim());

                tdNodes = loserDetails.SelectNodes(".//td");
                var losingTeam = tdNodes[0].InnerText.Trim();
                var losingTeamPoints = int.Parse(tdNodes[1].InnerText.Trim());
                var game = new Game { WinningTeam = winningTeam, WinningTeamPoints = winningTeamPoints, LosingTeam = losingTeam, LosingTeamPoints = losingTeamPoints }; ;
                games.Add(game);
            }
        }
        return games;
    }
}
