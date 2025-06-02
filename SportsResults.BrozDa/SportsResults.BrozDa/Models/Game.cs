namespace SportsResults.BrozDa
{
    internal class Game
    {
        public Team? Winner { get; set; } = null!;
        public Team? Loser { get; set; } = null!;
        public Stat? Pts { get; set; } = null!;
        public Stat? Trb { get; set; } = null!;
        public override string ToString()
        {
            if (Winner is not null && Loser is not null && Pts is not null && Trb is not null)
            {
                return $"Winner: {Winner.Name} | Score: {Winner.TotalScore}\n" +
                $"Loser: {Loser.Name} | Score: {Loser.TotalScore}\n" +
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
