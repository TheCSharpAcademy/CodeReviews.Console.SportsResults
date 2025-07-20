using SportResults.Services;
using SportResults.Models;
using SportResults.Mailer;
using SportResults.Helpers;


string url = "https://www.basketball-reference.com/boxscores/";

ScraperService scraperService = new ScraperService(url);
List<Result> results = scraperService.ScrapGameData();

string formattedResult = MailHelper.GenerateMailBody(results);

Mailer.SendEmail(formattedResult);

Console.WriteLine("Email sent.");
