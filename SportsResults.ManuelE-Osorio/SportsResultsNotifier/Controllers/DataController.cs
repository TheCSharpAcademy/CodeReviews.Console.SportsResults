using SportsResultsNotifier.Models;
using SportsResultsNotifier.Services;
using SportsResultsNotifier.UI;

namespace SportsResultsNotifier.Controllers;

public class DataController(EmailBuilderService emailBuilderService, WebCrawlerService webCrawlerService, AppVars appVars)
{
    public DateOnly? LastScrappedDate = appVars.LastScrappedDate;
    public DateOnly? LastRunDate = appVars.LastRunDate;
    private readonly AppVars AppVarsInstance = appVars;
    private readonly EmailBuilderService EmailServiceInstance = emailBuilderService;
    private readonly WebCrawlerService WebCrawlerInstance = webCrawlerService;

    public async Task<bool> Start()
    {
        MainUI.WelcomeMessage();
        if(LastRunDate >= DateOnly.FromDateTime(DateTime.Today))
        {
            MainUI.ErrorMessage("The app already ran today.");
            return false;
        }
        
        if(!await TestConnectionAsync())
        {
            return false;
        }

        var dateTask = GetWebScrappedDateAsync();
        var sportResultsTask = GetSportResultsAsync();
        var date = await dateTask;
        if(date == null)
        {
            return false;
        }
        else if(LastScrappedDate >= date)
        {
            MainUI.ErrorMessage("The page doesn't have new information to scrap.");
            return false;
        }

        var saveTask = Task.Run(() => SaveSettings((DateOnly) date));
        var sportResults = await sportResultsTask;
        if(sportResults == null)
        {
            return false;
        }

        var emailSenderTask = SendEmail((DateOnly) date, sportResults);
        await saveTask;        
        var emailStatus = await emailSenderTask;
        if(emailStatus)
        {
            MainUI.InformationMessage("Email sent succesfully");
            return false;
        }
        else
        {
            MainUI.ErrorMessage("Cannot send the email");
            return false;
        }
    }

    public async Task<DateOnly?> GetWebScrappedDateAsync()
    {
        MainUI.InformationMessage("Getting the last scrapped date.");
        var dateTask = WebCrawlerInstance.GetWebScrappedDateAsync();
        try
        {
            var date = await dateTask;
            MainUI.InformationMessage("Date scrapped succesfully");
            return date;
        }
        catch(Exception e)
        {
            MainUI.ErrorMessage(e.Message);
            return null;
        }
    } 

    public async Task<bool> TestConnectionAsync()
    {
        MainUI.InformationMessage("Testing connection to the web page");
        var status = await WebCrawlerInstance.TestConnectionAsync();
        
        if(!status)
        {
            MainUI.ErrorMessage("The app cannot connect to the web page");
            return false;
        }
        MainUI.InformationMessage("Connection to the web page succesful");
        return true;
    }

    public async Task<List<SportResults>?> GetSportResultsAsync()
    {
        MainUI.InformationMessage("Getting the latest scores.");
        var sportResults= await WebCrawlerInstance.GetGameScoresAsync();
        if(sportResults == null || sportResults.Count <= 0)
        {
            MainUI.ErrorMessage("Cannot obtain game scores information from web page.");
            return null;
        }
        else
            MainUI.InformationMessage("Scores obtained succesfully");
            return sportResults;
    }

    public async Task<bool> SendEmail(DateOnly date, List<SportResults> sportResults)
    {
        var emailSenderTask = EmailServiceInstance.SendEmail(date, sportResults);
        MainUI.InformationMessage("Sending email");
        try
        {
            var status = await emailSenderTask;
            return status;
        }
        catch (Exception e)
        {
            MainUI.ErrorMessage(e.Message);
            return false;
        }
    }

    public bool SaveSettings(DateOnly newScrappedDate)
    {
        MainUI.InformationMessage("Saving data to appsettings.json");
        try
        {
            Helpers.DataSaving.UpdateAppSetting(newScrappedDate, DateOnly.FromDateTime(DateTime.Today), 
                AppVarsInstance);
            MainUI.InformationMessage("appsettings updated succesfully");
            return true;
        }
        catch(Exception e)
        {
            MainUI.ErrorMessage(e.Message);
            return false;
        }
    }
}