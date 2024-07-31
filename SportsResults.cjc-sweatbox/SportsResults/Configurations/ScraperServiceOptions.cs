namespace SportsResults.Configurations;

/// <summary>
/// Class to hold the required application options for the Scraper service.
/// </summary>
public class ScraperServiceOptions
{
    #region Properties

    public bool DateOverride { get; set; }

    public int DateOverrideDay { get; set; }

    public int DateOverrideMonth { get; set; }

    public int DateOverrideYear { get; set; }

    #endregion
}
