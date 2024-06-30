namespace SportsResultsApp.kalsson.Utils;
using Microsoft.Extensions.Configuration;

public static class ConfigurationHelper
{
    /// <summary>
    /// Retrieves the email settings from the configuration file.
    /// </summary>
    /// <returns>The EmailSettings object containing the SMTP server, port, sender email, sender password, and recipient email.</returns>
    public static EmailSettings GetEmailSettings()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        return config.GetSection("EmailSettings").Get<EmailSettings>() ?? throw new InvalidOperationException();
    }
}

public class EmailSettings
{
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public string SenderEmail { get; set; }
    public string SenderPassword { get; set; }
    public string RecipientEmail { get; set; }
}