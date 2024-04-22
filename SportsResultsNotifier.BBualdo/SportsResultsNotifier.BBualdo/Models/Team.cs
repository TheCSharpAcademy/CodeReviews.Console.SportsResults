namespace SportsResultsNotifier.BBualdo.Models;

internal class Team
{
  public string Name { get; set; }
  public string Score { get; set; }

  internal Team(string name, string score)
  {
    Name = name;
    Score = score;
  }
}