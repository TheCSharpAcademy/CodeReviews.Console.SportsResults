using System.Text;
using System.Xml;

namespace SportsResults.BrozDa.Models
{
    internal class ScrapingServiceResult
    {
        public bool IsGameValid { get; set; }
        public List<Game>? Games { get; set; }
        public string? ErrorMessage { get; set; }

        public string? Comment { get; set; }

        public override string ToString()
        {
            if (!IsGameValid)
            {
                return "Unsucessful scraping: " + ErrorMessage;
            }
            if(Games is null)
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
        public static ScrapingServiceResult NoGamePlayed()
        {
            return new ScrapingServiceResult
            {
                IsGameValid = true,
                Games = null,
                Comment = "No Games were played"
            };
        }
        public static ScrapingServiceResult Success(List<Game> games)
        {
            return new ScrapingServiceResult
            {
                IsGameValid = true,
                Games = games
            };
        }

        public static ScrapingServiceResult Fail(string errorMsg)
        {
            return new ScrapingServiceResult
            {
                IsGameValid = false,
                ErrorMessage = errorMsg
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
