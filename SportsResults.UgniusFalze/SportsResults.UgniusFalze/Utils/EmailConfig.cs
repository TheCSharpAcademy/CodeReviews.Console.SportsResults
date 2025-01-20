namespace SportsResults.UgniusFalze.Utils;

public record EmailConfig(string SMTP, int PortNumber, string EmailFrom, string AppPassword, string EmailTo);