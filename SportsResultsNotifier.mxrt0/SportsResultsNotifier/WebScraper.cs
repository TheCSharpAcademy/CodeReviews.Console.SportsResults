using HtmlAgilityPack;
using System.Text;

namespace SportsResultsNotifier;

public class WebScraper
{
    private HtmlDocument _document;
    public const string BasketballReferenceUrl = "https://www.basketball-reference.com/boxscores/?month={0}&day={1}&year={2}";
    public async Task LoadWebDocument()
    {
        HtmlWeb web = new();
        Console.WriteLine("Loading web document...");

        string month = DateTime.Today.ToString("MM");
        string day = DateTime.Today.ToString("dd");
        string year = DateTime.Today.ToString("yyyy");
        var url = string.Format(BasketballReferenceUrl, month, day, year);
        _document = await web.LoadFromWebAsync(url);

        Console.WriteLine("\nSuccessfully loaded web document!");
    }

    public string FetchData()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"<h2>Daily NBA Games Report: ({DateTime.Today.ToString("dd-MM-yyyy")})</h2>");
        string gamesString = _document!.DocumentNode.SelectSingleNode(@"//*[@id=""content""]/div[2]/h2")?.InnerText ?? _document!.DocumentNode.SelectSingleNode(@"//*[@id=""content""]/div[2]/div[1]/p/strong").InnerText;

        if (gamesString.StartsWith("no", StringComparison.OrdinalIgnoreCase))
        {
            sb.AppendLine($"<p>{gamesString}</p>");
            return sb.ToString().TrimEnd();
        }

        sb.AppendLine(gamesString);
        var teamRows = _document!.DocumentNode
            .SelectNodes("//div[@class='game_summaries']//tr[@class='winner' or @class='loser']");

        int gameCount = teamRows.Count / 2;
        int numberToTake = 0;
        int quarterOffset = 0;
        int topScorerPts = 0;
        HtmlNode topScorer = _document.DocumentNode.FirstChild;
        for (int i = 0; i < gameCount; i++)
        {
            var row1 = teamRows[i * 2];
            var row2 = teamRows[i * 2 + 1];

            var team1 = row1.SelectSingleNode(".//a")?.InnerText.Trim();
            var score1 = row1.SelectSingleNode(".//td[@class='right']")?.InnerText.Trim();
            var scoreByQuarterTeam1 = _document!.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[3]/div/table[2]/tbody/tr[1]/td[@class='center']");

            var team2 = row2.SelectSingleNode(".//a")?.InnerText.Trim();
            var score2 = row2.SelectSingleNode(".//td[@class='right']")?.InnerText.Trim();
            var scoreByQuarterTeam2 = _document!.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[3]/div/table[2]/tbody/tr[2]/td[@class='center']");

            sb.AppendLine($"<h3>Game {i + 1} Result:</h3>");
            sb.AppendLine($"<h4>{team1}: {score1} pts.</h4>");
            sb.AppendLine($"<h4>{team2}: {score2} pts.</h4>");
            sb.AppendLine("<table border='1' cellpadding='5' cellspacing='0'>");

            sb.AppendLine("<tr><th>Team</th><th>Q1</th><th>Q2</th><th>Q3</th><th>Q4</th>");

            var margin = (team2!.Length > team1!.Length) ? new string(' ', team2.Length) : new string(' ', team1.Length);
            numberToTake = row2.Descendants().Any(n => string.Equals(n.InnerText.Trim(), "ot", StringComparison.OrdinalIgnoreCase)) ? 5 : 4;
            if (numberToTake == 5)
            {
                sb.AppendLine("<th>OT</th>");
            }

            sb.AppendLine("<tr>");

            var currentGameTopScorer = _document!.DocumentNode
            .SelectSingleNode($"//*[@id=\"content\"]/div[3]/div[{i + 1}]/table[3]/tbody/tr[1]");
            string currentGameTopScorerName = currentGameTopScorer.SelectSingleNode("./td[2]/a")?.InnerText ?? currentGameTopScorer.SelectSingleNode("./td[2]").InnerText;
            string currentGameTopScorerTeam = currentGameTopScorer.SelectSingleNode("./td[2]").InnerText;
            string currentGameTopScorerPts = currentGameTopScorer.SelectSingleNode("./td[@class='right']").InnerText;

            sb.AppendLine($"<tr><td>{team1}</td>{string.Join("", scoreByQuarterTeam1.Skip(quarterOffset).Take(numberToTake).Select(s => $"<td>{s.InnerText}</td>"))}</tr>");
            sb.AppendLine($"<tr><td>{team2}</td>{string.Join("", scoreByQuarterTeam2.Skip(quarterOffset).Take(numberToTake).Select(s => $"<td>{s.InnerText}</td>"))}</tr>");
            sb.AppendLine("</table>");

            quarterOffset += numberToTake;
            if (int.Parse(currentGameTopScorerPts.Trim()) > topScorerPts)
            {
                topScorerPts = int.Parse(currentGameTopScorerPts.Trim());
                topScorer = currentGameTopScorer;
            }

            sb.AppendLine($"<p>Top Scorer: {currentGameTopScorerName.Trim()} ({(currentGameTopScorerName != currentGameTopScorerTeam ? currentGameTopScorerTeam.Trim().Split('-')[1] : string.Empty)}) - {currentGameTopScorerPts.Trim()} pts.</p>");
        }
        string topScorerName = topScorer.SelectSingleNode("./td[2]/a")?.InnerText ?? topScorer.SelectSingleNode("./td[2]").InnerText;
        string topScorerTeam = topScorer.SelectSingleNode("./td[2]").InnerText;

        sb.AppendLine("<p>- - - - - - - - - - - - - - - - - - - - - - - - - -</p>");
        sb.AppendLine($"<strong>Today's Top Scorer: {topScorerName.Trim()} ({(topScorerName != topScorerTeam ? topScorerTeam.Trim().Split('-')[1] : string.Empty)}) - {topScorerPts} pts.</strong>");
        return sb.ToString().TrimEnd();
    }

}

