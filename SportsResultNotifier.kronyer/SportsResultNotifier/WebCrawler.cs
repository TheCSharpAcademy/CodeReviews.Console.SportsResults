using HtmlAgilityPack;

namespace SportsResultNotifier
{
    internal class WebCrawler
    {
        public static Dictionary<string, string> GetContentHtml()
        {
            Dictionary<string, string> content = new Dictionary<string, string>();
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load("https://www.basketball-reference.com/boxscores/");

            var title = document.DocumentNode.SelectNodes("//*[@id=\"content\"]/h1").First().InnerText;
            content.Add("title", title);

            var games = document.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[2]/h2").First().InnerText;
            content.Add("games", games);

            var tableLenght = document.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[3]/div").Count();
            content.Add("tableLenght", tableLenght.ToString());

            for (int i = 1; i <= tableLenght; i++)
            {
                var teamOne = document.DocumentNode.SelectNodes($"//*[@id=\"content\"]/div[3]/div[{i}]/table[1]/tbody/tr[1]/td[1]/a").First().InnerText;
                var teamOnePoints = document.DocumentNode.SelectNodes($"//*[@id=\"content\"]/div[3]/div[{i}]/table[1]/tbody/tr[1]/td[2]").First().InnerText;
                var teamTwo = document.DocumentNode.SelectNodes($"//*[@id=\"content\"]/div[3]/div[{i}]/table[1]/tbody/tr[2]/td[1]/a").First().InnerText;
                var teamTwoPoints = document.DocumentNode.SelectNodes($"//*[@id=\"content\"]/div[3]/div[{i}]/table[1]/tbody/tr[2]/td[2]").First().InnerText;

                content.Add($"teamOne{i}", $"{teamOne} - {teamOnePoints}");
                content.Add($"teamTwo{i}", $"{teamTwo} - {teamTwoPoints}");
            }
            return content;
        }
    }
}
