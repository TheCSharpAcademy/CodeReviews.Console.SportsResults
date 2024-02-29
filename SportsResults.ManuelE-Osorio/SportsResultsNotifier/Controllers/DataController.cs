using System.ComponentModel;
using SportsResultsNotifier.Models;
using SportsResultsNotifier.Services;
using SportsResultsNotifier.UI;

namespace SportsResultsNotifier.Controllers;

public class DataController
{
    public DateOnly? LastScrappedDate;
    public DateOnly? LastRunDate;
    private readonly EmailBuilderService EmailServiceInstance;
    private readonly WebCrawlerService WebCrawlerInstance;

    public DataController(EmailBuilderService emailBuilderService, WebCrawlerService webCrawlerService, AppVars appVars)
    {
        LastScrappedDate = appVars.LastScrappedDate;
        LastRunDate = appVars.LastRunDate;
        EmailServiceInstance = emailBuilderService;
        WebCrawlerInstance = webCrawlerService;
    }

    public async Task<bool> Start()
    {
        if(LastRunDate >= DateOnly.FromDateTime(DateTime.Today))
        {
            MainUI.InformationMessage("The app has already scrapped the data for today.");
            MainUI.ExitMessage();
            return false;
        }

        var testConnectionTask = TestConnectionAsync();
        var dateTask = GetWebScrappedDateAsync();
        var sportResultsTask = GetSportResultsAsync();
        
        if(!await testConnectionTask)
        {
            MainUI.ExitMessage();
            return false;
        }

        var date = await dateTask;
        if(date == null)
        {
            MainUI.ExitMessage();
            return false;
        }
        else if(LastScrappedDate >= date)
        {
            MainUI.InformationMessage("The page doesn't have new information to scrap.");
            MainUI.ExitMessage();
            return false;
        }

        var saveTask = Task.Run(() => SaveSettings((DateOnly) date));
        var sportResults = await sportResultsTask;
        if(sportResults == null || sportResults.Count == 0)
        {
            MainUI.ExitMessage();
            return false;
        }

        await EmailServiceInstance.SendEmail((DateOnly) date, await sportResultsTask);
        return true;
    }

    public async Task<DateOnly?> GetWebScrappedDateAsync()
    {
        MainUI.InformationMessage("Getting the last scrapped date.");
        var dateTask = WebCrawlerInstance.GetWebScrappedDateAsync();
        try
        {
            return await dateTask;
        }
        catch(Exception e)
        {
            MainUI.InformationMessage(e.Message);
            return null;
        }
    } 
    public async Task<bool> TestConnectionAsync()
    {
        MainUI.InformationMessage("Testing connection to the web page");
        var status = await WebCrawlerInstance.TestConnectionAsync();
        
        if(!status)
        {
            MainUI.InformationMessage("The app cannot connect to the web page");
            return false;
        }
        return true;
    }

    public async Task<List<SportResults>> GetSportResultsAsync()
    {
        var sportResultsTask = WebCrawlerInstance.GetGameScoresAsync();


        

    }

    public static bool SaveSettings(DateOnly newScrappedDate)
    {
        try
        {
            Helpers.DataSaving.UpdateAppSetting(newScrappedDate, DateOnly.FromDateTime(DateTime.Today));
            return true;
        }
        catch(Exception e)
        {
            MainUI.InformationMessage(e.Message);
            return false;
        }
    }
}