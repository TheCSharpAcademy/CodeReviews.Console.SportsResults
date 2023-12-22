using HtmlAgilityPack;
using SportsResults.Speedierone;

string url = "https://www.basketball-reference.com/boxscores/";

HtmlWeb web = new HtmlWeb();
HtmlDocument doc = web.Load(url);

Scraper scraper = new Scraper();
List<Results> results = scraper.GetResults(doc);

string formattedResult = Helpers.GenerateBody(results);

SendEmail.SendResultEmail(formattedResult);

Console.WriteLine("Email sent. Press any key to end program.");
Console.ReadLine();
