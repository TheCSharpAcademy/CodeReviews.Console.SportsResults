namespace SportsResults.ukpagrace.Model
{
    public class Game
    {
        public string Winner {  get; set; } = string.Empty;
        public string Loser {  get; set; } = string.Empty;
        
        public int WinnerScore { get; set; }

        public int LoserScore { get; set;}
    }
}
