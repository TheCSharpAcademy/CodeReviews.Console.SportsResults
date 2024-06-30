using Microsoft.Extensions.Configuration;

namespace SportsScraper;

public static class ConfigManager
{
    private static Config? _config;

    public static Config Config
    {
        get => _config ?? throw new Exception("Missing config");

        set => _config = value;
    }

    public static void Init()
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        string scrapeUrl = config["Scrape:Url"] ??
            throw new Exception("Scrape URL missing");
        string outboundEmailAddress = config["Email:OutboundAddress"] ??
            throw new Exception("Outbound email address missing");
        string fromEmailAddress = config["Email:From"] ??
            throw new Exception("Outbound email address missing");
        string fromEmailPassword = config["Email:FromPassword"] ??
            throw new Exception("Outbound email address password missing");
        string smtpAddress = config["Email:SmtpAddress"] ??
           throw new Exception("Smtp address missing");
        string smtpPort = config["Email:SmtpPort"] ??
           "587";

        Config = new Config(
            scrapeUrl, outboundEmailAddress, fromEmailAddress,
            fromEmailPassword, smtpAddress, smtpPort
        );
    }
}

public record class Config(
    string ScrapeUrl,
    string OutboundEmailAddress,
    string FromEmailAddress,
    string FromEmailPassword,
    string SmtpAddress,
    string SmtpPort
);