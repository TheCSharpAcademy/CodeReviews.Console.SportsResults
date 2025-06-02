namespace SportsResults.BrozDa
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string baseUrl = "https://www.basketball-reference.com/boxscores/?";


            var url = baseUrl + $"month=5&day={1}&year=2025";

            var service = new ScrapingService(baseUrl);

            var scrapeResult = service.GetGames("lol");
            Console.WriteLine(scrapeResult.ToString());

            /*for (int i = 1; i <= 20; i++)
            {
                var url = baseUrl + $"month=5&day={i}&year=2025";

                var scrapeResult = service.GetGames(url);
                Console.WriteLine(scrapeResult.ToString());
                Console.WriteLine( new string('#', 20));
            
            }*/

            

            

        }

    }
}
