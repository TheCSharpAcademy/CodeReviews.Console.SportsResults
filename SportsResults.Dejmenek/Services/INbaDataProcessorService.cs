using SportsResults.Dejmenek.Models;

namespace SportsResults.Dejmenek.Services;
public interface INbaDataProcessorService
{
    string PrepareEmailBody(List<Game> games, List<TeamStanding> easternConference, List<TeamStanding> westernConference);
}
