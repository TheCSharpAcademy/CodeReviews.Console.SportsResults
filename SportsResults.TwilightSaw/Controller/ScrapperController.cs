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
        var teams = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'game_summary expanded nohover ']/table[not(@class) and not(@id)]//a").ToList();
        
        var pr = htmlDoc.DocumentNode.SelectNodes("//table[@class = 'stats']//td[2]").ToList();
        var pr2 = htmlDoc.DocumentNode.SelectNodes("//table[@class = 'stats']//td[3]").ToList();
        var pr3 = htmlDoc.DocumentNode.SelectNodes("//table[@class = 'stats']//strong").ToList();

        var c1 = htmlDoc.DocumentNode.SelectNodes("//div[@id = 'all_confs_standings_E']//tbody/tr[@class = 'full_table']").ToList();
        var c2 = htmlDoc.DocumentNode.SelectNodes("//div[@id = 'all_confs_standings_W']//tbody/tr[@class = 'full_table']").ToList();
        var head = htmlDoc.DocumentNode.SelectSingleNode("//div[@id = 'all_confs_standings_E']//thead/tr");
        var head2 = htmlDoc.DocumentNode.SelectSingleNode("//div[@id = 'all_confs_standings_W']//thead/tr");

        var roundScore = htmlDoc.DocumentNode.SelectNodes("//td[@class = 'center']").ToList();
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

        var x = head.ChildNodes.Where(node => node.Name is "th" or "td");
        var xx = head2.ChildNodes.Where(node => node.Name is "th" or "td");
        var x1 = string.Join("   ", x.Select(node => node.InnerText.Trim()));
        var xx1 = string.Join("   ", xx.Select(node => node.InnerText.Trim()));
        Console.WriteLine($"{x1}\t\t{xx1}\n");
        //Console.WriteLine(htmlDoc.DocumentNode.InnerHtml);
        for (var index = 0; index < c1.Count; index++)
        {
            var ctx = c1[index];
            var ctx2 = c2[index];
            var cellNodes = ctx.ChildNodes.Where(node => node.Name is "th" or "td");
            var cellNodes2 = ctx2.ChildNodes.Where(node => node.Name is "th" or "td");

            string values = string.Join("  ", cellNodes.Select(node => node.InnerText.Trim()));
            string values2 = string.Join("  ", cellNodes2.Select(node => node.InnerText.Trim()));
            Console.WriteLine($"{values}\t{"",-15}\t{values2}");
        }


        int.TryParse(htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'section_heading']/h2").InnerText.Remove(2), out var nbaGames);
        for (var index = 0; index < winnersList.Count; index++)
        {
            totalMessage += $"\n\t\t   {"1  2  3  4", +15}\n{teams[index].InnerText,-10}" +
                            $"\t\t{roundScore[0].InnerText} {roundScore[1].InnerText} {roundScore[2].InnerText} {roundScore[3].InnerText}" +
                            $"\n{teams[index+1].InnerText,-15} " +
                            $"\t{roundScore[4].InnerText} {roundScore[5].InnerText} {roundScore[6].InnerText} {roundScore[7].InnerText}\n" +
                            $"{pr3[index].InnerText}  {pr[index].InnerText,-25}\t----->\t{pr2[index].InnerText}\n" +
                            $"{pr3[index + 1].InnerText}  {pr[index + 1].InnerText,-25}\t----->\t{pr2[index + 1].InnerText}\n";
            roundScore.RemoveRange(0, 8);
            teams.RemoveRange(0,1);
            pr.RemoveRange(0,1);
            pr2.RemoveRange(0,1);
            pr3.RemoveRange(0,1);
        }

        return totalMessage;
    }
}