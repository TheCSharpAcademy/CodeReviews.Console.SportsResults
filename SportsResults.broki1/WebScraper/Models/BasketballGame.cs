using System.ComponentModel.DataAnnotations;

namespace WebScraper.Models;

public class BasketballGame
{
    [Key]
    public int GameId { get; set; }
    public DateTime Date {  get; set; }
    public string HomeTeam { get; set; } = null!;
    public string AwayTeam { get; set; } = null!;
    public int HomeTeamScore { get; set; }
    public int AwayTeamScore { get; set; }
}
