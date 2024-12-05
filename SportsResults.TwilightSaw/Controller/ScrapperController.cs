using HtmlAgilityPack;
using System.Net;

namespace SportsResults.TwilightSaw.Controller;

public class ScrapperController
{
    public string GetWeb()
    {
        var web = new HtmlWeb();
        var htmlDoc = web.Load("https://www.basketball-reference.com/boxscores/");

        var title = htmlDoc.DocumentNode.SelectSingleNode("//title").InnerText;
        return title.Replace(" | Basketball-Reference.com", "");
    }
    public string GetMessage()
    {
        var web = new HtmlWeb();
        var htmlDoc = web.Load("https://www.basketball-reference.com/boxscores/");

        var winners = htmlDoc.DocumentNode.SelectNodes("//tr[@class = 'winner']/td/a");
        var losers = htmlDoc.DocumentNode.SelectNodes("//tr[@class = 'loser']/td/a");
        var winnersScore = htmlDoc.DocumentNode.SelectNodes("//tr[@class = 'winner']/td[@class = 'right']");
        var loserScore = htmlDoc.DocumentNode.SelectNodes("//tr[@class = 'loser']/td[@class = 'right']");

        var roundScore = htmlDoc.DocumentNode.SelectNodes("//td[@class = 'center']");
        var winnersList = winners.Where(p => p.InnerText != "Final").ToList();
        var losersList = losers.Where(p => p.InnerText != "Final").ToList();

        var totalMessage = $"{GetWeb()}\n\nWinners - Score\t\tLosers - Score\n\n";
        for (var index = 0; index < winnersList.Count; index++)
        {
            var clearedWinner = winnersScore.ToList()[index].InnerText.Replace("&nbsp;\n\t\t\t", " ");
            var clearedLoser = loserScore.ToList()[index].InnerText.Replace("&nbsp;\n\t\t\t", " ");
            if (clearedWinner.Equals(" ")) winnersScore.Remove(winnersScore[index]);
            if (clearedLoser.Equals(" ")) loserScore.Remove(loserScore[index]);
            totalMessage += ($"{winnersList[index].InnerText} - {winnersScore[index].InnerText}\t----->\t{losersList[index].InnerText} - {loserScore[index].InnerText}\n");
        }
        
        
        int.TryParse(htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'section_heading']/h2").InnerText.Remove(2), out var nbaGames);
        for (var index = 0; index < winnersList.Count; index++)
        {
            totalMessage += $"\n\t      1  2  3  4\n{winnersList[index].InnerText}\n\n{losersList[index].InnerText}\n";
        }

        return totalMessage;
    }
}