using System;
using System.Globalization;
using System.Xml;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;

namespace SportsResultsNotifier;

public class SportsResultsNotifier
{
    public static void Main()
    {
		var html = @"https://www.basketball-reference.com/boxscores/";

        HtmlWeb web = new();

        var htmlDoc = web.Load(html);

        var node = htmlDoc.DocumentNode.SelectSingleNode("//body/div[@id='wrap']/div[@id='content']");

        var title = node.SelectSingleNode("//h1")
            .InnerText
            .Replace("NBA Games Played on ", "");;

        title = title.Replace("NBA Games Played on ", "");

        var date = DateOnly.TryParseExact(title, "MMMM d, yyyy", 
            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly scrappedDate);

        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        var dateFromConfig = config.GetSection("Setttings").Value;

    
        config.GetSection("Settings").Value = scrappedDate.ToString();  //cannot store, use inbuilt json to store

        var dateFromConfigBool = DateOnly.TryParseExact(title, "MMMM d, yyyy", 
            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly scrappedDateFromConfig);

        var gameSummaries = node.SelectSingleNode("//div[@class='game_summaries']");

    }
}
