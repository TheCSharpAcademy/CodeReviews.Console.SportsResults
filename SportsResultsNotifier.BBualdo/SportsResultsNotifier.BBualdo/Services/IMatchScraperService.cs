using SportsResultsNotifier.BBualdo.Models;

namespace SportsResultsNotifier.BBualdo.Services;

internal interface IMatchScraperService
{
  List<Match> GetMatches(string url);
}