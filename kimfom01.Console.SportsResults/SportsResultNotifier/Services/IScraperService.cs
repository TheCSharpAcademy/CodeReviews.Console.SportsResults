using SportsResultNotifier.Models;

namespace SportsResultNotifier.Services;

public interface IScraperService
{
    public List<Result> GetResults();
}
