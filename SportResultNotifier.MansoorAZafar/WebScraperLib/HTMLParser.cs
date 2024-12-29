using HtmlAgilityPack;
using WebScraperLib.Models;

namespace WebScraperLib;

public class HTMLParser
{
    private HtmlWeb Web;
    private HtmlDocument Document;

    public HTMLParser(string url)
    {
        this.Web = new HtmlWeb();
        this.Document = this.Web.Load(url);
    }

    public List<Game> GetText()
    {
        var gameList = this.Document.DocumentNode
            .SelectSingleNode("//div[@class='game_summaries']").ChildNodes;

        List<Game> games = new();

        string winner, loser;
        int winner_score, loser_score;

        foreach(var game in gameList)
        {
            //Skip all blank lines | empty lines (i.e lines with just \n)
            if (!game.HasAttributes) continue;

            int[] scores = new int[8];
            // Get winner stats
            winner = game.SelectSingleNode(".//table/tbody/tr[2]/td[1]/a").InnerHtml;
            int.TryParse(game.SelectSingleNode(".//table/tbody/tr[2]/td[2]").InnerHtml, out winner_score);
            
            loser = game.SelectSingleNode(".//table[1]/tbody/tr[1]/td[1]/a").InnerHtml;
            int.TryParse(game.SelectSingleNode(".//table/tbody/tr[1]/td[2]").InnerHtml, out loser_score);

            for(int i = 0; i < scores.Length; ++i)
            {
                int tdIndex = (i % 4) + 1;

                // 1 if i < 4 otherwise 2
                int trIndex = i < 4 ? 1 : 2;

                // + 1 because the starting is the team name
                int.TryParse(game.SelectSingleNode($".//table[2]/tbody/tr[{trIndex}]/td[{tdIndex + 1}]").InnerHtml, out scores[i]);
            
            
            }

            games.Add(new Game(
                new Team(winner, winner_score),
                new Team(loser, loser_score), 
                scores
            ));
        }

        return games;
    }

}