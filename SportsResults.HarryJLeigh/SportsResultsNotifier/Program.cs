using SportsResultsNotifier.Services;
using SportsResultsNotifier.Models;

string url = "https://www.basketball-reference.com/boxscores/";
EmailService email = new EmailService();
WebScraperService webScraper = new WebScraperService();

List<GameResult> results = webScraper.GetGameResults(url);
email.Send(results);