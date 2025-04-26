
public class GameSummary
{
    public Team Winner { get; set; }
    public Team Loser { get; set; }

    public GameSummary(Team winner, Team loser)
    {
        Winner = winner;
        Loser = loser;
    }
}