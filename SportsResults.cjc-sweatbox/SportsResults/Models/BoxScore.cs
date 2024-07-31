namespace SportsResults.Models;

/// <summary>
/// DTO for holding a Team's Score in a Game.
/// </summary>
public class BoxScore
{
    #region Properties

    public string Name { get; set; } = "";

    public int Score { get; set; }

    #endregion
}
