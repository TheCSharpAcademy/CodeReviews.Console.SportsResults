using HtmlAgilityPack;
using System.Configuration;
using System.Net;
using System.Net.Mail;

Console.WriteLine("Loading!");
LoadInformation();

void LoadInformation()
{
    var url = "https://www.basketball-reference.com/boxscores/";
    var web = new HtmlWeb();
    var doc = web.Load(url);

    var loserName = doc.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[3]/div[1]/table[1]/tbody/tr[@class=\"loser\"]/td[1]/a").First().InnerText;
    var loserScore = doc.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[3]/div[1]/table[1]/tbody/tr[@class=\"loser\"]/td[2]").First().InnerText;

    var winnerName = doc.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[3]/div[1]/table[1]/tbody/tr[@class=\"winner\"]/td[1]/a").First().InnerText;
    var winnerScore = doc.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[3]/div[1]/table[1]/tbody/tr[@class=\"winner\"]/td[2]").First().InnerText;

    string title = doc.DocumentNode.SelectNodes("//*[@id=\"content\"]/h1").First().InnerText;
    string message = $"@{winnerName} has won again @{loserName}! The match score was @{winnerScore} to @{loserScore}. Thanks for subscribing!";

    SendEmail(title, message);
}

void SendEmail(string title, string message)
{

    Console.WriteLine("Sending Email...");

    var smtpAddress = ConfigurationManager.AppSettings.Get("SMTPAdress");
    int portNumber = 587;
    bool enableSSL = true;
    var emailFromAddress = ConfigurationManager.AppSettings.Get("SentEmail");
    var password = ConfigurationManager.AppSettings.Get("ReceiveEmail");
    var emailToAddress = ConfigurationManager.AppSettings.Get("Password");
    string subject = title;
    string body = message;


    using (MailMessage mail = new MailMessage())
    {
        mail.From = new MailAddress(emailFromAddress);
        mail.To.Add(emailToAddress);
        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true;

        using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
        {
            smtp.Credentials = new NetworkCredential(emailFromAddress, password);
            smtp.EnableSsl = enableSSL;
            smtp.Send(mail);
        }
    }

    Console.WriteLine("Email Sent!");
}

