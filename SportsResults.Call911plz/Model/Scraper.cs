
using HtmlAgilityPack; 

public class Scraper
{
    public static List<GameSummary> GetSummaries(string link)
    {
        List<GameSummary> summaries = [];
        string html = $@"{link}";
        HtmlWeb web = new();
        HtmlDocument htmlDoc = web.Load(html);

        var summaryNodes = htmlDoc.DocumentNode
            .SelectNodes("//div[contains(@class, \"game_summary\")]") 
            ?? throw new Exception("Summaries not found");;
        
        foreach(var node in summaryNodes)
        {
            summaries.Add(HtmlNodeToSummary(node));
        }

        return summaries;
    }

    private static GameSummary HtmlNodeToSummary(HtmlNode node)
    {
        /*
           Basketball reference have summaries containing a child indicating winner and loser.
           That winner/loser node contains a node with the actual team name and unique attribute
           This team node and attribute is also used for the score table and the score of a specific team. 
       */
        Team team1 = new()
        {
            Name = FindTeamNode(node, 1).InnerText,
            Scores = FindScores(node, 1)
        };
        Team team2 = new()
        {
            Name = FindTeamNode(node, 2).InnerText,
            Scores = FindScores(node, 2)
        };
        Team winner = (team1.Scores.Sum() > team2.Scores.Sum()) ? team1 : team2;
        Team loser = (team1.Scores.Sum() > team2.Scores.Sum()) ? team2 : team1;

        return new GameSummary(winner, loser);
    }

    // teamIndex is the index number in the teams node. Basically getting either team 1 or 2
    private static HtmlNode FindTeamNode(HtmlNode summaryNode, int teamIndex)
    {
        var teamNode = summaryNode
            .SelectSingleNode($".//table[@class=\"teams\"]/tbody/tr[{teamIndex}]/td/a") 
            ?? throw new Exception("Node not found");

        return teamNode;
    }

    private static List<int> FindScores(HtmlNode summaryNode, int teamIndex)
    {
        var scoreTablesNode = summaryNode
            .SelectSingleNode($".//table[2]/tbody/tr[{teamIndex}]")
            ?? throw new Exception("Node not found");
                
        var descendants = scoreTablesNode.Descendants("td").ToList();
 
        return 
        [
            int.Parse(descendants[1].InnerText),
            int.Parse(descendants[2].InnerText),
            int.Parse(descendants[3].InnerText),
            int.Parse(descendants[4].InnerText),
        ];
    }

    // Debug purposes
    private static void TraverseTree(HtmlNode node, int depth)
    {
        Console.WriteLine($"Depth: {depth}, Node: {node.Name}, Text: {node.InnerText.Trim()}");

        foreach (var attribute in node.Attributes)
        {
            Console.WriteLine($"Name: {attribute.Name}, Value: {attribute.Value}");
        }

        foreach(var child in node.ChildNodes)
        {
            TraverseTree(child, depth + 1);
        }
    }
}