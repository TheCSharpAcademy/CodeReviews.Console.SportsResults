using SportsResultsNotifier.BBualdo.Models;
using SportsResultsNotifier.BBualdo.Services;

namespace SportsResultsNotifier.BBualdo;

internal class AppEngine
{
  private readonly MatchScraperService _matchScraperService;
  private readonly EmailService _emailService;

  internal AppEngine(MatchScraperService matchScraperService, EmailService emailService)
  {
    _matchScraperService = matchScraperService;
    _emailService = emailService;
  }

  internal async Task SendEmail()
  {
    List<Match> matches = _matchScraperService.GetMatches("https://www.basketball-reference.com/boxscores/");
    string subject = $"NBA Result for date: {DateTime.Now:dd-MM-yyyy}";
    string message = "";
    string receiver = System.Configuration.ConfigurationManager.AppSettings.Get("Receiver")!;

    if (matches.Count == 0)
    {
      Console.WriteLine("There is no matches today. Come back later.");
      await Task.CompletedTask;
    }

    foreach (Match match in matches)
    {
      message += match.GetResults();
    }

    await _emailService.SendEmailAsync(receiver, subject, message);
  }
}