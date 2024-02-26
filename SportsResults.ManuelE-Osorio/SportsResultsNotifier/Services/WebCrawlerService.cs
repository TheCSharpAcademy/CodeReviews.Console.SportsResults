using SportsResultsNotifier.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;

namespace SportsResultsNotifier.Services;

public class WebCrawlerService(AppVars appVars)
{
    public const string WebPageDateFormat = "MMMM d, yyyy";
    private DateOnly LastScrappedDate = appVars.LastScrappedDate;
    private DateOnly LastRunDate = appVars.LastRunDate;
    public static void TestConnection()
    {
        throw new NotImplementedException();
    }

    public static void GetWebScrappedDate()
    {
        throw new NotImplementedException();
    }

    public static void GetGameSummaries()
    {
        throw new NotImplementedException();
    }

    public static void GetGameScores()
    {
        throw new NotImplementedException();
    }
}