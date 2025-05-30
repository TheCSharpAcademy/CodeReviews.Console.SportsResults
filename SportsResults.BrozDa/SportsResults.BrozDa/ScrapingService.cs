using HtmlAgilityPack;

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
            _url = "https://www.basketball-reference.com/boxscores/?month=5&day=18&year=2025";

            var web = new HtmlWeb();
            var doc = web.Load(_url);

            var games = doc.DocumentNode.SelectNodes("//div[@class='game_summary expanded nohover ']");

            if (games is null || games.Count == 0)
            {
                Console.WriteLine("Nada");
                return;
            }

            Team winner = new Team();
            Team loser = new Team();

            foreach (var game in games)
            {
                winner = GetSingleTeam(game.SelectSingleNode("//tr[@class='winner']"));
                loser = GetSingleTeam(game.SelectSingleNode("//tr[@class='loser']"));
            }
            
            
        }
        public Team GetSingleTeam(HtmlNode game)
        {
            var name =  game.SelectSingleNode(".//td/a").InnerHtml;
            var score = game.SelectSingleNode(".//td[@class='right']").InnerHtml;
          
            return new Team() { Name = name, TotalScore = int.Parse(score)};
        }

    }
}
