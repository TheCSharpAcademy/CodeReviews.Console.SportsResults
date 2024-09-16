namespace SportsResults.Models
{
    public class SportData
    {
        public string Winner { get; set; } = "";
        public string Loser { get; set; } = "";
        public int WinnerScore { get; set; }
        public int LoserScore { get; set; }
    }
}