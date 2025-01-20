using System.Text.Json;
using SportsResultsNotifier.Models;

namespace SportsResultsNotifier.Helpers;

public class DataSaving()
{
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };
    
    public static void UpdateAppSetting(DateOnly scrappedDate, DateOnly runDate, AppVars appVars)
    {
        AppSettingsJson config = new () {Settings = appVars};

        if(config.Settings is not null)
        {
            config.Settings.LastScrappedDate = scrappedDate;
            config.Settings.LastRunDate = runDate;
        }
        var updatedConfigJson = JsonSerializer.Serialize(config, Options);
        File.WriteAllText("appsettings.json", updatedConfigJson);
    }
}