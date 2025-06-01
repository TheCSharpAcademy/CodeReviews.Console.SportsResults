namespace SportsResults.BrozDa
{
    internal class Team
    {
        public string? Name { get; set; } = null!;
        public int? TotalScore { get; set; }
        public List<int>? Quarters { get; set; } = null!;
    }
}

