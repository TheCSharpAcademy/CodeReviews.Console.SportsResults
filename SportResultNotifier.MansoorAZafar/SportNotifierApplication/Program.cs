using Microsoft.Extensions.Configuration;
using SportNotifierApplication.Models;
using WebScraperLib.Models;
using EmailSenderLib;
using WebScraperLib;

//Build the app-settings.json
IConfiguration config = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json", false, false)
                            .Build();


// Get Personal Email Information
String senderEmail = config.GetSection("Email")["SenderAddress"] ?? "N/A";
String emailPassword = config.GetSection("Email")["Password"] ?? "N/A";
String receiverEmail = config.GetSection("Email")["ReceiverAddress"] ?? "N/A";
String subject = config["Subject"] ?? "N/A";


// Get other email information
String smtpAddress = config["SmtpAddress"] ?? "N/A";
int portNumber = int.Parse(config["PortNumber"] ?? "0");
bool enableSsl = bool.Parse(config["EnableSsl"] ?? "false");


// Get Web scraping URL
String url = config["URL"] ?? "N/A";


// Create the scraper and get the games
HTMLParser parser = new(url);
List<Game> games = parser.GetText();


// Create an instance of the mail sender & Generate the mail text
MailSender mailSender = new(smtpAddress, portNumber,
    enableSsl, senderEmail, emailPassword, receiverEmail, subject, MailGenerator.GenerateTableMail(games));


// Send the mail
mailSender.SendMail();
