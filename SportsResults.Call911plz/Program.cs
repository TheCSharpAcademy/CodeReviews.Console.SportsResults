namespace SportsResults.Call911plz;

class Program
{
    static void Main(string[] args)
    {
        List<GameSummary> summaries = Scraper.GetSummaries(@"https://www.basketball-reference.com/boxscores/?month=04&day=19&year=2025");
        foreach(var summary in summaries)
        {
            Console.WriteLine($"Winner: {summary.Winner.Name}");
            Console.Write($"Score: ");
            foreach (int score in summary.Winner.Scores)
            {
                Console.Write($"\t{score}");
            }
            Console.WriteLine();


            Console.WriteLine($"Loser: {summary.Loser.Name}");
            Console.Write($"Score: ");
            foreach (int score in summary.Loser.Scores)
            {
                Console.Write($"\t{score}");
            }
            Console.WriteLine();
        }
    }
}
