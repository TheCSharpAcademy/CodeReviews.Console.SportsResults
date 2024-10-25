using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using SportsResultsNotifier.Arashi256.Classes;
using SportsResultsNotifier.Arashi256.Config;
using SportsResultsNotifier.Arashi256.Controllers;
using SportsResultsNotifier.Arashi256.Models;

namespace SportsResultsNotifier.Arashi256.Services
{
    internal class WebScraperService
    {
        private readonly AppManager _appManager;
        private readonly ILogger<SportsResultsController> _logger;

        public WebScraperService(AppManager appManager, ILogger<SportsResultsController> logger)
        {
            _appManager = appManager;
            _logger = logger;
        }

        public async Task<ServiceResponse> ScrapeDataAsync()
        {
            List<TeamGame> games = new List<TeamGame>();
            // Get website data.
            var htmlData = new HtmlWeb();
            var doc = await htmlData.LoadFromWebAsync(_appManager.GetApiUrl());
            var gameSummaryNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'game_summary')]");
            foreach (var gameSummaryNode in gameSummaryNodes)
            {
                // Extract winner data
                var winnerNode = gameSummaryNode.SelectSingleNode(".//tr[contains(@class, 'winner')]");
                var winnerTeam = winnerNode.SelectSingleNode(".//td/a").InnerText;
                var winnerScore = int.Parse(winnerNode.SelectSingleNode(".//td[@class='right']").InnerText.Trim());
                // Extract loser data
                var loserNode = gameSummaryNode.SelectSingleNode(".//tr[contains(@class, 'loser')]");
                var loserTeam = loserNode.SelectSingleNode(".//td/a").InnerText;
                var loserScore = int.Parse(loserNode.SelectSingleNode(".//td[@class='right']").InnerText.Trim());
                // Create model for this data.
                var game = new TeamGame
                {
                    WinnerTeam = winnerTeam,
                    WinnerScore = winnerScore,
                    LoserTeam = loserTeam,
                    LoserScore = loserScore
                };
                _logger.LogInformation($"Game data: '{game}'");
                // Add to list
                games.Add(game);
            }
            return ServiceResponseUtils.CreateResponse(ResponseStatus.Success, "OK", games);
        }
    }
}