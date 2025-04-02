using System.Text;
using HtmlAgilityPack;

namespace SportsResultsNotifier;

internal class BasketballGame
{
    private string HomeTeam { get; set; }
    private string AwayTeam { get; set; }
    private bool HomeWin { get; set; }
    private string HomeTeamScore { get; set; }
    private string AwayTeamScore { get; set; }
    private bool AwayWin { get; set; }

    public BasketballGame(HtmlNode gameNode)
    {
        var trs = gameNode.Descendants("tr").ToList();
        
        var awayTds = trs[0].Descendants("td").ToList();
        AwayTeam = awayTds[0].InnerText;
        AwayTeamScore = awayTds[1].InnerText;
        AwayWin = trs[0].HasClass("winner");
        
        var homeTds = trs[1].Descendants("td").ToList();
        HomeTeam = homeTds[0].InnerText;
        HomeTeamScore = homeTds[1].InnerText;
        HomeWin = trs[1].HasClass("winner");
    }

    public string ScoreSummary()
    {
        var str = new StringBuilder();
        str.Append(AwayTeamSummary());
        str.Append(" at<br>");
        str.Append(HomeTeamSummary());
        str.Append("<br><br>");
        return str.ToString();
    }

    private string HomeTeamSummary()
    {
        var str = $"{HomeTeam} ({HomeTeamScore})";
        
        if (HomeWin)
        {
            str = $"<b>{str}</b>";
        }
        
        return str;
    }

    private string AwayTeamSummary()
    {
        var str = $"{AwayTeam} ({AwayTeamScore})";
        
        if (AwayWin)
        {
            str = $"<b>{str}</b>";
        }
        
        return str;
    }
}