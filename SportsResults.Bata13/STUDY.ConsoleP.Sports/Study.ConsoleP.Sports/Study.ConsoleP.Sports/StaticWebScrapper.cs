using HtmlAgilityPack;

namespace Study.ConsoleP.Sports;
internal class StaticWebScrapper
{  
    public List<Result> GetResults(string url)
    {
        List<Result> results = new List<Result>();

        HtmlWeb web = new HtmlWeb();
            
        var doc = web.Load(url);

        var date = doc.DocumentNode
            .SelectNodes("//*[@id=\"content\"]/h1")
            .First()
            .InnerText
            .Replace("NBA Games Played on ", "");

        var numberOfGames = doc.DocumentNode
            .SelectNodes("//*[@id=\"content\"]/div[2]/h2")
            .First()
            .InnerText;

        Console.WriteLine($"There were {numberOfGames} on {date}");

        var numberOfScoreTables = doc.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[3]/div");

        if (numberOfScoreTables != null)
        {
            foreach (HtmlNode table in numberOfScoreTables)
            {
                var result = new Result
                {
                    TeamOne = table.SelectSingleNode(".//table[1]/tbody/tr[1]/td[1]/a").InnerText,
                    TeamOneScore = table.SelectSingleNode(".//table[1]/tbody/tr[1]/td[2]").InnerText,
                    TeamTwo = table.SelectSingleNode(".//table[1]/tbody/tr[2]/td[1]/a").InnerText,
                    TeamTwoScore = table.SelectSingleNode(".//table[1]/tbody/tr[2]/td[2]").InnerText
                };

                results.Add(result);
            }
        }

        return results;
    }
}