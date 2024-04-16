namespace SportsResults.Dejmenek.Models;
public class TeamStanding
{
    public Team Team { get; set; }
    public string? Wins { get; set; }
    public string? Losses { get; set; }
    public string? WinPercentage { get; set; }

    public TeamStanding(Team team, string? wins, string? losses, string? winPercentage)
    {
        Team = team;
        Wins = wins;
        Losses = losses;
        WinPercentage = winPercentage;
    }
}
