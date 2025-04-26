namespace SportsResults.Call911plz;

class Program
{
    // Change these on use
    private readonly static string EMAIL = "htruong6219@gmail.com";
    private readonly static string SMTPGMAILPASSWORD = "";
    static void Main(string[] args)
    {
        EmailSender sender = new(EMAIL, SMTPGMAILPASSWORD);
        sender.SendToSelf("testing", "worked");

        // List<GameSummary> summaries = Scraper.GetSummaries(@"https://www.basketball-reference.com/boxscores/?month=04&day=19&year=2025");
        // foreach(var summary in summaries)
        // {
        //     Console.WriteLine($"Winner: {summary.Winner.Name}");
        //     Console.Write($"Score: ");
        //     foreach (int score in summary.Winner.Scores)
        //     {
        //         Console.Write($"\t{score}");
        //     }
        //     Console.WriteLine();


        //     Console.WriteLine($"Loser: {summary.Loser.Name}");
        //     Console.Write($"Score: ");
        //     foreach (int score in summary.Loser.Scores)
        //     {
        //         Console.Write($"\t{score}");
        //     }
        //     Console.WriteLine();
        // }
    }
}
