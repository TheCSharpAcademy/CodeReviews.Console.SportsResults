using System.Dynamic;
using System.Text;
using HtmlAgilityPack;

namespace SportsResult;

public class Scrapper
{

    private MatchScore _matchScore;
    private readonly string _urlLink = "https://www.basketball-reference.com/boxscores/";
    private HtmlDocument _doc;

    public string MatchDate
    {
        get => _matchScore.MatchTitle;
    }

    public Scrapper()
    {
        _matchScore = new MatchScore();
        
    }

    public void ScrapeMatchData()
    {
        HtmlWeb web = new HtmlWeb();
        _doc = web.Load($"{_urlLink}");
        UpdateTeamInfo();

        var firstTeam = _doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[3]/div/table[1]/tbody/tr[1]");
        var secondTeam = _doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[3]/div/table[1]/tbody/tr[2]");
        var firstTeamScores = _doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[3]/div/table[2]/tbody/tr[1]");
        var secondTeamScores = _doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[3]/div/table[2]/tbody/tr[2]");
        if(firstTeam == null || secondTeam == null || firstTeamScores == null || secondTeamScores == null) return;
        if (firstTeam.GetAttributeValue<string>("class", null) == "loser")
        {
            UpdateTeamsScores(secondTeam, firstTeam, secondTeamScores, firstTeamScores);
        }
        else
        {
            UpdateTeamsScores(firstTeam, secondTeam, firstTeamScores, secondTeamScores);
        }
    }

    private void UpdateTeamInfo()
    {
        var matchTitle = _doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/h1");
        _matchScore.MatchTitle = matchTitle == null ? "No Title Found" : matchTitle.InnerHtml;
        var matchDate = _doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[1]/span");
        _matchScore.MatchDate = matchDate == null ? "No Date Found" : matchDate.InnerHtml;

        var topScorer = _doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[3]/div/table[3]/tbody/tr[1]");
        var topRebounder = _doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[3]/div/table[3]/tbody/tr[2]");
        if(topScorer != null)
        {
            _matchScore.TopScorer.Name = topScorer.SelectSingleNode("td[2]").Element("a").InnerHtml;
            _matchScore.TopScorer.Team = topScorer.SelectSingleNode("td[2]").Element("#text").InnerHtml.Substring(1);
            _matchScore.TopScorer.Score = Convert.ToInt32(topScorer.SelectSingleNode("td[3]").InnerHtml);
        }
        if(topRebounder != null)
        {
            _matchScore.TopRebounder.Name = topRebounder.SelectSingleNode("td[2]").Element("a").InnerHtml;
            _matchScore.TopRebounder.Team = topRebounder.SelectSingleNode("td[2]").Element("#text").InnerHtml.Substring(1);
            _matchScore.TopRebounder.Rebounds = Convert.ToInt32(topRebounder.SelectSingleNode("td[3]").InnerHtml);
        }
    }

    private void UpdateTeamsScores(HtmlNode winner, HtmlNode loser, HtmlNode winnerScores, HtmlNode loserScores)
    {
        _matchScore.Loser.TeamName = loser.SelectSingleNode("td[1]").Element("a").InnerHtml.ToString();
        _matchScore.Loser.TotalScore = Convert.ToInt32(loser.SelectSingleNode("td[2]").InnerHtml);
        _matchScore.Loser.Q1Score = Convert.ToInt32(loserScores.SelectSingleNode("td[2]").InnerHtml);
        _matchScore.Loser.Q2Score = Convert.ToInt32(loserScores.SelectSingleNode("td[3]").InnerHtml);
        _matchScore.Loser.Q3Score = Convert.ToInt32(loserScores.SelectSingleNode("td[4]").InnerHtml);
        _matchScore.Loser.Q4Score = Convert.ToInt32(loserScores.SelectSingleNode("td[5]").InnerHtml);

        _matchScore.Winner.TeamName = winner.SelectSingleNode("td[1]").Element("a").InnerHtml.ToString();
        _matchScore.Winner.TotalScore = Convert.ToInt32(winner.SelectSingleNode("td[2]").InnerHtml);
        _matchScore.Winner.Q1Score = Convert.ToInt32(winnerScores.SelectSingleNode("td[2]").InnerHtml);
        _matchScore.Winner.Q2Score = Convert.ToInt32(winnerScores.SelectSingleNode("td[3]").InnerHtml);
        _matchScore.Winner.Q3Score = Convert.ToInt32(winnerScores.SelectSingleNode("td[4]").InnerHtml);
        _matchScore.Winner.Q4Score = Convert.ToInt32(winnerScores.SelectSingleNode("td[5]").InnerHtml);
    }

