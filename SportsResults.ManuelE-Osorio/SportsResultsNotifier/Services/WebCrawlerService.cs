using SportsResultsNotifier.Models;
using HtmlAgilityPack;
using System.Net;
using SportsResultsNotifier.Validation;

namespace SportsResultsNotifier.Services;

public class WebCrawlerService
{
    public const string WebPageDateFormat = "MMMM d, yyyy";
    public DateOnly? LastScrappedDate;
    public DateOnly? LastRunDate;
    private string? WebPage;
    private HttpClient Client;  //refactor

    public WebCrawlerService(AppVars appVars)
    {
        LastScrappedDate = appVars.LastScrappedDate;
        LastRunDate = appVars.LastRunDate;
        if(!DataValidation.UriValidation(appVars.WebPage, out Uri? webPageUri))
            throw new Exception("Invalid Web Page in appsetting.json");
        Client = new()
        {
            BaseAddress = webPageUri,
            Timeout = new TimeSpan(0, 0, 10)
        };
    }
    public async Task<bool> TestConnection()
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

    public async Task<DateOnly> GetWebScrappedDate()
    {
        HtmlWeb web = new();
        var htmlDoc = await web.LoadFromWebAsync(Client.BaseAddress, 
            System.Text.Encoding.Unicode, null);

        var title = htmlDoc.DocumentNode.SelectSingleNode("//body/div[@id='wrap']/div[@id='content']/h1").InnerText;

        if(DataValidation.DateValidation(title.Replace("NBA Games Played on ", ""), 
            WebPageDateFormat,out DateOnly scrappedDate))
            return scrappedDate;
        else
            throw new Exception("Web page error, cannot load content");
    }

    public async Task<List<SportResults>> GetGameScores()
    {
		var html = @"https://www.basketball-reference.com/boxscores/";

        HtmlWeb web = new();

        var htmlDoc = web.Load(html);
        return await GetGameScoresFromSummary(htmlDoc.DocumentNode.SelectNodes("//body/div[@id='wrap']/div[@id='content']"+
            "/div[@class='game_summaries']/div[@class='game_summary expanded nohover ']"));     
    }

    private static async Task<List<SportResults>> GetGameScoresFromSummary(HtmlNodeCollection gameList)
    {
        List<Task<SportResults>> resultsTasks = [];

        foreach (HtmlNode game in gameList)
        {
            resultsTasks.Add(CreateResults(game));   
        };
        var results = await Task.WhenAll(resultsTasks);
        return [.. results];
    }

    private static async Task<SportResults> CreateResults(HtmlNode game)
    {
        var loserResults = game.SelectSingleNode("//table[@class='teams']/tbody/tr[@class='loser']"); //doesnt look node
        var winnerResults = game.SelectSingleNode("//table[@class='teams']/tbody/tr[@class='winner']");
        SportResults result = new();
        
            result.LoserTeamName = loserResults?.ChildNodes[1].InnerText;
            result.LoserTeamScore = loserResults?.ChildNodes[3].InnerText;
            result.WinnerTeamName = winnerResults?.ChildNodes[1].InnerText;
            result.WinnerTeamScore = winnerResults?.ChildNodes[3].InnerText;
        
        return await Task.FromResult(result);
    }
}