using HtmlAgilityPack;
using System.Collections.Generic;
using System.Net;
using SportsResults.TwilightSaw.Models;
using System;
using Org.BouncyCastle.Utilities.IO;

namespace SportsResults.TwilightSaw.Controllers;

public class ScrapperController
{
    private readonly HtmlDocument? _htmlDoc = new HtmlWeb().Load("https://www.basketball-reference.com/boxscores/");
    public string GetWeb()
    {
        var title = _htmlDoc.DocumentNode.SelectSingleNode("//title").InnerText;
        return title.Replace(" | Basketball-Reference.com", "");
    }

    public List<Game> GetGames()
    {
        var winners = _htmlDoc.DocumentNode.SelectNodes("//tr[@class = 'winner']/td/a").Where(p => p.InnerText != "Final").ToList();
        var losers = _htmlDoc.DocumentNode.SelectNodes("//tr[@class = 'loser']/td/a").Where(p => p.InnerText != "Final").ToList();

        var winnersScore = _htmlDoc.DocumentNode.SelectNodes("//tr[@class = 'winner']/td[@class = 'right'][1]");
        var losersScore = _htmlDoc.DocumentNode.SelectNodes("//tr[@class = 'loser']/td[@class = 'right'][1]");

        var gamesList = new List<Game>();
        for (var index = 0; index < winners.Count; index++)
        {
            var clearedWinnerScore = winnersScore.ToList()[index].InnerText
                .Replace("&nbsp;\n\t\t\t", " ")
                .Replace("OT", " ");
            var clearedLoser = losersScore.ToList()[index].InnerText
                .Replace("&nbsp;\n\t\t\t", " ")
                .Replace("OT", " "); ;
            if (clearedWinnerScore.Equals(" ")) winnersScore.Remove(winnersScore[index]);
            if (clearedLoser.Equals(" ")) losersScore.Remove(losersScore[index]);
            int.TryParse(winnersScore[index].InnerText, out var winnerScore);
            int.TryParse(losersScore[index].InnerText, out var loserScore);

           gamesList.Add(new Game(losers[index].InnerText,winners[index].InnerText,
                winnerScore, loserScore));
        }

        return gamesList;
    }

