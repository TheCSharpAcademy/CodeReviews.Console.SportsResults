using Luizrodd.SportsResult;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory) 
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var scrapper = new Scrapper(config);
var email = new EmailSender(config);

email.SendEmail(scrapper.GetGamesToSendEmail());
