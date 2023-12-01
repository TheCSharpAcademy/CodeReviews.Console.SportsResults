using HtmlAgilityPack;
using SportsResult.Vocalnight;

partial class Program
{
    static void LoadInformation()
    {
        var url = "https://www.basketball-reference.com/boxscores/";
        var web = new HtmlWeb();
        var doc = web.Load(url);

        List<Game> matchesList = new List<Game>();

        var games = doc.DocumentNode.SelectNodes("//*[@class='game_summary expanded nohover ']");

        if (games == null)
        {
            Console.WriteLine("No games found. Try again later!");
            return;
        }

        foreach (var match in games)
        {
            Game game = new Game();

            game.WinnerName = match.SelectSingleNode(".//tr[@class='winner']/td[1]").InnerText;
            game.LoserName = match.SelectSingleNode(".//tr[@class='loser']/td[1]").InnerText;

            game.WinnerScore = match.SelectSingleNode(".//tr[@class='winner']/td[2]").InnerText;
            game.LoserScore = match.SelectSingleNode(".//tr[@class='loser']/td[2]").InnerText;

            matchesList.Add(game);
        }

        string title = doc.DocumentNode.SelectNodes("//*[@id=\"content\"]/h1").First().InnerText;

        SendEmail(title, matchesList);
    }
}

