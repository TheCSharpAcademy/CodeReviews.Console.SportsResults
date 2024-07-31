namespace SportsResults.Configurations;

/// <summary>
/// Class to hold the required application options for the Mail project.
/// </summary>
public class MailOptions
{
    #region Properties

    public bool SmtpClientEnableSsl { get; set; }

    public string SmtpClientHost { get; set; }

    public int SmtpClientPort { get; set; }

    public string ToEmailAddresses { get; set; }

    public string UserEmailAddress { get; set; }

    public string UserPassword { get; set; }

    #endregion
}
