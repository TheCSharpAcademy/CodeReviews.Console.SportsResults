using Microsoft.Extensions.Configuration;
using SportsResultsNotifier.Models;

namespace SportsResultsNotifier.Util;

public static class EmailExtensions
{
    internal static EmailSettings LoadEmailSettings()
    {
        string configPath = GetConfigPath();

        if (!File.Exists(configPath))
            throw new FileNotFoundException($"Configuration file not found at: {configPath}");

        var config = new ConfigurationBuilder()
            .AddJsonFile(configPath, optional: false, reloadOnChange: true)
            .Build();

        var emailSettings = config.GetSection("EmailSettings").Get<EmailSettings>()
                            ?? throw new InvalidOperationException("Failed to bind EmailSettings from configuration.");
        
        return emailSettings;
    }

    private static string GetConfigPath()
    {
        string projectRoot = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName
                             ?? throw new InvalidOperationException("Unable to determine the project root directory.");
        string configPath = Path.Combine(projectRoot, "appSettings.json");
        return configPath;
    }
}