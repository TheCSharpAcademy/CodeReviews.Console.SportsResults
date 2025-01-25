namespace SportsResults.Models;
internal class SportsResult
{
    public string Team1 {  get; set; }
    public string Team2 { get; set; }
    public int ScoreTeam1 { get; set; }
    public int ScoreTeam2 { get; set; }
    public string Result { get; set; }
    public string LinkToBoxScore { get; set; }
    public string LinkToPlayByPlay { get; set; }
    public string LinkToShotChart { get; set; }
}