namespace SportsResultsNotifier.Arashi256.Models
{
    internal class TeamGame
    {
        public string WinnerTeam { get; set; } = string.Empty;
        public string LoserTeam { get; set; } = string.Empty;
        public int WinnerScore { get; set; }
        public int LoserScore { get; set; }

        public override string ToString()
        {
            return $"Winning Team: {WinnerTeam} with score: {WinnerScore}, Losing Team: {LoserTeam} with score: {LoserScore}";
        }
    }
}