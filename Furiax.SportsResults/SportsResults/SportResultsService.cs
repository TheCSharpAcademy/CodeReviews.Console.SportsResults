using Microsoft.Extensions.Hosting;

namespace SportsResults
{
	internal class SportResultsService : BackgroundService
	{
		private readonly Scrape _scrape;
		private readonly Mail _mail;
		public SportResultsService(Scrape scrape, Mail mail)
		{
			_scrape = scrape;
			_mail = mail;
		}

		protected override async Task ExecuteAsync(CancellationToken stopToken)
		{
			var now = DateTime.Now;
			var sendTime = new DateTime(now.Year, now.Month, now.Day, 9, 30, 0);
			if (now > sendTime)
			{
				sendTime = sendTime.AddDays(1);
			}
			var initialDelay = sendTime - now;

			await Task.Delay(initialDelay, stopToken);

			while (!stopToken.IsCancellationRequested)
			{
				try
				{
					var results = Scrape.GetGame("https://www.basketball-reference.com/boxscores/");

					string date = results.Date;
					List<GameModel> games = results.Games;

					Mail.SendMail(date, games);

					sendTime = sendTime.AddDays(1);
					var delay = sendTime - DateTime.Now;
					await Task.Delay(delay, stopToken);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"The following error occured: {ex.Message}");
				}
			}
		}
	}
}
