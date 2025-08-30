using HtmlAgilityPack;
using SportsResultsNotifier.Model;

namespace SportsResultsNotifier;

public static class WebScraper
{
    private static string _siteUrl = $"https://www.basketball-reference.com/boxscores/?month={DateTime.Now.Month}&day={DateTime.Now.Day}&year={DateTime.Now.Year}";

    public async static Task<List<GameData>?> FetchSportsDataAsync()
    {
        HtmlWeb web = new HtmlWeb();
        HtmlDocument doc = await web.LoadFromWebAsync(_siteUrl);

        return GetGameResults(doc);
    }

    private static List<GameData>? GetGameResults(HtmlDocument doc)
    {
        if(doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[2]/div[1]/p").InnerText.Equals("No games played on this date."))
        {
            return null;
        }

        var gameResults = new List<GameData>();
        var gameSchedule = doc.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[3]/div"); ////*[@id="content"]/div[2]/div[1]/p/strong

        foreach (var item in gameSchedule)
        {
            var teams = ExtractTeamData(item);
            var winningTeam = teams[0].TotalScore > teams[1].TotalScore ? teams[0] : teams[1];
            var additionalStats = ExtractAdditionalGameData(item);

            gameResults.Add(new()
            {
                TeamData = teams,
                WinningTeam = winningTeam,
                PtsStat = additionalStats[0],
                TrbStat = additionalStats[1]
            });
        }

        return gameResults;
    }

    private static List<TeamData> ExtractTeamData(HtmlNode item)
    {
        var teams = new List<TeamData>();

        TeamData awayTeam = new TeamData()
        {
            Name = item.SelectSingleNode("table[1]/tbody/tr[1]/td[1]").InnerText,
            TotalScore = Int32.Parse(item.SelectSingleNode("table[1]/tbody/tr[1]/td[2]").InnerText)
        };
        var quarterPerformance = item.SelectNodes("table[2]/tbody/tr[1]/td");
        foreach (var quarter in quarterPerformance)
        {
            if (Int32.TryParse(quarter.InnerText, out int points))
            {
                awayTeam.ScoreByQuarter.Add(points);
            }
        }

        TeamData homeTeam = new TeamData()
        {
            Name = item.SelectSingleNode("table[1]/tbody/tr[2]/td[1]").InnerText,
            TotalScore = Int32.Parse(item.SelectSingleNode("table[1]/tbody/tr[2]/td[2]").InnerText)
        };
        quarterPerformance = item.SelectNodes("table[2]/tbody/tr[2]/td");
        foreach (var quarter in quarterPerformance)
        {
            if (Int32.TryParse(quarter.InnerText, out int points))
            {
                homeTeam.ScoreByQuarter.Add(points);
            }
        }

        teams.Add(awayTeam);
        teams.Add(homeTeam);

        return teams;
    }

    private static List<string> ExtractAdditionalGameData(HtmlNode item)
    {
        var data = new List<string>();

        var PtsStat = item.SelectSingleNode("table[3]/tbody/tr[1]/td[2]").InnerText 
            + " (" + item.SelectSingleNode("table[3]/tbody/tr[1]/td[3]").InnerText + ")";
        
        var TrbStat = item.SelectSingleNode("table[3]/tbody/tr[2]/td[2]").InnerText 
            + " (" + item.SelectSingleNode("table[3]/tbody/tr[2]/td[3]").InnerText + ")";

        data.Add(PtsStat);
        data.Add(TrbStat);

        return data;
    }
}
