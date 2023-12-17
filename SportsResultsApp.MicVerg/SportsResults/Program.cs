using SportsResults;
using SportsResults.Models;
using System.Text;

List<Game> gameScores = GetScores.GetGameScores();
StringBuilder stringBuilder = new StringBuilder();

foreach (Game game in gameScores)
{
    stringBuilder.AppendLine($"{game.team1PlusScore} - {game.team2PlusScore}");
}

string resultString = stringBuilder.ToString();
SendMail.SendEmail(resultString);