using SportsResultsNotifier.Models;

namespace SportsResultsNotifier.Services;

public class EmailBuilderService
{
    private string? SourceEmail;
    private string? SourceEmailPassword;
    private string? DestinationEmail;

    public EmailBuilderService(AppVars appVars)
    {
        SourceEmail = appVars.SourceEmail;
        SourceEmailPassword = appVars.SourceEmailPassword;
        DestinationEmail = appVars.DestinationEmail;

        //Fix this
    }

    public static void SendEmail()
    {

    }

    public static void BuildEmailBody()
    {

    }
}
