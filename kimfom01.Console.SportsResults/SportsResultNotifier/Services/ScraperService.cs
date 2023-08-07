using HtmlAgilityPack;
using SportsResultNotifier.Models;

namespace SportsResultNotifier.Services;

public class ScraperService : IScraperService
{
    private readonly string _url;

    public ScraperService(string url)
    {
        _url = url;
    }

    private HtmlDocument GetDocument(string url)
    {
        var web = new HtmlWeb();

        HtmlDocument document = web.Load(url);

        return document;
    }

    private List<string> GetLinks()
    {
        var linksList = new List<string>();

        var doc = GetDocument(_url);

        var linkNodes = doc.DocumentNode.SelectNodes("//p[@class = 'links']/a[1]");

        foreach (var node in linkNodes)
        {
            var baseUrl = new Uri(_url);

            var url = new Uri(baseUrl, node.Attributes["href"].Value);

            linksList.Add(url.AbsoluteUri);
        }

        return linksList;
    }

    public List<Result> GetResults()
    {
        var linksList = GetLinks();

        var results = new List<Result>();

        foreach (var link in linksList)
        {
            var doc = GetDocument(link);

            var namesNode = doc.DocumentNode.SelectNodes("//strong/a");
            List<string> names = GetTeamsNames(namesNode);

            var scoresNode = doc.DocumentNode.SelectNodes("//div[@class = 'score']");
            List<string> scores = GetTeamsScores(scoresNode);

            var result = CreateResult(names, scores);

            results.Add(result);
        }

        return results;
    }

    private List<string> GetTeamsScores(HtmlNodeCollection scoresNode)
    {
        var scores = new List<string>();
        foreach (var node in scoresNode)
        {
            scores.Add(node.InnerText);
        }

        return scores;
    }

    private List<string> GetTeamsNames(HtmlNodeCollection namesNode)
    {
        var names = new List<string>();
        foreach (var node in namesNode)
        {
            names.Add(node.InnerText);
        }

        return names;
    }

    private Result CreateResult(List<string> names, List<string> scores)
    {
        var teamA = new Team
        {
            Name = names[0],
            Score = scores[0]
        };

        var teamB = new Team
        {
            Name = names[1],
            Score = scores[1]
        };

        var result = new Result
        {
            HomeTeam = teamA,
            AwayTeam = teamB
        };

        return result;
    }
}
