namespace Luizrodd.SportsResult.Models;

public class Game
{
    public Game(string teamOneName, string teamTwoName, int teamOnePoints, int teamTwoPoints)
    {
        TeamOne = new Team(teamOneName, teamOnePoints);
        TeamTwo = new Team(teamTwoName, teamTwoPoints);
        Winner = teamOnePoints > teamTwoPoints ? teamOneName : teamTwoName;
    }

    public string Winner { get; set; }
    public Team TeamOne { get; set; }
    public Team TeamTwo { get; set; }
}
