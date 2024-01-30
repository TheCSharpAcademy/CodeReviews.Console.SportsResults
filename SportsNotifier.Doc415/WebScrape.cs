using HtmlAgilityPack;
using SportsNotifier.Models;
namespace SportsNotifier;

internal class WebScrape
{
    private string _url = "https://www.basketball-reference.com/boxscores/";
    private List<Game> games = new();

    public void SetScrapeUrl(string url)
    {
        _url = url;
    }

    public (string Title, List<Game> Games) GetGames()
    {
        HtmlWeb web = new();
        string url = "https://www.basketball-reference.com/boxscores/";
        HtmlDocument doc = web.Load(url);
        try
        {
            var title = doc.GetElementbyId("content").SelectSingleNode("//h1").InnerText;
            var resultTable = doc.DocumentNode.SelectNodes("//div[@class='game_summary expanded nohover ']").ToList();
            foreach (var node in resultTable)
            {
                games.Add(new Game
                {
                    Winner = node.SelectSingleNode(".//table/tbody/tr[1]/td[1]").InnerText,
                    Looser = node.SelectSingleNode(".//table/tbody/tr[2]/td[1]").InnerText,
                    WinnerScore = node.SelectSingleNode(".//table/tbody/tr[1]/td[2]").InnerText,
                    LooserScore = node.SelectSingleNode(".//table/tbody/tr[2]/td[2]").InnerText
                });
            }
            return (title, games);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"There was an error getting data from web: {ex.Message}");
        }

        return (null, null);
    }
}
