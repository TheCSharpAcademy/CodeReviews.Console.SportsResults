namespace SportsResults.cacheMe512;

internal class EmailConfig
{
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public string FromEmail { get; set; }
    public string ToEmail { get; set; }
    public string EmailPassword { get; set; }
}
