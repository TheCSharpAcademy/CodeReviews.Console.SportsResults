using SportsResults.Models;
using SportsResults.Service;

namespace SportsResults;

internal static class Program
{
    static void Main(string[] args)
    {
        DateTime lastScrape;

        List<Game> games;

        try
        {
            games = WebScraper.Scrape();

            Email.Send(games);
        }
        catch (Exception e)
        {

            Console.WriteLine($"Something went wrong. Error: {e.Message}\n\n");
        }

        lastScrape = DateTime.Now;

        while (true)
        {
            TimeSpan timeSpan = DateTime.Now - lastScrape;

            if (timeSpan.TotalHours >= 24)
            {
                try
                {
                    games = WebScraper.Scrape();

                    Email.Send(games);
                }
                catch (Exception e)
                {

                    Console.WriteLine($"Something went wrong. Error: {e.Message}\n\n");
                }

                lastScrape = DateTime.Now;
            }

        }

    }
}

