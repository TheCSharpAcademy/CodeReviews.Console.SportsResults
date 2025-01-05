namespace WebScraperLib.Models;
public class Game
{
    private Team winner;
    private Team loser;
    public int[] TeamLabels = new int[8];

    public Game(Team winner, Team loser, int[] labels)
    {
        this.winner = winner;
        this.loser = loser;
        this.TeamLabels = labels;
    }
    public Game() { }
    
    public string GetWinner()
        => this.winner.TeamName;

    public string GetLoser()
        => this.loser.TeamName;

    public int GetWinnerScore()
        => this.winner.Score;

    public int GetLoserScore()
        => this.loser.Score;
}
public record Team(string TeamName, int Score);