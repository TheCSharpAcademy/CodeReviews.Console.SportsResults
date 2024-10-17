using HtmlAgilityPack;

namespace SportsResults;

public class Scrapper
{
    public Uri Uri { get; set; }
    public Scrapper(string url)
    {
        Uri = new Uri(url);
    }
    public List<Match> Run()
    {
        HtmlWeb web = new HtmlWeb();
        HtmlDocument document = web.Load(Uri);

        var matchTable = document.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[3]/div/table[@class='teams']/tbody");
        var scores = document.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[3]/div/table[1]/tbody/tr/td[2]");

        List<Match> matches = new List<Match>();
        foreach (var match in matchTable)
        {
            Team winner = new()
            {
                Name = match.SelectSingleNode("tr[@class='winner']/td[1]/a[1]").InnerText,
                Score = Convert.ToInt32(match.SelectSingleNode("tr[@class='winner']/td[2]").InnerText)
            };

            Team loser = new()
            {
                Name = match.SelectSingleNode("tr[@class='loser']/td[1]/a[1]").InnerText,
                Score = Convert.ToInt32(match.SelectSingleNode("tr[@class='loser']/td[2]").InnerText)
            };

            matches.Add(new Match()
            {
                Winner = winner,
                Loser = loser
            });
        }

        return matches;
    }
}