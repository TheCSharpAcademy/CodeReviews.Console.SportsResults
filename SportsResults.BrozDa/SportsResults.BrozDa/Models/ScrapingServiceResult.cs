namespace SportsResults.BrozDa.Models
{
    internal class ScrapingServiceResult
    {
        public bool IsGameValid { get; set; }
        public Game? Game { get; set; }
        
        public string? ErrorMessage { get; set; }

        public static ScrapingServiceResult Success(Game game)
        {
            return new ScrapingServiceResult
            {
                IsGameValid = true,
                Game = game,
            };
        }
        public static ScrapingServiceResult InvalidStats()
        {
            return new ScrapingServiceResult
            {
                IsGameValid = false,
                ErrorMessage = "Error while scraping stats"
            };
        }
        public static ScrapingServiceResult InvalidTeams()
        {
            return new ScrapingServiceResult
            {
                IsGameValid = false,
                ErrorMessage = "Error while scraping teams"
            };
        }
        
    }
}
