// See https://aka.ms/new-console-template for more information

using SportsResults;

Console.WriteLine("Hello, World!");
Scrapper scrapper = new Scrapper("https://www.basketball-reference.com/boxscores/");
scrapper.ScrapeMatches();