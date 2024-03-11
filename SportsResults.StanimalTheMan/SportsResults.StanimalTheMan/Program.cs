namespace SportsResults.StanimalTheMan;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
        Scraper.ScrapeGames("https://www.basketball-reference.com/boxscores/");
    }
}