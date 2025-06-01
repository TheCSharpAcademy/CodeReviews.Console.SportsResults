using HtmlAgilityPack;
using SportsResults.BrozDa.Models;

namespace SportsResults.BrozDa
{
    internal class ScrapingService
    {
        private string _url;
        public ScrapingService(string url) 
        { 
            _url = url;
        }
        public void GetTeams()
        {
            //2game day:
            // _url = "https://www.basketball-reference.com/boxscores/?month=5&day=11&year=2025";

            //1 game day:
             _url = "https://www.basketball-reference.com/boxscores/?month=5&day=16&year=2025";

            //0 game day:
            //_url = "http://basketball-reference.com/boxscores/?month=5&day=17&year=2025";

            var web = new HtmlWeb();
            var doc = web.Load(_url);

            var games = doc.DocumentNode.SelectNodes("//div[@class='game_summary expanded nohover ']");

            if (games is null || games.Count == 0)
            {
                Console.WriteLine("Nada");
                return;
            }

            List<Game> gameList = new List<Game>();

            foreach (var game in games) 
            {
                var playedGame = GetGame(game);
                Console.WriteLine(playedGame.ToString());
            }
           
        }
        public ScrapingServiceResult GetGame(HtmlNode gameDetails)
        {
            Team? teamA = GetTeam(gameDetails.SelectSingleNode(".//table[2]/tbody/tr[1]"));
            Team? teamB = GetTeam(gameDetails.SelectSingleNode(".//table[2]/tbody/tr[2]"));

            if (teamA is null || teamB is null) 
            {
                return ScrapingServiceResult.InvalidTeams();
            }


            Stat? pts = GetStat(gameDetails.SelectSingleNode(".//table[3]/tbody/tr[1]"));
            Stat? trb = GetStat(gameDetails.SelectSingleNode(".//table[3]/tbody/tr[2]"));

           if(pts is null || trb is null)
            {
                return ScrapingServiceResult.InvalidStats();
            }

            return ScrapingServiceResult.Success(GetValidGame(teamA, teamB, pts, trb));
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
        private Team? GetTeam(HtmlNode? teamInfo)
        {
            if(teamInfo is null)
            {
                return null;
            }

            string teamName = teamInfo.SelectSingleNode(".//td/a")?.InnerHtml ?? "error while fetching teamName";

            List<int>? quarterScores = GetQuarterScores(teamInfo.SelectNodes(".//td[@class='center']"));

            if(quarterScores is null)
            {
                return null;
            }    

            return new Team()
            {
                Name = teamName,
                TotalScore = quarterScores.Sum(),
                Quarters = quarterScores
            };
        }
        private List<int>? GetQuarterScores(HtmlNodeCollection? quarterScores) 
        {
            if(quarterScores is null)
            {
                return null;
            }

            List<int> scores = new List<int>();

            foreach (var quarter in quarterScores)
            {

                scores.Add(int.TryParse(quarter.InnerHtml, out var score) 
                    ? score 
                    : 0
                    );
            }
            return scores; 
        }

        private Stat? GetStat(HtmlNode? statRow) 
        {
            if(statRow is null)
            {
                return null;
            }

            string statName;
            string? statPlayer, statTeam;

            statName = statRow.SelectSingleNode(".//td[1]/strong")?
                .InnerHtml ?? "error while fetching stat name";

            statPlayer = statRow.SelectSingleNode(".//td[2]/a")?
                .InnerHtml ?? null;

            if(statPlayer is null)
            {
                statPlayer = statRow.SelectSingleNode(".//td[2]")?.InnerHtml ?? "error while fetching stat player";
                statTeam = null;
                
            }

            statTeam = statRow.SelectSingleNode(".//td[2]")?
                .LastChild
                .InnerHtml
                .Substring(1);

            string statPoints = statRow.SelectSingleNode(".//td[3]")?
                .InnerHtml ?? 
                "error while fetching stat points";

            return new Stat() { Name = statName, Player = statPlayer, Team = statTeam, Points = statPoints };
        }
    }
}
