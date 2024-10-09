using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace SportsResultsNotifier.Services;

public class HtmlScraperService
{
    private readonly string _url = "https://www.basketball-reference.com/boxscores/";
    private readonly ILogger<HtmlScraperService> _logger;

    public HtmlScraperService(ILogger<HtmlScraperService> logger)
    {
        _logger = logger;
    }

    public async Task<HtmlDocument> FetchHtml(CancellationToken stoppingToken)
    {
        int maxRetries = 3;
        int delay = 2000;

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {   
                try
                {
                    var web = new HtmlWeb();
                    var doc = await web.LoadFromWebAsync(_url, stoppingToken);
                    return doc;
                }
                catch
                {
                    if (attempt == maxRetries) { throw; }

                    _logger.LogError("Failed to scrape HTML. Attempt {attempt} of {maxRetries}.", attempt, maxRetries);
                    await Task.Delay(delay, stoppingToken);
                }
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HttpRequestException: Unable to fetch URL {url}.", _url);
                throw;
            }
            catch (TaskCanceledException canceledEx)
            {
                _logger.LogError(canceledEx, "TaskCanceledException: Request to {url} timed out.", _url);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                throw;
            }
        }

        throw new NotSupportedException();
    }

    public List<List<string>> ParseHtml(HtmlDocument doc)
    {
        var tableEast = doc.DocumentNode.SelectNodes("//*[@id=\"confs_standings_E\"]/tbody/tr");
        var tableWest = doc.DocumentNode.SelectNodes("//*[@id=\"confs_standings_W\"]/tbody/tr");

        var listEast = ParseTableToStringList(tableEast);
        var listWest = ParseTableToStringList(tableWest);

        return [listEast, listWest];
    }

    public List<string> ParseTableToStringList(HtmlNodeCollection table)
    {
        var list = new List<string>();

        foreach (var row in table)
        {
            var teamName = row.SelectSingleNode("th[1]").InnerText;
            var wins = row.SelectSingleNode("td[1]").InnerText;
            var losses = row.SelectSingleNode("td[2]").InnerText;

            list.Add($"Team: {teamName} Wins: {wins} Losses: {losses}");
        }

        return list;
    }

    public async Task<List<List<string>>> ExecuteScrapeAsync(CancellationToken stoppingToken)
    {
        var doc = await FetchHtml(stoppingToken);

        return ParseHtml(doc);
    }
}