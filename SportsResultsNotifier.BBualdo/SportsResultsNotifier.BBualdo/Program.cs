using SportsResultsNotifier.BBualdo;
using SportsResultsNotifier.BBualdo.Services;

AppEngine app = new(new MatchScraperService(), new EmailService());

await app.SendEmail();