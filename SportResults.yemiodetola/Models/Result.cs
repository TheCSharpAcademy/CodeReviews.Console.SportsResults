namespace SportResults.Models;

public class Result
{
  public int WinnerScore { get; set; }
  public int LoserScore { get; set; }
  public string? Winner { get; set; }
  public string? Loser { get; set; }
}