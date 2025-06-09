namespace SportsResults.BrozDa
{
    /// <summary>
    /// Represents Team who played the basketball game
    /// </summary>
    internal class Team
    {
        public string? Name { get; set; } = null!;
        public int? TotalScore { get; set; }
        public List<int>? Quarters { get; set; } = null!;

        /// <summary>
        /// Returns a string representation of the Team.
        /// </summary>
        /// <returns>A string representation of the Team</returns>
        public override string ToString()
        {
            if (Name is null || Quarters is null)
            {
                return "Invalid team";
            }

            return $"{Name} \t {string.Join(" | ", Quarters!)}";
        }
    }
}