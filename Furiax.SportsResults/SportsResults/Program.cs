using SportsResults;

var results = Scrape.GetGame("https://www.basketball-reference.com/boxscores/");

string date = results.Date;
List<GameModel> games = results.Games;

Mail.SendMail(date, games);

