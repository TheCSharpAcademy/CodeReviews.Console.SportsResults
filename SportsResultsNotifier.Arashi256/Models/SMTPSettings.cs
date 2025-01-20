namespace SportsResultsNotifier.Arashi256.Models
{
    internal class SMTPSettings
    {
        public string? SmtpServer = string.Empty;
        public string? SmtpUsername = string.Empty;
        public string? SmtpPassword = string.Empty;
        public int SmtpPort = 0;
        public bool SmtpSSLEnabled = false;
    }
}