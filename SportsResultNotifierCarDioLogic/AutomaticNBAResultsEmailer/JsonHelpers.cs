using AutomaticNBAResultsEmailer.Model;
using Newtonsoft.Json;

namespace AutomaticNBAResultsEmailer;

internal class JsonHelpers
{


    static public void WriteToJSON(string lastDate)
    {
        LastDateStorage config = new LastDateStorage()
        {
            LastDate = lastDate,
        };

        string json = JsonConvert.SerializeObject(config, Formatting.Indented);

        string filePath = NBAconfigHelpers.GetSolutionFolderPath() + @"\AutomaticNBAResultsEmailer\bin\Debug\net7.0\LastDateStorage.json";
        File.WriteAllText(filePath, json);
    }

    static public string ReadFromJSON()
    {
        string filePath = NBAconfigHelpers.GetSolutionFolderPath() + @"\AutomaticNBAResultsEmailer\bin\Debug\net7.0\LastDateStorage.json";
        string lastDate;

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            LastDateStorage loadedConfig = JsonConvert.DeserializeObject<LastDateStorage>(json);

            lastDate = loadedConfig.LastDate;

            return lastDate;
        }
        else
        {
            // Handle the case where the configuration file doesn't exist yet
            lastDate = "";
            return lastDate;
        }
    }
}