    public List<GameStatistic> GetGameStatistics()
    {

        var winners = _htmlDoc.DocumentNode.SelectNodes("//tr[@class = 'winner']/td/a").Where(p => p.InnerText != "Final").ToList();
        var losers = _htmlDoc.DocumentNode.SelectNodes("//tr[@class = 'loser']/td/a").Where(p => p.InnerText != "Final").ToList();

        var winnersList = winners.Where(p => p.InnerText != "Final").ToList();
        var losersList = losers.Where(p => p.InnerText != "Final").ToList();

        var totalMessage = "";


        var teams = _htmlDoc.DocumentNode.SelectNodes("//div[@class = 'game_summary expanded nohover ']/table[not(@class) and not(@id)]//a").ToList();

        var playerStatsName = _htmlDoc.DocumentNode.SelectNodes("//table[@class = 'stats']//td[2]").ToList();
        var playerStats = _htmlDoc.DocumentNode.SelectNodes("//table[@class = 'stats']//td[3]").ToList();
        var statsName = _htmlDoc.DocumentNode.SelectNodes("//table[@class = 'stats']//strong").ToList();
        var roundScore = _htmlDoc.DocumentNode.SelectNodes("//td[@class = 'center']").ToList();

        var gameStatList = new List<GameStatistic>();

        for (var index = 0; index < winnersList.Count; index++)
        {
            totalMessage += $"\n\t\t   {"1  2  3  4",+15}\n{teams[index].InnerText,-10}" +
                            $"\t\t{roundScore[0].InnerText} {roundScore[1].InnerText} {roundScore[2].InnerText} {roundScore[3].InnerText}" +
                            $"\n{teams[index + 1].InnerText,-15} " +
                            $"\t{roundScore[4].InnerText} {roundScore[5].InnerText} {roundScore[6].InnerText} {roundScore[7].InnerText}\n" +
                            $"{statsName[index].InnerText}  {playerStatsName[index].InnerText,-25}\t----->\t{playerStats[index].InnerText}\n" +
                            $"{statsName[index + 1].InnerText}  {playerStatsName[index + 1].InnerText,-25}\t----->\t{playerStats[index + 1].InnerText}\n";

            var winnerStatsNodeList = new List<HtmlNode> { roundScore[0], roundScore[1], roundScore[2], roundScore[3]};
            var winnerStatsList = new List<int>();

            var loserStatsNodeList = new List<HtmlNode> { roundScore[4], roundScore[5], roundScore[6], roundScore[7]};
            var loserStatsList = new List<int>();
            for (var i = 0; i < 4; i++)
            {
                var winnerStat = winnerStatsNodeList[i];
                int.TryParse(winnerStat.InnerText, out var winnerStatResult);
                winnerStatsList.Add(winnerStatResult);
            }
            for (var i = 0; i < 4; i++)
            {
                var loserStat = loserStatsNodeList[i];
                int.TryParse(loserStat.InnerText, out var loserStatResult);
                winnerStatsList.Add(loserStatResult);
            }

            gameStatList.Add(new GameStatistic(teams[index].InnerText
                ,teams[index + 1].InnerText
                ,winnerStatsList
                ,loserStatsList));

            roundScore.RemoveRange(0, 8);
            teams.RemoveRange(0, 1);
            playerStatsName.RemoveRange(0, 1);
            playerStats.RemoveRange(0, 1);
            statsName.RemoveRange(0, 1);

           

        }

        return gameStatList;
    }
    public string GetMessage()
    {
        var winners = _htmlDoc.DocumentNode.SelectNodes("//tr[@class = 'winner']/td/a");
        var losers = _htmlDoc.DocumentNode.SelectNodes("//tr[@class = 'loser']/td/a");

        var winnersScore = _htmlDoc.DocumentNode.SelectNodes("//tr[@class = 'winner']/td[@class = 'right']");
        var loserScore = _htmlDoc.DocumentNode.SelectNodes("//tr[@class = 'loser']/td[@class = 'right']");

        var teams = _htmlDoc.DocumentNode.SelectNodes("//div[@class = 'game_summary expanded nohover ']/table[not(@class) and not(@id)]//a").ToList();
        
        var playerStatsName = _htmlDoc.DocumentNode.SelectNodes("//table[@class = 'stats']//td[2]").ToList();
        var playerStats = _htmlDoc.DocumentNode.SelectNodes("//table[@class = 'stats']//td[3]").ToList();
        var statsName = _htmlDoc.DocumentNode.SelectNodes("//table[@class = 'stats']//strong").ToList();

        var standingsEast = _htmlDoc.DocumentNode.SelectNodes("//div[@id = 'all_confs_standings_E']//tbody/tr[@class = 'full_table']").ToList();
        var standingsWest = _htmlDoc.DocumentNode.SelectNodes("//div[@id = 'all_confs_standings_W']//tbody/tr[@class = 'full_table']").ToList();

        var eastStandingsName = _htmlDoc.DocumentNode.SelectNodes("//div[@id = 'all_confs_standings_E']//thead/tr").ToList();
        var westStandingsName = _htmlDoc.DocumentNode.SelectNodes("//div[@id = 'all_confs_standings_W']//thead/tr").ToList();

        eastStandingsName.AddRange(standingsEast);
        westStandingsName.AddRange(standingsWest);

        var roundScore = _htmlDoc.DocumentNode.SelectNodes("//td[@class = 'center']").ToList();
        var winnersList = winners.Where(p => p.InnerText != "Final").ToList();
        var losersList = losers.Where(p => p.InnerText != "Final").ToList();

        var totalMessage = $"{GetWeb()}\n\nWinners - Score\t\t\t\tLosers - Score\n\n";
        for (var index = 0; index < winnersList.Count; index++)
        {
            var clearedWinner = winnersScore.ToList()[index].InnerText.Replace("&nbsp;\n\t\t\t", " ");
            var clearedLoser = loserScore.ToList()[index].InnerText.Replace("&nbsp;\n\t\t\t", " ");

            if (clearedWinner.Equals(" ")) winnersScore.Remove(winnersScore[index]);
            if (clearedLoser.Equals(" ")) loserScore.Remove(loserScore[index]);

            totalMessage += ($"{winnersList[index].InnerText} - {winnersScore[index].InnerText,-15}\t----->\t{losersList[index].InnerText} - {loserScore[index].InnerText}\n");
        }

        return totalMessage;
      /*  for (var index = 0; index < winnersList.Count; index++)
        {
            totalMessage += $"\n\t\t   {"1  2  3  4", +15}\n{teams[index].InnerText,-10}" +
                            $"\t\t{roundScore[0].InnerText} {roundScore[1].InnerText} {roundScore[2].InnerText} {roundScore[3].InnerText}" +
                            $"\n{teams[index+1].InnerText,-15} " +
                            $"\t{roundScore[4].InnerText} {roundScore[5].InnerText} {roundScore[6].InnerText} {roundScore[7].InnerText}\n" +
                            $"{statsName[index].InnerText}  {playerStatsName[index].InnerText,-25}\t----->\t{playerStats[index].InnerText}\n" +
                            $"{statsName[index + 1].InnerText}  {playerStatsName[index + 1].InnerText,-25}\t----->\t{playerStats[index + 1].InnerText}\n";
            roundScore.RemoveRange(0, 8);
            teams.RemoveRange(0,1);
            playerStatsName.RemoveRange(0,1);
            playerStats.RemoveRange(0,1);
            statsName.RemoveRange(0,1);
        }
        for (var index = 0; index <= standingsEast.Count; index++)
        {
            var standingsEastRow = eastStandingsName[index];
            var standingsEastList = standingsEastRow.ChildNodes.Where(node => node.Name is "td" or "th").Select(node => node.InnerText.Replace("&mdash;", "-").Trim()).ToList();

            totalMessage += (index == 0 ? "\n\n" : "") +
                $"{standingsEastList[0],-25} {standingsEastList[1],3} {standingsEastList[2],3} {standingsEastList[3],6}" +
                $" {standingsEastList[4],5} {standingsEastList[5],6} {standingsEastList[6],6}" + (index == 0 ? "\n" : "");
        }
        for (var index = 0; index <= standingsWest.Count; index++)
        {
            var standingsWestRow = westStandingsName[index];
            var standingsWestList = standingsWestRow.ChildNodes.Where(node => node.Name is "td" or "th").Select(node => node.InnerText.Replace("&mdash;", "-").Trim()).ToList();

            totalMessage += (index == 0 ? "\n\n" : "") +
                            $"{standingsWestList[0],-25} {standingsWestList[1],3} {standingsWestList[2],3} {standingsWestList[3],6} " +
                            $"{standingsWestList[4],5} {standingsWestList[5],6} {standingsWestList[6],6}" + (index == 0 ? "\n" : "");
        }
      */
        return totalMessage;
    }
}