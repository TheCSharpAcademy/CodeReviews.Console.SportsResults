// See https://aka.ms/new-console-template for more information

using SportsResults;
using SportsResults.Controllers;

var mail = new EmailSender();
mail.AddReceiver("pkrzysiek24@gmail.com");
string url = "https://www.basketball-reference.com/boxscores/";
AppController ctr = new AppController(url, mail);
ctr.StartController();
Thread.Sleep(Timeout.Infinite);