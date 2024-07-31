namespace SportsResults.Configurations;

/// <summary>
/// Class to hold the required application options for the WorkerService project.
/// </summary>
public class WorkerServiceOptions
{
    #region Properties

    public int ScheduledHour { get; set; }

    public int ScheduledMinute { get; set; }

    public int ScheduledInvervalInHours { get; set; }

    #endregion
}
