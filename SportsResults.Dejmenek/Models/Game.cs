namespace SportsResults.Dejmenek.Models;
public class Game
{
    public Team HomeTeam { get; set; }
    public Team AwayTeam { get; set; }
    public string? HomeTeamScore { get; set; }
    public string? AwayTeamScore { get; set; }

    public Game(Team homeTeam, Team awayTeam, string? homeTeamScore, string? awayTeamScore)
    {
        HomeTeam = homeTeam;
        AwayTeam = awayTeam;
        HomeTeamScore = homeTeamScore;
        AwayTeamScore = awayTeamScore;
    }
}
