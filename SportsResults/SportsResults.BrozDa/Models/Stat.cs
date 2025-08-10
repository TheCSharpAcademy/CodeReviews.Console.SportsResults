namespace SportsResults.BrozDa
{
    /// <summary>
    /// Represents Statistics field of the basketball game
    /// </summary>
    internal class Stat
    {
        public string Name { get; set; } = null!;
        public string Player { get; set; } = null!;
        public string? Team { get; set; } = null!;
        public string Points { get; set; } = null!;
    }
}