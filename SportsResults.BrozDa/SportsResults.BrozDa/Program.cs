namespace SportsResults.BrozDa
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var service = new ScrapingService("https://www.basketball-reference.com/boxscores/?month=5&day=11&year=2025");

            service.GetTeams();

        }
    }
}
