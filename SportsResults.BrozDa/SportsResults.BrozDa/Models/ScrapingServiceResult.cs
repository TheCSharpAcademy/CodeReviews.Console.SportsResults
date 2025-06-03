using System.Text;

namespace SportsResults.BrozDa.Models
{
    /// <summary>
    /// Represents the result of a scraping operation for basketball game data.
    /// </summary>
    internal class ScrapingServiceResult
    {
        public bool IsGameValid { get; set; }
        public List<Game>? Games { get; set; }
        public string? ErrorMessage { get; set; }

        public string? Comment { get; set; }

        /// <summary>
        /// Returns a string representation of the scraping result.
        /// </summary>
        /// <returns>A description of the scraping result, or the list of games.</returns>
        public override string ToString()
        {
            if (!IsGameValid)
            {
                return "Unsucessful scraping: " + ErrorMessage;
            }
            if(Games!.Count == 0)
            {
                return "No games were played";
            }

            StringBuilder sb = new StringBuilder();
            foreach (Game game in Games) 
            {
                sb.Append(game.ToString());
                sb.Append("\n");
            }
            return sb.ToString();

        }
        /// <summary>
        /// Creates a successful result with no games played.
        /// </summary>
        /// <returns>A <see cref="ScrapingServiceResult"/> with an empty game list.</returns>
        public static ScrapingServiceResult NoPlayedGames()
        {
            return new ScrapingServiceResult
            {
                IsGameValid = true,
                Games = new List<Game>()
            };
        }
        /// <summary>
        /// Creates a successful result with a list of games.
        /// </summary>
        /// <param name="games">The list of successfully scraped games.</param>
        /// <returns>A <see cref="ScrapingServiceResult"/> containing the games.</returns>
        public static ScrapingServiceResult Success(List<Game> games)
        {
            return new ScrapingServiceResult
            {
                IsGameValid = true,
                Games = games
            };
        }
        /// <summary>
        /// Creates a failed result with an error message.
        /// </summary>
        /// <param name="errorMsg">The error message describing the failure.</param>
        /// <returns>A <see cref="ScrapingServiceResult"/> indicating failure.</returns>
        public static ScrapingServiceResult Fail(string errorMsg)
        {
            return new ScrapingServiceResult
            {
                IsGameValid = false,
                ErrorMessage = errorMsg
            };
        }
        
    }
}
