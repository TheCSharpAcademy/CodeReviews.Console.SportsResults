using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System;
using static System.Net.WebRequestMethods;

namespace SportsResultsNotifier.Services;

public class HtmlScraperService
{
    private readonly string _url = "https://www.basketball-reference.com/boxscores/";
    private readonly ILogger<HtmlScraperService> _logger;

    public HtmlScraperService(ILogger<HtmlScraperService> logger)
    {
        _logger = logger;
    }

    public async Task<HtmlDocument?> FetchHtml(CancellationToken stoppingToken)
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
                catch (Exception ex)
                {
                    if (attempt == maxRetries) throw;

                    _logger.LogError(ex, "Failed to scrape HTML. Attempt {attempt} of {maxRetries}.", attempt, maxRetries);
                    await Task.Delay(delay, stoppingToken);
                }
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HttpRequestException: Unable to fetch URL {url}. Reason: {httpEx.Message}", 
                    _url, httpEx.Message);
                break;
            }
            catch (TaskCanceledException canceledEx)
            {
                _logger.LogError(canceledEx, "TaskCanceledException: Request to {url} timed out. Reason: {message}", 
                    _url, canceledEx.Message);
                break;
            }
            catch (HtmlWebException htmlWebEx)
            {
                _logger.LogError(htmlWebEx, "HtmlWebException: Error loading HTML from {url}. Reason: {message}", 
                    _url, htmlWebEx.Message);
                break;
            }
            catch (InvalidOperationException invalidOpEx)
            {
                _logger.LogError(invalidOpEx, "InvalidOperationException: Invalid operation while processing {url}. Reason: {message}",
                    _url, invalidOpEx.Message);
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An unexpected error occurred while fetching URL {url}. Reason: {ex.Message}",
                    _url, ex.Message);
                break;
            }
        }

        return null;
    }

    public async Task ExecuteScrapeAsync(CancellationToken stoppingToken)
    {
        var doc = await FetchHtml(stoppingToken);
    }
}