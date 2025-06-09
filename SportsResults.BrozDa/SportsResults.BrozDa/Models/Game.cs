namespace SportsResults.BrozDa
{
    /// <summary>
    /// Represents Basketball game scraped from the website
    /// </summary>
    internal class Game
    {
        public Team? Winner { get; set; } = null!;
        public Team? Loser { get; set; } = null!;
        public Stat? Pts { get; set; } = null!;
        public Stat? Trb { get; set; } = null!;

        /// <summary>
        /// Returns a string representation of the Game.
        /// </summary>
        /// <returns>A string representation of the Game.</returns>
        public override string ToString()
        {
            if (Winner is not null && Loser is not null && Pts is not null && Trb is not null)
            {
                return $"Winner: {Winner.Name} \t| Score: {Winner.TotalScore}\n" +
                $"Loser: {Loser.Name} \t| Score: {Loser.TotalScore}\n" +
                $"{Winner.ToString()}\n" +
                $"{Loser.ToString()}\n\n" +
                $"PTS: {Pts.Player} - {Pts.Team}, {Pts.Points}\n" +
                $"TRB: {Trb.Player} - {Trb.Team}, {Trb.Points}\n";
            }
            else
            {
                return "Invalid game";
            }
        }
    }
}