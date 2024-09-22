using HtmlAgilityPack;
using SportsResults.Models;
using SportsResults.Utilities;

namespace SportsResults.Services;
public class ScraperService
{
    private readonly HtmlParser _htmlParser;
    private readonly ConfigReader _configReader;

    public ScraperService(HtmlParser htmlParser, ConfigReader configReader)
    {
        _htmlParser = htmlParser;
        _configReader = configReader;
    }

    public async Task<List<SportData>> ScrapeSportsDataAsync()
    {
        var url = _configReader.GetWebsiteUrl();
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(url);

        return _htmlParser.ParseSportData(doc);
    }
}