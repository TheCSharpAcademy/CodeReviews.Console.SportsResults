using SportsResults;

List<GameModel> games = new List<GameModel>();
games= Scrape.GetGame("https://www.basketball-reference.com/boxscores/");
//Mail.SendMail();

