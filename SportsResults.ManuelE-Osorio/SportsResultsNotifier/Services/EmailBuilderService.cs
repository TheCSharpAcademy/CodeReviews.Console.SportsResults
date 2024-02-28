using System.Net.Mail;
using SportsResultsNotifier.Models;
using SportsResultsNotifier.Validation;

namespace SportsResultsNotifier.Services;

public class EmailBuilderService
{
    private MailAddress? SourceEmail;
    private string? SourceEmailPassword;
    private MailAddress? DestinationEmail;

    public EmailBuilderService(AppVars appVars)
    {
        if(DataValidation.EmailValidation(appVars.SourceEmail, out SourceEmail))
            SourceEmailPassword = appVars.SourceEmailPassword;
        else
            throw new Exception($"The email {appVars.SourceEmail} from appsettings.json is invalid.");

        if(!DataValidation.EmailValidation(appVars.DestinationEmail, out DestinationEmail))
            throw new Exception($"The email {appVars.DestinationEmail} from appsettings.json is invalid.");
    }

    public static void SendEmail()
    {

    }

    public static void BuildEmailBody()
    {

    }
}
