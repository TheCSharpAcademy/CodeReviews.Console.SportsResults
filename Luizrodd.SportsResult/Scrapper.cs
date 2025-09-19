using HtmlAgilityPack;
using Luizrodd.SportsResult.Models;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Luizrodd.SportsResult;

public class Scrapper
{
    private readonly string URL;
    private readonly string USER_AGENT;
    private readonly string GAMES_NODES_PATH = "//*[@id='content']/div[3]/div";
    private readonly string TEAM_PATH = "./td[1]/a";
    private readonly string PTS_PATH = "./td[2]";
    private readonly string GAME_TABLE_PATH = ".//table[1]//tbody/tr";

    private readonly int TEAM_ONE_PATH = 0;
    private readonly int TEAM_TWO_PATH = 1;

    public Scrapper(IConfiguration configuration)
    {
        URL = configuration["Scrapper:Url"];
        USER_AGENT = configuration["Scrapper:UserAgent"];
    }

    public string GetGamesToSendEmail()
    {
        var web = new HtmlWeb
        {
            UserAgent = USER_AGENT
        };
        var doc = web.Load(URL);

        var games = Get(doc);

        return BuildEmailBody(games);
    }

    private string BuildEmailBody(IEnumerable<Game> games)
    {
        var sb = new StringBuilder();
        sb.AppendLine("🏆 Resultados das Partidas\n");

        int count = 1;
        foreach (var game in games)
        {
            sb.AppendLine($"Partida {count++}:");
            sb.AppendLine($"{game.TeamOne.Name} ({game.TeamOne.Points}) x {game.TeamTwo.Name} ({game.TeamTwo.Points})");
            sb.AppendLine($"➡️ Vencedor: {game.Winner}");
            sb.AppendLine(new string('-', 40));
        }

        return sb.ToString();
    }

    private List<Game> Get(HtmlDocument doc)
    {
        var lists = doc.DocumentNode.SelectNodes(GAMES_NODES_PATH);

        var games = new List<Game>();

        if (lists != null)
        {
            int i = 1;
            foreach (var list in lists)
            {
                var rows = list.SelectNodes(GAME_TABLE_PATH);
                if (rows != null && rows.Count >= 2)
                {
                    string team1 = rows[TEAM_ONE_PATH].SelectSingleNode(TEAM_PATH)?.InnerText.Trim();
                    string pts1 = rows[TEAM_ONE_PATH].SelectSingleNode(PTS_PATH)?.InnerText.Trim();

                    string team2 = rows[TEAM_TWO_PATH].SelectSingleNode(TEAM_PATH)?.InnerText.Trim();
                    string pts2 = rows[TEAM_TWO_PATH].SelectSingleNode(PTS_PATH)?.InnerText.Trim();

                    var game = new Game(team1, team2, int.Parse(pts1), int.Parse(pts2));
                    games.Add(game);
                }
            }
        }

        return games;
    }
}
