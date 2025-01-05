using Microsoft.Extensions.Configuration;
using SportNotifierApplication.Models;
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

// Create an instance of the mail sender with dummy mail text
EmailSender mailSender = new(smtpAddress, portNumber,
    enableSsl, senderEmail, emailPassword, receiverEmail, subject);


while (true)
{
    // Ensure the task is finished
    mailSender.EnsureTaskIsFinished();

    System.Console.WriteLine($"Running task at: {DateTime.Now}");
    //Send the sport data
    mailSender.SetText(
        MailGenerator.GenerateTableMail(
            parser.GetText()
            )
        );

    //Send it asynchronously
    mailSender.SendMailAsync();

    await Task.Delay(TimeSpan.FromHours(24));
}