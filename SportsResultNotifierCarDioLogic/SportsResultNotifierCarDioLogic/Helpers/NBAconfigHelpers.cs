using Newtonsoft.Json;
using SportsResultNotifierCarDioLogic.Model;
using System.Reflection;

public class NBAconfigHelpers
{
    static public void WriteToNBAconfig(string email, string encryptedPassword, bool isAutomaticEmailsOn)
    {
        EmailConfig config = new EmailConfig()
        {
            Email = email,
            Password = encryptedPassword,
            IsAutomaticEmailsOn = isAutomaticEmailsOn
        };

        string json = JsonConvert.SerializeObject(config, Formatting.Indented);

        string filePath = GetSolutionFolderPath()+ @"\SportsResultNotifierCarDioLogic\bin\Debug\net7.0\NBAconfig.json";
        File.WriteAllText(filePath, json);
    }

    static public (string email, string decryptedPassword, bool isAutomaticEmailsOn) ReadFromNBAconfig()
    {
        string filePath = GetSolutionFolderPath() + @"\SportsResultNotifierCarDioLogic\bin\Debug\net7.0\NBAconfig.json";

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            EmailConfig loadedConfig = JsonConvert.DeserializeObject<EmailConfig>(json);

            string email = loadedConfig.Email;
            string encryptedPassword = loadedConfig.Password;
            bool isAutomaticEmailsOn = loadedConfig.IsAutomaticEmailsOn;

            return (email, encryptedPassword, isAutomaticEmailsOn);
        }
        else
        {
            return ("", "", false);
        }
    }

    static public string GetSolutionFolderPath()
    {
        string assemblyLocation = Assembly.GetExecutingAssembly().Location;
        string assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
        string solutionFolderPath = Path.GetFullPath(Path.Combine(assemblyDirectory, "..\\..\\..\\.."));
        return solutionFolderPath;
    }
}