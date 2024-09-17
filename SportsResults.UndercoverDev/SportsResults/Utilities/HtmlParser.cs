using HtmlAgilityPack;
using SportsResults.Models;

namespace SportsResults.Utilities;
public class HtmlParser
{
    public List<SportData> ParseSportData(HtmlDocument doc)
    {
        var dataList = new List<SportData>();

        var nodes = doc.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[@class='game_summaries']/div/table[@class='teams']/tbody");

        foreach (var node in nodes)
        {
            var sportData = new SportData()
            {
                Winner = node.SelectSingleNode("tr[@class='winner']/td[1]").InnerText,
                Loser = node.SelectSingleNode("tr[@class='loser']/td[1]").InnerText,
                WinnerScore = int.Parse(node.SelectSingleNode("tr[@class='winner']/td[2]").InnerText),
                LoserScore = int.Parse(node.SelectSingleNode("tr[@class='loser']/td[2]").InnerText)
            };

            dataList.Add(sportData);
        }

        return dataList;
    }
}