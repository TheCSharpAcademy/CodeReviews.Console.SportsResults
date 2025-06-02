using HtmlAgilityPack;
using SportsResults.BrozDa.Models;
using System;

namespace SportsResults.BrozDa
{
    internal class ScrapingService
    {
        private string _url;
        public ScrapingService(string url) 
        { 
            _url = url;
        }

        public ScrapingServiceResult GetGames(string url)
        {
            //2game day:
             _url = "https://www.basketball-reference.com/boxscores/?month=5&day=11&year=2025";

            //1 game day:
            //_url = "https://www.basketball-reference.com/boxscores/?month=5&day=16&year=2025";

            //0 game day:
            //_url = "http://basketball-reference.com/boxscores/?month=5&day=17&year=2025";

            var web = new HtmlWeb();
            var doc = web.Load(_url);

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


        public ItemScrapingResult<Game> GetGame(HtmlNode gameNode)
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
            return ItemScrapingResult<Game>.Success(GetValidGame(teamA.Data!, teamB.Data!, pts.Data!, trb.Data!));
        }

        private HtmlNodeCollection? GetGameSummaries(HtmlDocument doc)
        {
           
            var gameSummaries = doc.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[3]/div");

            if (gameSummaries is null || gameSummaries.Count == 0)
            {
                return null;
            }

            return gameSummaries;
        }
        private bool WasNoGamePlayed(HtmlDocument doc)
        {
            var test = doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[2]/div[1]/p/strong");

            if(test is not null && test.InnerHtml == "No games played on this date.")
            {
                return true;
            }
            return false;
        }
        private Game GetValidGame(Team teamA, Team teamB, Stat pts, Stat trb)
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
                return ItemScrapingResult<Stat>.Fail("Error while reading stat Points");

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
