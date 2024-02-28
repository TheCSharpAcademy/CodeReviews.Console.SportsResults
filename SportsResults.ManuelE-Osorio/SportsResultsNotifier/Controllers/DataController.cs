using SportsResultsNotifier.Services;
using SportsResultsNotifier.UI;

namespace SportsResultsNotifier.Controllers;

public class DataController
{
    private EmailBuilderService EmailServiceInstance;
    private WebCrawlerService WebCrawlerInstance;

    public DataController(EmailBuilderService emailBuilderService, WebCrawlerService webCrawlerService)
    {
        EmailServiceInstance = emailBuilderService;
        WebCrawlerInstance = webCrawlerService;
    }

    public async Task<bool> Crawl()
    {
        var status = await WebCrawlerInstance.TestConnection();
        MainUI.InformationMessage(status.ToString());
        Thread.Sleep(2000);
        var list = await WebCrawlerInstance.GetGameScores();
        return true;
    }
}