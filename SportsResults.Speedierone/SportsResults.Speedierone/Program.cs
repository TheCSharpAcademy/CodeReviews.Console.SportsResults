using HtmlAgilityPack;
using SportsResults.Speedierone;
/*HtmlWeb web = new HtmlWeb();
HtmlDocument document = web.Load("https://www.basketball-reference.com/boxscores/");

var title = document.DocumentNode.SelectNodes("//div/h1").First().InnerText;
var winner = document.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[3]/div[1]/table[1]/tbody/tr[2]/td[1]").First().InnerText;
var winnerScore = document.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[3]/div[1]/table[1]/tbody/tr[2]/td[2]").First().InnerText;
var loser = document.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[3]/div[1]/table[1]/tbody/tr[1]/td[1]").First().InnerText;
var loserScore = document.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[3]/div[2]").First().InnerText;

//Console.WriteLine(title);
//Console.WriteLine($"{winner} = {winnerScore}");
//Console.WriteLine($"{loser} = {loserScore}");
//*[@id="content"]/div[3]/div[2]/table[1]/tbody/tr[1]/td[1]

var table = document.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[3]/div/table[1]/tbody/tr[2]/td[1]");
var tableScore = document.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[3]/div/table[1]/tbody/tr[2]/td[1]");

//table.ToList().ForEach(x => Console.WriteLine(x.InnerText));
var tableWinners = table.ToList();
var tableScores = tableScore.ToList();

//*[@id="content"]/div[3]/div[1]/table[1]
//*[@id="content"]/div[3]/div[2]/table[1]*/

string url = "https://www.basketball-reference.com/boxscores/";

Scraper scraper = new Scraper();
List<Results> results = scraper.GetResults(url);

foreach (var result in results)
{
    Console.WriteLine($"Winning Team : {result.Team1} - {result.Score1}");
    //Console.WriteLine($"Losing Team : {result.LosingTeam} - {result.LoserScore}");
}
Console.ReadLine();
