namespace SportsResults.Forser.Models
{
    internal class SettingsModel
    {
        public string EmailServer { get; set; }
        public string Password { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string ToEmail { get; set; }
        public string ToName { get; set; }
        public string DefaultUrl { get; set; }
    }
}