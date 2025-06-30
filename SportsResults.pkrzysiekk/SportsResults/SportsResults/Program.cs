// See https://aka.ms/new-console-template for more information

using SportsResults;

Console.WriteLine("Hello, World!");
Scrapper scrapper = new Scrapper("https://www.basketball-reference.com/boxscores/");
 var matches =scrapper.ScrapeMatches();
 foreach (var match in matches)
 {
     Console.WriteLine(match.Team1);
     Console.WriteLine(match.Team2);
 }