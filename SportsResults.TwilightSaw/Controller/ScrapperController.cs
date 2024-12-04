using HtmlAgilityPack;

namespace SportsResults.TwilightSaw.Controller;

public class ScrapperController
{
    public void GetWeb()
    {
        var web = new HtmlWeb();
        var htmlDoc = web.Load("https://www.basketball-reference.com/boxscores/");

        var title = htmlDoc.DocumentNode.SelectSingleNode("//title").InnerText;
        Console.WriteLine(title);
    }
    public void GetParagraphs()
    {
        var web = new HtmlWeb();
        var htmlDoc = web.Load("https://www.basketball-reference.com/boxscores/");

        var winners = htmlDoc.DocumentNode.SelectNodes("//tr[@class = 'winner']/td/a");
        var winnersScore = htmlDoc.DocumentNode.SelectNodes("//tr[@class = 'winner']/td[@class = 'right']");
        var t = winners.Where(p => p.InnerText != "Final").ToList();
        var t1 = winnersScore.Where(p => p.InnerText != "\u00A0").ToList();
        for (var index = 0; index < t.Count; index++)
        {
            var paragraph = t[index];
            Console.WriteLine($"{paragraph.InnerText} {t1[index].InnerText}");
        }
    }
}