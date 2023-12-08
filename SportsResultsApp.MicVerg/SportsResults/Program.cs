
using HtmlAgilityPack;
using System.Text.RegularExpressions;

var url = "https://www.basketball-reference.com/boxscores/";
var web = new HtmlWeb();
var loadedDocument = web.Load(url);

var gameSummaryNodes = loadedDocument.DocumentNode.SelectNodes("//*[contains(@class, 'winner')]");

Console.WriteLine("The winning teams today are: \n");

foreach (var node in gameSummaryNodes)
{
    string innerText = node.InnerText.Trim();

    // clean string
    innerText = Regex.Replace(innerText, @"\s+", " "); // Replace multiple spaces with a single space
    innerText = innerText.Replace("\n", "").Replace("\t", ""); // Remove newline and tab characters
    innerText = innerText.Replace("&nbsp;", ""); // Remove non-breaking space
    innerText = innerText.Replace("Final", "", StringComparison.OrdinalIgnoreCase);
    string teamName = Regex.Match(innerText, @"^[^\d]+").Value.Trim();


    Console.WriteLine(teamName);
}

Console.ReadLine();