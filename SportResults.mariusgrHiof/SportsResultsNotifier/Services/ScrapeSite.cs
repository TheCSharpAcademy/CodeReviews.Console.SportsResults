using HtmlAgilityPack;
using SportsResultsNotifier.Models;
namespace SportsResultsNotifier.Services;
public class ScrapeSite
{
    public readonly string URL = "https://www.basketball-reference.com/boxscores";

    HtmlWeb web = new HtmlWeb();

    public void GetData()
    {
        HtmlNode tableContainer = null;

        var htmlDoc = web.Load(URL);

        var matchDate = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/h1");

        var noGames = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[2]/div[1]/p/strong");

        string bodyHtml = string.Empty;

        if (noGames == null)
        {
            string team1 = string.Empty;
            string team2 = string.Empty;
            int team1Score = 0;
            int team2Score = 0;

            List<Match> matches = new List<Match>();

            HtmlNode thead = null;
            HtmlNode tbody = null;

            var gameSummaries = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[3]");
            var gameDivs = gameSummaries.SelectNodes("div");

            foreach (var gameDiv in gameDivs)
            {
                HtmlNode table = gameDiv.SelectSingleNode("table");

                team1 = gameDiv.SelectSingleNode("table/tbody/tr[1]/td[1]/a").InnerText;
                team1Score = Int32.Parse(gameDiv.SelectSingleNode("table/tbody/tr[1]/td[2]").InnerText);

                team2 = gameDiv.SelectSingleNode("table/tbody/tr[2]/td[1]/a").InnerText;
                team2Score = Int32.Parse(gameDiv.SelectSingleNode("table/tbody/tr[2]/td[2]").InnerText);

                var match = new Match
                {
                    Team1 = team1,
                    Team2 = team2,
                    Team1Score = team1Score,
                    Team2Score = team2Score

                };
                matches.Add(match);
            }

            tableContainer = htmlDoc.CreateElement("table");
            tableContainer.SetAttributeValue("style", "font-family: sans-serif; " +
                "width: 700px;" +
                "border-collapse: collapse;");

            thead = htmlDoc.CreateElement("thead");

            HtmlNode trHeading = htmlDoc.CreateElement("tr");
            HtmlNode thHeading = htmlDoc.CreateElement("th");

            trHeading.SetAttributeValue("style", "background-color: #011B51; " +
                "color: white;" +
                "font-size: 1.2rem;" +
                "padding: 10px 0px;" +
                "border: 1px solid #dddddd;");

            thHeading.SetAttributeValue("colspan", "3");
            thHeading.InnerHtml = matchDate.InnerHtml;

            trHeading.AppendChild(thHeading);

            thead.AppendChild(trHeading);

            tbody = htmlDoc.CreateElement("tbody");


            foreach (var match in matches)
            {
                AddMatchResults(tbody, htmlDoc, match);
            }

            tableContainer.AppendChild(thead);
            tableContainer.AppendChild(tbody);
        }
        else
        {
            bodyHtml = "No matches today";
        }

        bodyHtml = tableContainer.OuterHtml;

        string from = "marius.gravningsmyhr@gmail.com";
        string to = "marius.gravningsmyhr@gmail.com";
        string subject = "Matchday results";

        MailService mailService = new MailService(from, to, subject, bodyHtml);
        mailService.SendEmail();
    }

    private static void AddMatchResults(HtmlNode tbody, HtmlDocument htmlDoc, Match match)
    {
        HtmlNode trRow = htmlDoc.CreateElement("tr");
        HtmlNode tdTeam1 = htmlDoc.CreateElement("td");
        HtmlNode tdTeam2 = htmlDoc.CreateElement("td");
        HtmlNode tdResult = htmlDoc.CreateElement("td");

        tdTeam1.SetAttributeValue("style", "text-align: right;" +
            "font-size: 1.2rem;" +
            "border: 1px solid #dddddd;" +
            "padding: 6px;");
        tdTeam1.InnerHtml = match.Team1;

        tdResult.SetAttributeValue("style", "background-color: #D6181F;" +
            "font-size: 1rem;" +
            "text-align: center;" +
            "color:white;" +
            "width: 70px;" +
            "border-top: 2px solid grey;  " +
            "border: 1px solid #dddddd;" +
            "padding: 6px;");
        tdResult.InnerHtml = $"{match.Team1Score} - {match.Team2Score}";

        tdTeam2.SetAttributeValue("style", "text-align: left;" +
            "font-size: 1.2rem;" +
            "border: 1px solid #dddddd;" +
            "padding: 6px;");
        tdTeam2.InnerHtml = match.Team2;

        trRow.AppendChild(tdTeam1);
        trRow.AppendChild(tdResult);
        trRow.AppendChild(tdTeam2);

        tbody.AppendChild(trRow);
    }
}

