using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace SportsResults.K_MYR;

public class WebScrapper : IWebScrapper
{
    private readonly ILogger<WebScrapper> _Logger;
    private readonly HtmlWeb Web = new();
    private readonly Uri Uri = new("https://www.basketball-reference.com/boxscores");
    private DateOnly LastDateWorkedOn;

    public WebScrapper(ILogger<WebScrapper> logger)
    {   
        _Logger = logger;
    }

    public List<GameSummary> GetGames()
    {
        var gameSummaries = new List<GameSummary>();

        var doc = Web.Load(Uri);

        var date = DateOnly.Parse(doc.DocumentNode.SelectSingleNode("//span[@class='button2 index']").InnerText);

        if (date.CompareTo(LastDateWorkedOn) == 0)
            return gameSummaries;
        
        LastDateWorkedOn = date;

        var nodes = doc.DocumentNode
                        .SelectNodes("/html/body/div[1]/div[4]/div[3]/div[position()>0]"); 

        if (nodes is not null)
        {
            foreach (var node in nodes)
            {                
                gameSummaries.Add(new GameSummary 
                (
                    node.SelectSingleNode("table[@class='teams']/tbody/tr[1]/td[1]/a").InnerText,
                    node.SelectSingleNode("table[@class='teams']/tbody/tr[2]/td[1]/a").InnerText,
                    node.SelectSingleNode("//tr[@class='winner']//td[1]/a").InnerText,
                    int.Parse(node.SelectSingleNode("table[@class='teams']/tbody/tr[1]/td[2]").InnerText),
                    int.Parse(node.SelectSingleNode("table[@class='teams']/tbody/tr[2]/td[2]").InnerText),
                    Uri + node.SelectSingleNode("table[1]/tbody/tr[1]/td[3]/a").Attributes["href"].Value,
                    node.SelectNodes("table[2]/tbody/tr[1]/td[position()>1]").Select(x=> int.Parse(x.InnerText)).ToArray(),
                    node.SelectNodes("table[2]/tbody/tr[2]/td[position()>1]").Select(x=> int.Parse(x.InnerText)).ToArray(),
                    node.SelectSingleNode("table[@class='stats']/tbody/tr[1]/td[2]/a").InnerText,
                    int.Parse(node.SelectSingleNode("table[3]/tbody/tr[1]/td[3]").InnerText),
                    node.SelectSingleNode("table[@class='stats']/tbody/tr[2]/td[2]/a").InnerText,
                    int.Parse(node.SelectSingleNode("table[3]/tbody/tr[2]/td[3]").InnerText),
                    date
                ));
            }
        }            

        return gameSummaries;
    }
}