    public string GetTeamScoresAsHtml()
    {
        var border = "style='border: 1px solid black; padding: 5px;'";
        var builder = new StringBuilder();
        builder.Append("<body>");
        builder.Append($"<h1 style='color: Crimson;'>{_matchScore.MatchTitle}</h1>");
        builder.Append($"<h4 style='margin-top:10px;margin-bottom:10px;'>")
            .Append($"<span style='color: Chocolate'>{_matchScore.Winner.TeamName}</span>")
            .Append(" won from ")
            .Append($"<span style='color: DarkBlue'>{_matchScore.Loser.TeamName}</span>")
            .Append("</h4>");
        builder.Append($"<table style='border: 1px solid black; border-collapse: collapse;'>");
        builder.Append($"<tr>");
        builder.Append($"<th {border}>Team Name</th>");
        builder.Append($"<th {border}>Q1 Score</th>");
        builder.Append($"<th {border}>Q2 Score</th>");
        builder.Append($"<th {border}>Q3 Score</th>");
        builder.Append($"<th {border}>Q4 Score</th>");
        builder.Append($"<th {border}>Total Score</th>");
        builder.Append($"</tr>");
        builder.Append($"<tr>");
        builder.Append($"<td {border}>{_matchScore.Winner.TeamName}</td>");
        builder.Append($"<td {border}>{_matchScore.Winner.Q1Score}</td>");
        builder.Append($"<td {border}>{_matchScore.Winner.Q2Score}</td>");
        builder.Append($"<td {border}>{_matchScore.Winner.Q3Score}</td>");
        builder.Append($"<td {border}>{_matchScore.Winner.Q4Score}</td>");
        builder.Append($"<td {border}>{_matchScore.Winner.TotalScore}</td>");
        builder.Append($"</tr>");
        builder.Append($"<tr>");
        builder.Append($"<td {border}>{_matchScore.Loser.TeamName}</td>");
        builder.Append($"<td {border}>{_matchScore.Loser.Q1Score}</td>");
        builder.Append($"<td {border}>{_matchScore.Loser.Q2Score}</td>");
        builder.Append($"<td {border}>{_matchScore.Loser.Q3Score}</td>");
        builder.Append($"<td {border}>{_matchScore.Loser.Q4Score}</td>");
        builder.Append($"<td {border}>{_matchScore.Loser.TotalScore}</td>");
        builder.Append($"</tr>");
        builder.Append($"</table>");
        builder.Append($"<div style='margin-top:10px;margin-bottom:10px; color: Chocolate'>");
        builder.Append($"{_matchScore.TopScorer.Name} was the standout player for {_matchScore.TopScorer.Team} with {_matchScore.TopScorer.Score} points.");
        builder.Append($"</div>");
        builder.Append($"<div style='margin-top:10px;margin-bottom:10px; color: DarkBlue'>");
        builder.Append($"{_matchScore.TopRebounder.Name} was notable for {_matchScore.TopRebounder.Team}, especially in rebounding with {_matchScore.TopRebounder.Rebounds} rebounds.");
        builder.Append($"</div>");
        builder.Append("</body>");
        return builder.ToString();
    }

}