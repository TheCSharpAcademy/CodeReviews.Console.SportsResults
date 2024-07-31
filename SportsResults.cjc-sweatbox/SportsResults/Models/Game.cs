namespace SportsResults.Models;

/// <summary>
/// DTO for holding a Home and Away Team's BoxScore.
/// </summary>
public class Game
{
    #region Properties

    public BoxScore? Home {  get; set; }
    
    public BoxScore? Away { get; set; }

    #endregion
}
