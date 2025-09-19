namespace Luizrodd.SportsResult.Models;

public class Team
{
    public Team(string name, int points)
    {
        Name = name;
        Points = points;
    }
    public string Name { get; set; }
    public int Points { get; set; }
}
