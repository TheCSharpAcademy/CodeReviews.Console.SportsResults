namespace SportsResultsNotifier.BBualdo.Models;

internal class Match
{
  public Team Winner { get; set; }
  public Team Loser { get; set; }

  internal Match(Team winner, Team loser)
  {
    Winner = winner;
    Loser = loser;
  }

  public string GetResults()
  {
    string results = $"\n{Winner.Name} won in match against {Loser.Name}. Result: {Winner.Score}:{Loser.Score}\n";
    return results;
  }
}