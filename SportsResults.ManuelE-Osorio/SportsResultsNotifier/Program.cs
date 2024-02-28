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
using SportsResultsNotifier.Controllers;
using SportsResultsNotifier.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace SportsResultsNotifier;

public class SportsResultsNotifier
{
    public static async Task Main()
    {
        // UpdateAppSetting(DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today));
        IHost? app;
        try
        {
            app = StartUp.AppInit();
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            Thread.Sleep(4000);
            return;
        }
        var controller = app.Services.CreateScope()
            .ServiceProvider.GetRequiredService<DataController>();
        await controller.Crawl();
    }


    // public static void Main()
    // {
	// 	var html = @"https://www.basketball-reference.com/boxscores/";

    //     HtmlWeb web = new();

    //     var htmlDoc = web.Load(html);

    //     var node = htmlDoc.DocumentNode.SelectNodes("//body/div[@id='wrap']/div[@id='content']"+
    //         "/div[@class='game_summaries']/div[@class='game_summary expanded nohover ']");

    //     foreach(HtmlNode game in node)
    //     {
    //         var loserResults = game.SelectSingleNode("//table[@class='teams']/tbody/tr[@class='loser']");
    //         var winnerResults = game.SelectSingleNode("//table[@class='teams']/tbody/tr[@class='winner']");
    //         SportResults result = new();
            
    //             result.LoserTeamName = loserResults?.ChildNodes[1].InnerText;
    //             result.LoserTeamScore = loserResults?.ChildNodes[3].InnerText;
    //             result.WinnerTeamName = winnerResults?.ChildNodes[1].InnerText;
    //             result.WinnerTeamScore = winnerResults?.ChildNodes[3].InnerText;
    //     };
        // var title = node.SelectSingleNode("//h1")
        //     .InnerText
        //     .Replace("NBA Games Played on ", "");

        // GetDateFromWeb(title.Replace("NBA Games Played on ", ""), out DateOnly scrappedDate);
        // GetDateFromConfig(out DateOnly storedDate);



        // UpdateAppSetting(["LastScrappedDate", "LastRunDate"], 
        //         [scrappedDate.ToString("yyyy/MM/dd"), DateTime.Today.ToString("yyyy/MM/dd")]);
        
          

        // var dateFromConfigBool = DateOnly.TryParseExact(title, "MMMM d, yyyy", 
        //     CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly scrappedDateFromConfig);

        // var gameSummaries = node.SelectSingleNode("//div[@class='game_summaries']");

        //SendEmail(gameSummaries.InnerHtml);
    
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

    public static void UpdateAppSetting(DateOnly scrappedDate, DateOnly runDate)
    {
        var configJson = File.ReadAllText("appsettings.json");
        var config = JsonSerializer.Deserialize<AppSettingsJson>(configJson) ?? 
            throw new FileNotFoundException("appsettings.json is not found, please make sure it is in the program folder.");
        
        if(config.Settings is not null)  //TestChange
        {
            config.Settings.LastScrappedDate = scrappedDate;
            config.Settings.LastRunDate = runDate;
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
