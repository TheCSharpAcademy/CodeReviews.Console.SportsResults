using Spectre.Console;
using SportsResultNotifierCarDioLogic.Helpers;
using System.Globalization;

namespace SportsResultNotifierCarDioLogic;

static internal class UILogic
{
    static internal void SetEmailConfigs()
    {
        string email = AnsiConsole.Prompt(new TextPrompt<string>("Enter the email:"));

        if (IsValidEmail(email))
        {
            string password = AnsiConsole.Prompt(new TextPrompt<string>("Enter the email password:").Secret());
            bool IsAutomaticEmailsOn = false;

            string encryptedPassword = Encryptor.EncryptPassword(password);
            NBAconfigHelpers.WriteToNBAconfig(email, encryptedPassword, IsAutomaticEmailsOn);

            Console.WriteLine("User Configs were set! Automatic Emails are not active by default!");
            Console.ReadLine();
        }
        else
        {
            Console.WriteLine("Invalid email address format.");
            Console.ReadLine();
        }
    }

    static internal bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    static internal bool IsValidDate(string dateString)
    {
        if (DateTime.TryParseExact(dateString, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    static internal void DeleteEmailConfigs()
    {
        string email = "";
        string password = "";
        bool isAutomaticEmailsOn = false;

        NBAconfigHelpers.WriteToNBAconfig(email, password, isAutomaticEmailsOn);
    }

}
