// See https://aka.ms/new-console-template for more information

using SportsResults;

Console.WriteLine("Hello, World!");
Scrapper scrapper = new Scrapper("https://www.basketball-reference.com/boxscores/");
 var matches =scrapper.ScrapeMatches();
 var mail= new EmailSender();
 mail.AddReceiver("pkrzysiek24@gmail.com");
 mail.Send("Test","Test works yessssssssssssssss");
 