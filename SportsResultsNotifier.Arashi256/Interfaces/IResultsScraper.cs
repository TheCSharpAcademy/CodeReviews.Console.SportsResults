using SportsResultsNotifier.Arashi256.Models;

namespace SportsResultsNotifier.Arashi256.Interfaces
{
    internal interface IResultsScraper
    {
        List<TeamGame> GetResults(string apiURL);
    }
}