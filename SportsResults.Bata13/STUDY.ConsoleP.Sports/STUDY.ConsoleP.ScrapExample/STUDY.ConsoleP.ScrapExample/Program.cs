using STUDY.ConsoleP.ScrapExample;

namespace STUDY.ConsoleP.ScrapExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var scraper = new DynamicWebScrapper();
            var url = "https://www.basketball-reference.com/boxscores/";

            var users = scraper.GetUsers(url);
            Console.WriteLine(users);
        }
    }
}