namespace SportsResults.samggannon;

public class EmailSettings
{
    public string? SmtpAddress { get; set; }
    public int SmtpPort { get; set; }
    public string? FromAddress { get; set; }
    public string? SmtpPassword { get; set; }
    public string? ToAddress { get; set; }
}
