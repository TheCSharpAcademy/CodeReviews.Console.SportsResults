namespace SportsResults.StanimalTheMan;

class Program
{
    static void Main(string[] args)
    {
        var gamesData = Scraper.ScrapeGames("https://www.basketball-reference.com/boxscores/");
        EmailClient.SendEmail(gamesData);
    }
}