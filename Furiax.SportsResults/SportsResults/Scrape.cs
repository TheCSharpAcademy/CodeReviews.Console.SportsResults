using HtmlAgilityPack;

namespace SportsResults
{
	public class Scrape
	{
		internal static (string Date, List<GameModel> Games) GetGame(string url)
		{
			List<GameModel> games = new();
			string date = "";

			try
			{
				HtmlWeb web = new HtmlWeb();
				var doc = web.Load(url);

				var dateInfo = doc.DocumentNode.SelectNodes("//title").First().InnerText;
				if (string.IsNullOrWhiteSpace(dateInfo))
				{
					throw new InvalidOperationException("Could not retrieve the date from the page.");
				}
				string[] dateSplit = dateInfo.Split('|');
				date = dateSplit[0].Trim();

				var gameNodes = doc.DocumentNode.SelectNodes("//div[@class='game_summary expanded nohover ']").ToList();
				if (gameNodes != null)
				{
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
					}
				}
				else
				{
					throw new InvalidOperationException("Could not retrieve any games from the page");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred during scraping: {ex.Message}");
			}
			return (date, games);
		}
	}
}
