using HtmlAgilityPack;
using HtmlParser;

public class HtmlParserClass
{
    public static async Task<List<Match_>> Demo(string urlParameter)
    {
        string url = urlParameter;
        string htmlContent = await DownloadHtmlAsync(url);

        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(htmlContent);

        var matchResultNodes = doc.DocumentNode
            .SelectNodes("//div[@class='game_summary expanded nohover ']").ToList();

        List<Match_> matches;

        if (matchResultNodes is not null)
        {
            matches = new();

            foreach (var match in matchResultNodes)
            {
                Team t1 = new()
                {
                    Name = match.SelectSingleNode(".//table/tbody/tr[1]/td[1]").InnerText,
                    Points = int.Parse(match.SelectSingleNode(".//table/tbody/tr[1]/td[2]").InnerText)
                };

                Team t2 = new()
                {
                    Name = match.SelectSingleNode(".//table/tbody/tr[2]/td[1]").InnerText,
                    Points = int.Parse(match.SelectSingleNode(".//table/tbody/tr[2]/td[2]").InnerText)
                };

                Match_ matchAux = new()
                {
                    Team1 = t1,
                    Team2 = t2
                };

                matches.Add(matchAux);

            }

            return matches;
        }

        else
        {
            await Console.Out.WriteLineAsync("No match result elements found.");
            return null!;
        }


    }

    static async Task<string> DownloadHtmlAsync(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception($"Failed to download HTML content. Status code: {response.StatusCode}");
            }
        }
    }


}
