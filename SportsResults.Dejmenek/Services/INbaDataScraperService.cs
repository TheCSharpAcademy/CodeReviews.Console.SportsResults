using SportsResults.Dejmenek.Models;

namespace SportsResults.Dejmenek.Services;
public interface INbaDataScraperService
{
    List<Game> ScrapeGames();
    List<TeamStanding> ScrapeEasternConferenceStandings();
    List<TeamStanding> ScrapeWesternConferenceStandings();
}
