namespace SportsResults.KamilKolanowski.Models;

internal class Mail
{
    public string Subject { get; set; }
    public MimeKit.TextPart Body { get; set; }
}
