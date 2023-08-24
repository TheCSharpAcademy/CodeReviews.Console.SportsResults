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
            //Will never get to this point logically. You only read when you want to send emails. And the progam does not let you do anything email related unitl you set up userConfigs and therefore create the file...
            // Handle the case where the configuration file doesn't exist yet
            return ("", "", false);
        }
    }

    static public string GetSolutionFolderPath()
    {
        string assemblyLocation = Assembly.GetExecutingAssembly().Location;
        string assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
        string solutionFolderPath = Path.GetFullPath(Path.Combine(assemblyDirectory, "..\\..\\..\\..")); // Adjust the number of "..\\" to match your solution's location
        return solutionFolderPath;
    }//will get the absolute path of my console project
}