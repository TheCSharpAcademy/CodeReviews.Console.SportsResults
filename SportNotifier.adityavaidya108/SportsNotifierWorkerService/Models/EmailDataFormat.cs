namespace SportsNotifierWorkerService.Models;

public class EmailDataFormat
{
    public string? TotalGames;
    public string? Date;
    public List<GameDataFormat>? games;
}