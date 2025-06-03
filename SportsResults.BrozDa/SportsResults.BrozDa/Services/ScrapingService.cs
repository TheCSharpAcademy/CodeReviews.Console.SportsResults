using HtmlAgilityPack;
using SportsResults.BrozDa.Models;

namespace SportsResults.BrozDa.Services
{
    /// <summary>
    /// Handles scraping NBA game data from basketball-reference.com.
    /// </summary>
    internal class ScrapingService
    {
        /// <summary>
        /// The base URL for the basketball-reference box scores page.
        /// </summary>
        public string BaseUrl { get; } = "https://www.basketball-reference.com/boxscores/?";

        /// <summary>
        /// Retrieves game data for the current date by scraping the website.
        /// </summary>
        /// <returns>A <see cref="ScrapingServiceResult"/> representing the outcome of the scrape.</returns>
        public ScrapingServiceResult GetGames()
        {

            var url = BaseUrl + $"month={DateTime.Now.Month}&day={DateTime.Now.Day}&year={DateTime.Now.Year}";

            var web = new HtmlWeb();
            var doc = web.Load(url);

            var gameSummaries = GetGameSummaries(doc);

            if (gameSummaries is null) 
            {
                if (WasNoGamePlayed(doc)) 
                {
                    return ScrapingServiceResult.NoPlayedGames();
                }

                return ScrapingServiceResult.Fail("Error while reading game summary/summaries");
            }

            List<Game> games = new List<Game>();

            foreach (var game in gameSummaries) 
            {
                var tempGame = GetGame(game);

                if (tempGame.IsSuccessful)
                {
                    games.Add(tempGame.Data!);
                }
                else
                {
                    return ScrapingServiceResult.Fail(tempGame?.ErrorMessage ?? "Unhandled errror");
                }
            }  

            return ScrapingServiceResult.Success(games);
        }
        /// <summary>
        /// Attempts to select all game summary nodes from the HTML document.
        /// </summary>
        /// <param name="doc">The loaded HTML document.</param>
        /// <returns>A collection of game summary nodes or null.</returns>
        private HtmlNodeCollection? GetGameSummaries(HtmlDocument doc)
        {

            var gameSummaries = doc.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[3]/div");

            if (gameSummaries is null || gameSummaries.Count == 0)
            {
                return null;
            }

            return gameSummaries;
        }
        /// <summary>
        /// Checks if the loaded page indicates that no games were played today.
        /// </summary>
        /// <param name="doc">The loaded HTML document.</param>
        /// <returns>True if no games were played, false otherwise.</returns>
        private bool WasNoGamePlayed(HtmlDocument doc)
        {
            var test = doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[2]/div[1]/p/strong");

            if (test is not null && test.InnerHtml == "No games played on this date.")
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Extracts a game object from an HTML node representing a game summary.
        /// </summary>
        /// <param name="gameNode">The HTML node containing game data.</param>
        /// <returns>An <see cref="ItemScrapingResult{Game}"/> indicating success or failure.</returns>
        private ItemScrapingResult<Game> GetGame(HtmlNode gameNode)
        {
            var teamA = GetTeam(gameNode.SelectSingleNode(".//table[2]/tbody/tr[1]"));
            var teamB = GetTeam(gameNode.SelectSingleNode(".//table[2]/tbody/tr[2]"));

            if (!teamA.IsSuccessful || !teamB.IsSuccessful)
            {
                return ItemScrapingResult<Game>.Fail("Teams not scraped properly.");
            }


            var pts = GetStat(gameNode.SelectSingleNode(".//table[3]/tbody/tr[1]"));
            var trb = GetStat(gameNode.SelectSingleNode(".//table[3]/tbody/tr[2]"));

            if (!pts.IsSuccessful || !trb.IsSuccessful)
            {
                return ItemScrapingResult<Game>.Fail("Stats not scraped properly.");
            }

            //null checks are done via IsSuccessful property
            return ItemScrapingResult<Game>.Success(GetGameObject(teamA.Data!, teamB.Data!, pts.Data!, trb.Data!));
        }
        /// <summary>
        /// Constructs a <see cref="Game"/> object from given team and stat data.
        /// </summary>
        /// <param name="teamA">Team A details.</param>
        /// <param name="teamB">Team B details.</param>
        /// <param name="pts">Points stat.</param>
        /// <param name="trb">Total rebounds stat.</param>
        /// <returns>A populated <see cref="Game"/> object.</returns>
        private Game GetGameObject(Team teamA, Team teamB, Stat pts, Stat trb)
        {
            Game game = new Game();

            if (teamA.TotalScore > teamB.TotalScore)
            {
                game.Winner = teamA;
                game.Loser = teamB;
            }
            else
            {
                game.Winner = teamB;
                game.Loser = teamA;
            }

            game.Pts = pts;
            game.Trb = trb;

            return game;
        }
        /// <summary>
        /// Extracts a <see cref="Team"/> object from an HTML node.
        /// </summary>
        /// <param name="teamInfo">The HTML node representing team info.</param>
        /// <returns>An <see cref="ItemScrapingResult{Team}"/> with success or error info.</returns>
        private ItemScrapingResult<Team> GetTeam(HtmlNode? teamInfo)
        {
            if(teamInfo is null)
            {
                return ItemScrapingResult<Team>.Fail("Error while reading team Info");
            }

            string teamName = teamInfo.SelectSingleNode(".//td/a")?.InnerHtml ?? "error while fetching teamName";
            if(string.IsNullOrEmpty(teamName))
                return ItemScrapingResult<Team>.Fail("Error while reading team Name");


            var quarterScores = GetQuarterScores(teamInfo.SelectNodes(".//td[@class='center']"));

            if(!quarterScores.IsSuccessful)
            {
                return ItemScrapingResult<Team>.Fail("Error while reading team quarterScores");
            }

            //null checks are done via IsSuccessful property
            return ItemScrapingResult<Team>.Success(
                    new Team()
                    {
                        Name = teamName,
                        TotalScore = quarterScores.Data!.Sum(),
                        Quarters = quarterScores.Data
                    }
                );
        }
        /// <summary>
        /// Parses the quarter scores for a team from the HTML.
        /// </summary>
        /// <param name="quarterScores">A collection of HTML nodes with quarter scores.</param>
        /// <returns>An <see cref="ItemScrapingResult{List{int}}"/> of parsed scores.</returns>
        private ItemScrapingResult<List<int>> GetQuarterScores(HtmlNodeCollection? quarterScores) 
        {
            if(quarterScores is null)
            {
                return ItemScrapingResult<List<int>>.Fail("Error while reading team quarterScores");
            }

            List<int> scores = new List<int>();

            foreach (var quarter in quarterScores)
            {
                scores.Add(int.Parse(quarter.InnerHtml));
            }

            return ItemScrapingResult<List<int>>.Success(scores); 
        }

        /// <summary>
        /// Extracts a <see cref="Stat"/> object (PTS or TRB) from the provided HTML row.
        /// </summary>
        /// <param name="statRow">The HTML node representing a stat row.</param>
        /// <returns>An <see cref="ItemScrapingResult{Stat}"/> containing the parsed stat or an error message.</returns>
        private ItemScrapingResult<Stat> GetStat(HtmlNode? statRow)
        {
            if (statRow == null)
                return ItemScrapingResult<Stat>.Fail("Error while reading stat info");

            var statName = statRow.SelectSingleNode(".//td[1]/strong")?.InnerHtml;
            if (string.IsNullOrWhiteSpace(statName))
                return ItemScrapingResult<Stat>.Fail("Error while reading stat Name");

            var statPlayerNode = statRow.SelectSingleNode(".//td[2]/a");
            var fallstatPlayerNode = statRow.SelectSingleNode(".//td[2]");

            var statPlayer = statPlayerNode?.InnerHtml ?? fallstatPlayerNode?.InnerHtml;
            if (string.IsNullOrWhiteSpace(statPlayer))
                return ItemScrapingResult<Stat>.Fail("Error while reading stat Player");

            string? statTeam = null;
            if (fallstatPlayerNode?.LastChild != null && fallstatPlayerNode.LastChild.InnerHtml.Length > 1)
            {
                statTeam = fallstatPlayerNode.LastChild.InnerHtml.Substring(1);
            }

            var statPoints = statRow.SelectSingleNode(".//td[3]")?.InnerHtml;
            if (string.IsNullOrWhiteSpace(statPoints))
                return ItemScrapingResult<Stat>.Fail("Error while reading stat Points.");

            return ItemScrapingResult<Stat>.Success(new Stat
            {
                Name = statName,
                Player = statPlayer,
                Team = statTeam,
                Points = statPoints
            });
        }
    }
}
