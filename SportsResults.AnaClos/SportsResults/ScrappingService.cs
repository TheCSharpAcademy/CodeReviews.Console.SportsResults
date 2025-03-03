using HtmlAgilityPack;

namespace SportsResults;

public class ScrappingService
{
    string url = "https://www.basketball-reference.com/boxscores/";
    HtmlWeb htmlWeb = new HtmlWeb();
    HtmlDocument document;
    public ScrappingService()
    {
        document = htmlWeb.Load(url);
    }

    public string GetTitle()
    {
        string subject = document.DocumentNode.SelectNodes("//div/h1").First().InnerText;
        return subject;
    }

    public string GetResults()
    {
        string result = "";
        int count = 1;
        var gameSummariesDiv = document.DocumentNode.SelectSingleNode("//div[@class='game_summaries']");
        var gameSummaryDivs = gameSummariesDiv.SelectNodes(".//div[contains(@class, 'game_summary expanded nohover')]");

        foreach (var gameSummaryDiv in gameSummaryDivs)
        {
            var teamsTable = gameSummaryDiv.SelectSingleNode(".//table[@class='teams']");

            var winnerRow = teamsTable.SelectSingleNode(".//tr[@class='winner']");

            var winnerName = winnerRow.SelectSingleNode(".//a").InnerText;

            var scoreWinner = winnerRow.SelectSingleNode(".//td[@class='right']").InnerHtml;

            var loserRow = teamsTable.SelectSingleNode(".//tr[@class='loser']");

            var loserName = loserRow.SelectSingleNode(".//a").InnerText;

            var scoreLoser = loserRow.SelectSingleNode(".//td[@class='right']").InnerHtml;

            result += $"Game #{count}{System.Environment.NewLine}{winnerName}: {scoreWinner}{System.Environment.NewLine}{loserName} {scoreLoser}{System.Environment.NewLine}{System.Environment.NewLine}";
            count++;
        }
        return result;
    }
}