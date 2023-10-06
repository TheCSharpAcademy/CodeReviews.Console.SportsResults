using Microsoft.Extensions.Options;
using SportsResults.Forser.Models;

namespace SportsResults.Forser.Services
{
    internal class Notifier
    {
        private readonly IEmailService _emailService;
        private readonly IScraper _scraper;
        private readonly IOptions<SettingsModel> _appSettings;

        public Notifier(IEmailService emailService, IScraper scraper, IOptions<SettingsModel> app) 
        {
            _emailService = emailService;
            _scraper = scraper;
            _appSettings = app;
        }
        public void GenerateNotification()
        {
            try
            {
                var page = _scraper.LoadWebsite(_appSettings.Value.DefaultUrl);

                if (page != null)
                {
                    string title = _scraper.GetDateOfResults(page);

                    var nodes = _scraper.GetAllNodes(page);
                    if(nodes.Count > 0)
                    {
                        List<GameModel> games = _scraper.GenerateGameModel(nodes);
                        if (games.Count > 0)
                        {
                            string email = _scraper.GenerateEmail(games, title);

                            _emailService.SendEmail(email);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }
    }
}