using SportsResultsNotifier.Models;
using HtmlAgilityPack;
using System.Net;
using SportsResultsNotifier.Validation;

namespace SportsResultsNotifier.Services;

public class WebCrawlerService
{
    public const string WebPageDateFormat = "MMMM d, yyyy";

    private readonly Uri? WebPage;
    private readonly HttpClient Client;

    public WebCrawlerService(AppVars appVars)
    {

        if(!DataValidation.UriValidation(appVars.WebPage, out WebPage))
            throw new Exception("Invalid Web Page in appsetting.json");
        Client = new()
        {
            BaseAddress = WebPage,
            Timeout = new TimeSpan(0, 0, 10)
        };
    }
    public async Task<bool> TestConnectionAsync()
    {      
        HttpRequestMessage httpRequestMessage = new()
        {
            Method = HttpMethod.Head,
        };

        HttpResponseMessage response;
        try
        {
            response = await Client.SendAsync(httpRequestMessage);
            if(response.StatusCode == HttpStatusCode.OK)
                return true;
        }
        catch
        {
            return false;
        }
        return false;
    }

    public async Task<DateOnly> GetWebScrappedDateAsync()
    {
        HtmlWeb web = new();
        var htmlDoc = await web.LoadFromWebAsync(WebPage,default, default);

        var title = htmlDoc.DocumentNode.SelectSingleNode("//body/div[@id='wrap']/div[@id='content']/h1").InnerText;

        if(DataValidation.DateValidation(title.Replace("NBA Games Played on ", ""), 
            WebPageDateFormat,out DateOnly scrappedDate))
            return scrappedDate;
        else
            throw new Exception("Web page error, cannot load content");
    }

    public async Task<List<SportResults>> GetGameScoresAsync()
    {
        HtmlWeb web = new();
        var htmlDoc = await web.LoadFromWebAsync(WebPage, default, default);
        
        return await GetGameScoresFromSummaryAsync(htmlDoc.DocumentNode.SelectNodes("//body/div[@id='wrap']/div[@id='content']"+
            "/div[@class='game_summaries']/div[@class='game_summary expanded nohover ']"));     
    }

    private static async Task<List<SportResults>> GetGameScoresFromSummaryAsync(HtmlNodeCollection gameList)
    {
        List<Task<SportResults>> resultsTasks = [];

        foreach (HtmlNode game in gameList)
        {
            resultsTasks.Add(Task.Run(() => CreateResults(game)));   
        };
        var results = await Task.WhenAll(resultsTasks);
        return [.. results];
    }

    private static SportResults CreateResults(HtmlNode game)
    {
        var loserResults = game.SelectSingleNode("table[@class='teams']/tbody/tr[@class='loser']");
        var winnerResults = game.SelectSingleNode("table[@class='teams']/tbody/tr[@class='winner']");
        SportResults result = new()
        {
            LoserTeamName = loserResults?.ChildNodes[1].InnerText,
            LoserTeamScore = loserResults?.ChildNodes[3].InnerText,
            WinnerTeamName = winnerResults?.ChildNodes[1].InnerText,
            WinnerTeamScore = winnerResults?.ChildNodes[3].InnerText
        };

        return result;
    }
}