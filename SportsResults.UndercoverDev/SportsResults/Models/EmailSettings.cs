namespace SportsResults.Models;
public class EmailSettings
{
    public string SmtpServer { get; set; } = "";
    public int SmtpPort { get; set; }
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string FromAddress { get; set; } = "";
    public string ToAddress { get; set; } = "";
}