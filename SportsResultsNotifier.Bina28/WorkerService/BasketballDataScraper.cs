using HtmlAgilityPack;
using System.Text;
using WorkerService.Services;

namespace WorkerService;
public class BasketballDataScraper
{
	private readonly EmailService _emailService;

	public BasketballDataScraper(EmailService emailService)
	{
		_emailService = emailService;
	}

	public void ScrapeData()
	{
		try
		{
			HtmlWeb web = new HtmlWeb();
			HtmlDocument document = web.Load("https://www.basketball-reference.com/boxscores/");
			var headerNode = document.DocumentNode.SelectSingleNode("//*[@id='content']/h1");
			string headerText = headerNode != null ? headerNode.InnerText.Trim() : "Header not found";

			var tables = document.DocumentNode.SelectNodes("//table[contains(@class, 'team')]");
			StringBuilder tableData = new StringBuilder();
			tableData.AppendLine($"<h1>{headerText}</h1>");
			foreach (var table in tables)
			{
				var rows = table.SelectNodes(".//tr");
				if (rows != null && rows.Count == 2)
				{
					var team1Columns = rows[0].SelectNodes(".//td");
					var team2Columns = rows[1].SelectNodes(".//td");

					if (team1Columns != null && team1Columns.Count == 3 && team2Columns != null && team2Columns.Count == 3)
					{
						string team1Name = team1Columns[0].InnerText.Trim();
						string team1Score = team1Columns[1].InnerText.Trim();
						string extraInfo1 = team1Columns[2].InnerText.Trim();

						string team2Name = team2Columns[0].InnerText.Trim();
						string team2Score = team2Columns[1].InnerText.Trim();
						string extraInfo2 = team2Columns[2].InnerText.Trim();

						tableData.AppendLine($"<p>{team1Name}, {team1Score}, {extraInfo1}<br>{team2Name}, {team2Score}</p>");
					}
				}
			}

			string tableDataAsHtml = tableData.ToString().Trim();
			EmailService.SendEmail("Daily Basketball Data Tables Update", tableDataAsHtml);
		}
		catch (Exception ex)
		{
			File.AppendAllText("error.log", $"{DateTime.Now}: {ex.Message}\n{ex.StackTrace}\n");
		}
	}
}
