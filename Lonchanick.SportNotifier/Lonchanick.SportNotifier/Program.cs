
using MailSender;

var matchesResult = await HtmlParserClass.Demo("https://www.basketball-reference.com/boxscores/");

string aux = string.Empty;

foreach(var x in matchesResult)
    aux = x + aux.ToString();

MailMessage message = new()
{
    sender = "washmo593@gmail.com",
    passwordSender = "put ur own password here",
    reciver = "put ur own email here",
    message = aux
};


Demo.SendEmail(message);