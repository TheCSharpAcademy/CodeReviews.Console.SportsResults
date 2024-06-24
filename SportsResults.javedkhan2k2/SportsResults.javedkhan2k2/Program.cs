using System.Runtime.Serialization;
using Microsoft.Extensions.Configuration;
using Phonebook.Services;
using SportsResult;

var builder = new ConfigurationBuilder()
    .AddUserSecrets<Program>();
var configuration = builder.Build();

var senderEmail = configuration["SenderEmail"];
var senderPassword = configuration["SenderPassword"];
var smtpHost = configuration["SmtpHost"];
var smtpPort = Convert.ToInt32(configuration["SmtpPort"]);

var emailService = new EmailService(senderEmail, senderPassword, smtpHost, smtpPort);

Scrapper scrapper = new Scrapper();
scrapper.ScrapeMatchData();

var emailBody = scrapper.GetTeamScoresAsHtml();
await emailService.SendEmailAsHtml(scrapper.MatchDate, emailBody, "javedkhan2k2@gmail.com");