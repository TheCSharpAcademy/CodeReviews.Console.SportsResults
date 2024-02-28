namespace SportsResultsNotifier.Models;

public class AppVars
{
    public DateOnly? LastScrappedDate {get; set;}
    public DateOnly? LastRunDate {get; set;}
    public string? SourceEmail {get; set;}
    public string? SourceEmailPassword {get; set;}
    public string? DestinationEmail {get; set;}
    public string? WebPage {get; set;}
}