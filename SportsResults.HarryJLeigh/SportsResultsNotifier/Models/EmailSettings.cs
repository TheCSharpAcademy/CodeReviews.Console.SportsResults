namespace SportsResultsNotifier.Models;

public class EmailSettings
{
    public string? SmtpServer { get; init; }
    public int Port { get; init; }
    public string? SenderEmail { get; init; }
    public string? SenderName { get; init; }
    public string? Password { get; init; }
    public bool UseSsL { get; init; }
    public string? ReceiverEmail { get; init; }
    public string? Subject { get; init; }
}