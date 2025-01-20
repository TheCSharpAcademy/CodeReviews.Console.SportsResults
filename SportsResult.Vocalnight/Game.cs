namespace SportsResult.Vocalnight
{
    public class Game
    {
        public string? LoserName { get; set; }
        public string? WinnerName { get; set; }
        public string? WinnerScore { get; set; }
        public string? LoserScore { get; set; }

        public string MatchResults()
        {
           return $"{WinnerName} has won against {LoserName}! The match score was {WinnerScore} to {LoserScore}.";
        }
    }
}
