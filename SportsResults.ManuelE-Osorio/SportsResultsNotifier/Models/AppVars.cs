namespace SportsResultsNotifier.Models;

public class AppVars
{
    public DateOnly? LastScrappedDate {get; set;} = null;
    public DateOnly? LastRunDate {get; set;} = null;
    public string? SourceEmail {get; set;}
    public string? SourceEmailPassword {get; set;}
    public string? Host {get; set;} = "smtp-mail.outlook.com";
    public int Port {get; set;} = 587;
    public string? DestinationEmail {get; set;}
    public string? WebPage {get; set;} = "https://www.basketball-reference.com/boxscores/";
}