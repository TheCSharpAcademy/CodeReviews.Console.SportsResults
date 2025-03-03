using HtmlAgilityPack;
using System;

namespace SportsResults;

internal class Program
{
    static void Main(string[] args)
    {
        var html = @"https://www.basketball-reference.com/boxscores/";

        HtmlWeb web = new HtmlWeb();
        var htmlDoc = web.Load(html);

        var nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[3]");

        foreach (var node in nodes)
        {
            var gameSummaryNode = node.SelectSingleNode("//*[@class='game_summary expanded nohover ']");
            if (gameSummaryNode != null)
            {
                var winnerNode = gameSummaryNode.SelectSingleNode(".//tr[@class='winner']");
                var loserNode = gameSummaryNode.SelectSingleNode(".//tr[@class='loser']");

                if (winnerNode != null && loserNode != null)
                {
                    var winnerTeam = winnerNode.SelectSingleNode(".//td[1]/a").InnerText.Trim();
                    var winnerScore = winnerNode.SelectSingleNode(".//td[2]").InnerText.Trim();

                    var loserTeam = loserNode.SelectSingleNode(".//td[1]/a").InnerText.Trim();
                    var loserScore = loserNode.SelectSingleNode(".//td[2]").InnerText.Trim();

                    Console.WriteLine($"Winner: {winnerTeam} with score {winnerScore}");
                    Console.WriteLine($"Loser: {loserTeam} with score {loserScore}");
                }
                else
                {
                    Console.WriteLine("Winner or loser node not found.");
                }
            }
            else
            {
                Console.WriteLine("Game summary node not found.");
            }
        }
    }
}
