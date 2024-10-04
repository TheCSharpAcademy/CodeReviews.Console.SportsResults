using HtmlAgilityPack;
using SportsResults.Models;

namespace SportsResults;
internal class WebScraper
{
    public static (List<Game>, List<List<BoxScore>>) GetGames(string url)
    {
        List<Game> games = [];
        List<List<BoxScore>> boxScores = [];

        HtmlWeb web = new();
        var doc = web.Load(url);

        var nodes = doc.DocumentNode.SelectNodes($"//*[@class='game_summary expanded nohover ']");

        if (nodes is null)
            return (new(), new());

        foreach (var node in nodes)
        {
            Game game = new Game
            {
                Winner = node.SelectSingleNode(".//tr[@class='winner']/td[1]").InnerText,
                Loser = node.SelectSingleNode(".//tr[@class='loser']/td[1]").InnerText,
                WinnerScore = node.SelectSingleNode(".//tr[@class='winner']/td[2]").InnerText,
                LoserScore = node.SelectSingleNode(".//tr[@class='loser']/td[2]").InnerText
            };
            games.Add(game);

            (string winner, string loser) = GetTeamsShortNames(node);
            List<string> teams = [winner, loser];            

            HtmlNode boxScoreNode = node.SelectSingleNode(".//td[@class='right gamelink']/a");
            string boxScoreUrl = "https://www.basketball-reference.com" + boxScoreNode.Attributes["href"].Value;

            boxScores.Add(GetBoxScore(boxScoreUrl, teams));
        }
        return (games, boxScores);
    }

    public static List<BoxScore> GetBoxScore (string url, List<string> teams)
    {
        List<BoxScore> boxScore = [];

        HtmlWeb web = new();
        var doc = web.Load(url);

        foreach (string team in teams)
        {
            for (int i = 1; i <= 5; i++)
            {
                var nodes = doc.DocumentNode.SelectNodes($"//*[@id=\"box-{team}-game-basic\"]/tbody/tr[{i}]");

                foreach (var item in nodes)
                {
                    BoxScore singleRow = new BoxScore
                    {
                        Starters = item.SelectSingleNode("th").InnerText,
                        PTS = item.SelectSingleNode("td[19]").InnerText,
                        AST = item.SelectSingleNode("td[14]").InnerText,
                        BLK = item.SelectSingleNode("td[15]").InnerText,
                        STL = item.SelectSingleNode("td[16]").InnerText,
                        TRB = item.SelectSingleNode("td[13]").InnerText
                    };
                    boxScore.Add(singleRow);
                }
            }
        }
        return boxScore;
    }

    private static (string, string) GetTeamsShortNames(HtmlNode node)
    {
        HtmlNode winnerNode = node.SelectSingleNode(".//tr[@class='winner']/td[1]/a");
        string winnerShortName = winnerNode.Attributes["href"].Value;
        winnerShortName = winnerShortName.Substring(winnerShortName.IndexOf("/teams/") + "/teams/".Length, 3);

        HtmlNode loserNode = node.SelectSingleNode(".//tr[@class='loser']/td[1]/a");
        string loserShortName = loserNode.Attributes["href"].Value;
        loserShortName = loserShortName.Substring(loserShortName.IndexOf("/teams/") + "/teams/".Length, 3);

        return (winnerShortName, loserShortName);
    }
}
