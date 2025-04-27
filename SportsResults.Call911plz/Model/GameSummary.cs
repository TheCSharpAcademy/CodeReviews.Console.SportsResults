
public class GameSummary
{
    public DateTime Date { get; set; }
    public Team Winner { get; set; }
    public Team Loser { get; set; }

    public GameSummary(DateTime date, Team winner, Team loser)
    {
        Date = date;
        Winner = winner;
        Loser = loser;
    }
}