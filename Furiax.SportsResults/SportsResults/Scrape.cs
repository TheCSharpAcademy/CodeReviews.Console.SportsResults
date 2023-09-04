using HtmlAgilityPack;

namespace SportsResults
{
	internal class Scrape
	{
		internal static List<GameModel> GetGame(string url)
		{
			List<GameModel> games = new();

			HtmlWeb web = new HtmlWeb();
			var doc = web.Load("https://www.basketball-reference.com/boxscores/?month=4&day=15&year=2023");

			var title = doc.DocumentNode.SelectNodes("//div/h2").First().InnerText;
			var date = doc.DocumentNode.SelectNodes("//title").First().InnerText;
			var gameNodes = doc.DocumentNode.SelectNodes("//div[@class='game_summary expanded nohover ']").ToList();


			if (gameNodes != null)
			{
				Console.WriteLine($"{date}");
				Console.WriteLine($"{title}");
				foreach (var gamenode in gameNodes)
				{
					GameModel game = new GameModel
					{
						HomeTeam = gamenode.SelectSingleNode(".//table/tbody/tr[1]/td[1]").InnerText,
						AwayTeam = gamenode.SelectSingleNode(".//table/tbody/tr[2]/td[1]").InnerText,
						HomeScore = gamenode.SelectSingleNode(".//table/tbody/tr[1]/td[2]").InnerText,
						AwayScore = gamenode.SelectSingleNode(".//table/tbody/tr[2]/td[2]").InnerText
					};
					games.Add(game);

					
					Console.WriteLine($"{game.HomeTeam} - {game.AwayTeam} : {game.HomeScore} - {game.AwayScore}");
				}
			}
			else
			{
				Console.WriteLine("No games were played today");
			}

			
			return games;
		}

	}
}
