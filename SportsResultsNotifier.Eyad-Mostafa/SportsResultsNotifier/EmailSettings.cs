namespace SportsResultsNotifier;

internal class EmailSettings
{
    public string SmtpAddress { get; set; } = string.Empty;
    public int PortNumber { get; set; }
    public bool EnableSSL { get; set; }
    public string EmailFromAddress { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string EmailToAddress { get; set; } = string.Empty;
}
