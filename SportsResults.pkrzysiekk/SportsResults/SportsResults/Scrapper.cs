using System.Globalization;
using HtmlAgilityPack;
using SportsResults.Models;

namespace SportsResults;

public class Scrapper
{
    private readonly string _url;

    public Scrapper(string url)
    {
       _url = url; 
    }

    private string[] ProcessResultRow(HtmlNodeCollection row)
    {
       string[] result = new string[2];
        
       result[0] = row[0].SelectSingleNode("a").InnerText.Trim();
       result[1] = row[1].InnerText.Trim();
       
       return result;
    }

    private DateTime GetMatchDate(HtmlDocument html)
    {
        var dateNode=html.DocumentNode.SelectSingleNode("//span[@class='button2 index']").InnerText.Trim();
        DateTime date = DateTime.Parse(dateNode,CultureInfo.InvariantCulture);
        return date;
    }
    public List<Match> GetMatchPlayers(HtmlNodeCollection nodes)
    {
        List<Match> matches = new List<Match>();
        foreach (HtmlNode node in nodes)
        {
            var tableInfo = node.SelectSingleNode("//table[@class='teams']/tbody");
            var loserTr=tableInfo.SelectSingleNode("//tr[@class='loser']");
            var winnerTr=tableInfo.SelectSingleNode("//tr[@class='winner']");

            var loserTds = loserTr.SelectNodes("./td");
            var winnerTds = winnerTr.SelectNodes("./td");

            var parsedLoser = ProcessResultRow(loserTds);
            var parsedWinner = ProcessResultRow(winnerTds);
            
            //string loserTeam = 

        }
        return matches;
    }
    public List<Match> ScrapeMatches()
    {
        var web= new HtmlWeb();
        var html=web.Load(_url);
        var matchesDiv=html.DocumentNode.SelectNodes("//div[@class='game_summaries']");
        var matchDate = GetMatchDate(html);
        var matches=GetMatchPlayers(matchesDiv);
        return matches;
    }
}