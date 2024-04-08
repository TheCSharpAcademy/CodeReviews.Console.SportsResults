using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SportsResults.Dejmenek.Services;

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var services = new ServiceCollection();

services.AddSingleton(configuration);
services.AddSingleton<HtmlWeb>();
services.AddSingleton<INbaDataProcessorService, NbaDataProcessorService>();
services.AddSingleton<INbaDataScraperService, NbaDataScraperService>();
services.AddSingleton<IEmailNotificationService, EmailNotificationService>();

var serviceProvider = services.BuildServiceProvider();

var scraper = serviceProvider.GetRequiredService<INbaDataScraperService>();
var processor = serviceProvider.GetRequiredService<INbaDataProcessorService>();
var emailService = serviceProvider.GetRequiredService<IEmailNotificationService>();

var easternStandings = scraper.ScrapeEasternConferenceStandings();
var westernStandings = scraper.ScrapeWesternConferenceStandings();
var games = scraper.ScrapeGames();
string emailBody = processor.PrepareEmailBody(games, easternStandings, westernStandings);
emailService.SendEmail(emailBody);
