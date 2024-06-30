namespace SportsResultsApp.kalsson.Models;

public class BasketballGameModel
{
    public string Team1 { get; set; }
    public string Team2 { get; set; }
    public int Team1Score { get; set; }
    public int Team2Score { get; set; }

    public override string ToString()
    {
        return $"{Team1} {Team1Score} - {Team2Score} {Team2}";
    }
}