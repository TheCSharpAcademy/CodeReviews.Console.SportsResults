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
                    Console.WriteLine($"{page.DocumentNode.SelectSingleNode("//div[@class='index']/h1").InnerText}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }
    }
}