using SportsResults.BrozDa.Services;

namespace SportsResults.BrozDa
{
    /// <summary>
    /// Coordinates the scraping of sports results and sending of notifications via email.
    /// </summary>
    internal class SportNotifierController
    {
        public ScrapingService ScrapingService { get; }
        public EmailService EmailService { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SportNotifierController"/> class.
        /// </summary>
        /// <param name="scrapingService">An instance of <see cref="ScrapingService"/> to retrieve game data.</param>
        /// <param name="emailService">An instance of <see cref="EmailService"/> to send emails.</param>
        public SportNotifierController(ScrapingService scrapingService, EmailService emailService)
        {
            ScrapingService = scrapingService;
            EmailService = emailService;
        }
        /// <summary>
        /// Retrieves the latest sports results and sends them in an email.
        /// </summary>
        public void SendReport()
        {
            var result = ScrapingService.GetGames();

            EmailService.Send(result.ToString());
        }

    }
}
