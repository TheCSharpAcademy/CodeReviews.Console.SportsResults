using HtmlAgilityPack;
using SportsResults;
using SportsResults.Models;
using System.Configuration;
using System.Net;
using System.Net.Mail;


string fromEmail = ConfigurationManager.AppSettings.Get("fromEmail");
string fromPassword = ConfigurationManager.AppSettings.Get("fromPassword");
string smtpClientUrl = ConfigurationManager.AppSettings.Get("SmtpClientUrl");
string portString = ConfigurationManager.AppSettings.Get("Port");




//list of emails to send to
List<string> emails = new List<string>
{
    "replace@email.com"
};

var scraper = new SportsScraper();
var url = "https://www.basketball-reference.com/boxscores/";
var document = new HtmlDocument();


try
{
    document = scraper.LoadDocument(url);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

var games = new List<Game>();

try
{
    games = scraper.GetGames(document);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);

}


var notification = Mail.BuildMailMessage(games, fromEmail);


int port = Mail.ParsePort(portString);

var smtpClient = new SmtpClient(smtpClientUrl)
{
    Port = port,
    Credentials = new NetworkCredential(fromEmail, fromPassword),
    EnableSsl = true
};

Mail.SendNotifications(emails, smtpClient, notification);


