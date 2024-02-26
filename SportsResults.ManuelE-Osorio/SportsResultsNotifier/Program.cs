using System.Globalization;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using System.Net;
using Org.BouncyCastle.Tls;
using MailKit.Security;

namespace SportsResultsNotifier;

public class SportsResultsNotifier
{
    public static void Main()
    {
        StartUp.AppInit();
    }


    // public static void Main()
    // {
	// 	var html = @"https://www.basketball-reference.com/boxscores/";

    //     HtmlWeb web = new();

    //     var htmlDoc = web.Load(html);

    //     var node = htmlDoc.DocumentNode.SelectSingleNode("//body/div[@id='wrap']/div[@id='content']");

    //     var title = node.SelectSingleNode("//h1")
    //         .InnerText
    //         .Replace("NBA Games Played on ", "");

    //     GetDateFromWeb(title.Replace("NBA Games Played on ", ""), out DateOnly scrappedDate);
    //     GetDateFromConfig(out DateOnly storedDate);



    //     UpdateAppSetting(["LastScrappedDate", "LastRunDate"], 
    //             [scrappedDate.ToString("yyyy/MM/dd"), DateTime.Today.ToString("yyyy/MM/dd")]);
        
          

    //     var dateFromConfigBool = DateOnly.TryParseExact(title, "MMMM d, yyyy", 
    //         CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly scrappedDateFromConfig);

    //     var gameSummaries = node.SelectSingleNode("//div[@class='game_summaries']");

    //     //SendEmail(gameSummaries.InnerHtml);
    
    // }


    public static bool GetDateFromWeb(string title, out DateOnly scrappedDate)
    {
            return DateOnly.TryParseExact(title, "MMMM d, yyyy", 
            CultureInfo.InvariantCulture, DateTimeStyles.None, out scrappedDate);
    }

    public static bool GetDateFromConfig(out DateOnly storedDate)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        return DateOnly.TryParseExact(config.GetSection("LastScrappedDate").Value, "yyyy/MM/dd", 
            CultureInfo.InvariantCulture, DateTimeStyles.None, out storedDate);
    }

    public static void UpdateAppSetting(string[] keys, string[] values)
    {
        var configJson = File.ReadAllText("appsettings.json");
        var config = JsonSerializer.Deserialize<Dictionary<string, object>>(configJson) ?? 
            throw new FileNotFoundException("appsettings.json is not found, please make sure it is in the program folder.");
        
        for (int i = 0; i< keys.Length; i++)
        {   
            config[keys[i]] = values[i];
        }
        var updatedConfigJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("appsettings.json", updatedConfigJson);
    }
    
    public static void SendEmail(string body)
    {
        var message = new MimeMessage ();
        message.From.Add (new MailboxAddress ("Manuel", "ManuelE-OsorioTesting@outlook.com"));
        message.To.Add (new MailboxAddress ("CSharpAcademy", "aux.manuel.osorio@gmail.com"));
        message.Subject = "CSharpAcademy app progress";

        message.Body = new TextPart ("html") {
            Text = $"<b>{body}</b>",
        };

        using (var client = new SmtpClient ()) 
        {
            client.Connect ("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);
            // client.Authenticate("ManuelE-OsorioTesting@outlook.com", "PasswordHere");
            client.Send (message);
            client.Disconnect (true);
        }
    }

}
