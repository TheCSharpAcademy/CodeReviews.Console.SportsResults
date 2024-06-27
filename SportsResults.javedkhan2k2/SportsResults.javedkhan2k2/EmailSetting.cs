namespace SportsResults;

public class EmailSettings
{
    public string SenderEmail { get; set; }
    public string SenderPassword { get; set; }
    public string RecipientEmail { get; set; }
    public string SmtpHost { get; set; }
    public int SmtpPort { get; set; }
}