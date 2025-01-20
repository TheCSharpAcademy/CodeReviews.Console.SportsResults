using SportsResultNotifier.Models;
using SportsResultNotifier.Services;

var url = "https://www.basketball-reference.com/boxscores/";

IScraperService scraper = new ScraperService(url);

var date = DateTime.Now.ToLongDateString();
var title = $"NBA Games Played on {date}";

var results = scraper.GetResults();

string messageBody = "";
foreach (var result in results)
{
    messageBody += $"{result.HomeTeam.Name} {result.HomeTeam.Score} : {result.AwayTeam.Score} {result.AwayTeam.Name}\n\n";
}

var message = new Message
{
    Title = title,
    MessageBody = messageBody
};

var userName = "";
var recipient = "";
var password = "";

IMailerService mailer = new MailerService(userName, password);

mailer.SendEmail(message, recipient);