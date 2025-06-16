using HtmlAgilityPack;
using SportResultsNotifier.Models;

namespace SportResultsNotifier.Controllers;

class ResultsController
{
    // test values
    private static DateTime Today = DateTime.Now.AddDays(-9);
    public static int AppIteration = 0;

    internal static Results GetResults()
    {
        // test values
        DateTime today = Today.AddDays(AppIteration);
        AppIteration++;

        // today real value
        //DateTime today = DateTime.Now;
        Results results = new();
        try
        {

            string url = $"https://www.basketball-reference.com/boxscores/?month={today.Month}&day={today.Day}&year={today.Year}";
            HtmlWeb web = new();
            HtmlDocument document = web.Load(url);

            Game previousGame = new();
            Game game = new();
            string outerHtml;
            int index = 1;

            List<HtmlNode> nodes = document.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[3]/div[position()>0]").ToList();
            results.Body = "<hr><div><table  width=80% align=\"center\" style=\"table-layout:fixed\"><tbody><tr>\r\n    <td width=30% >Home</td><td width=30%>Away</td>\r\n</tr></tbody></table></div><hr>\r\n\r\n";
            foreach (HtmlNode node in nodes)
            {
                game.HomeTeam = node.SelectSingleNode($"//div[{index}]/table[1]/tbody/tr[1]/td[1]").InnerText;
                game.AwayTeam = node.SelectSingleNode($"//div[{index}]/table[1]/tbody/tr[2]/td[1]").InnerText;
                game.HomeScore = int.Parse(node.SelectSingleNode($"//div[{index}]/table[1]/tbody/tr[1]/td[2]").InnerText);
                game.AwayScore = int.Parse(node.SelectSingleNode($"//div[{index}]/table[1]/tbody/tr[2]/td[2]").InnerText);
                outerHtml = node.SelectSingleNode($"//*[@id=\"content\"]/div[3]/div[{index}]/table[1]/tbody/tr[1]/td[1]/a").OuterHtml;
                game.HomeTeamRef = outerHtml.Substring(16, 3);
                outerHtml = node.SelectSingleNode($"//*[@id=\"content\"]/div[3]/div[{index}]/table[1]/tbody/tr[2]/td[1]/a").OuterHtml;
                game.AwayTeamRef = outerHtml.Substring(16, 3);

                results = FormatResults(results, game, today, index);
                index++;
            }
            results.Subject = $"NBA Games result({today.Date.ToString("dd/MM/yyyy")})";
            return results;
        }
        catch { results.Subject = $"No results for{today.Date.ToString("dd/MM/yyyy")}"; results.Body = "No Results"; return results; }
    }

    private static Results FormatResults(Results results, Game game, DateTime today, int index)
    {
        string gameResume = "";
        string homeStyle = "";
        string awaysStyle = "";
        game.GameRef = $"https://www.basketball-reference.com/boxscores/{today.ToString("yyyyMMdd")}0{game.AwayTeamRef}.html";
        if (!game.NoWinner)
        {
            homeStyle = game.HomeWin ? "style=\"color: green;\"" : "style=\"color: red;\"";
            awaysStyle = game.HomeWin ? "style=\"color: red;\"" : "style=\"color: green;\"";
        }

        gameResume = "<hr><div><table  width=80% align=\"center\" style=\"table-layout:fixed\"><tbody><tr>\n" +
            $"\r\n\t<td WIDTH=10%><a href=\"https://www.basketball-reference.com/teams/{game.HomeTeamRef}/{today.Year}.html\" ><img height=30 class=\"teamlogo\" src=\"https://cdn.ssref.net/req/202504041/tlogo/bbr/{game.HomeTeamRef}-{today.Year}.png\" alt=\"Home team Logo\"/></a></td>" +
            $"\t<td width=30% {homeStyle}>{game.HomeTeam}</td>\r\t<td width=20%>{game.HomeScore}</td>\r" +
            $"\r\n\t<td WIDTH=10%><a href=\"https://www.basketball-reference.com/teams/{game.AwayTeamRef}/{today.Year}.html\" ><img height=30 class=\"teamlogo\" src=\"https://cdn.ssref.net/req/202504041/tlogo/bbr/{game.AwayTeamRef}-{today.Year}.png\" alt=\"Away team Logo\"/></a></td>" +
            $"\t<td width=30% {awaysStyle}>{game.AwayTeam}</td>\r\t<td width=20%>{game.AwayScore}</td>\r" +
            $"<td><a href=\"https://www.basketball-reference.com/boxscores/{today.ToString("yyyyMMdd")}0{game.AwayTeamRef}.html\">details</a></td>\r" +
            $"</tr></tbody></table></div>\n\r";

        results.Body += gameResume;
        return results;
    }
}
