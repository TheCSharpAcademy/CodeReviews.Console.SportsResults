# Sports Results Notifier

## Table of Contents
- [General Info](#general-info)
- [Technologies](#technologies)
- [Installation and Setup](#installation-and-setup)
- [Features](#features)

## General Info
This C# project scrapes NBA game results from [Basketball Reference website](https://www.basketball-reference.com/boxscores/), processes the data using Handlebars templating, and sends an email notification with the formatted results.

## Technologies
- C#
- HtmlAgilityPack
- Handlebars.Net

## Installation and Setup
1. Clone or download this project repository.
2. Open the solution file (SportsResults.Dejmenek.sln) in Visual Studio.
3. Install the required NuGet packages:
	- Handlebars.Net
	- HtmlAgilityPack
	- Microsoft.Extensions.Configuration
	- Microsoft.Extensions.Configuration.FileExtensions
	- Microsoft.Extensions.Configuration.Json
	- Microsoft.Extensions.DependencyInjection
4. Update the appsettings.json.
	- ```Server```: Replace with the hostname or IP address of your SMTP server for sending emails
	- ```Port```: Update with the port number used by your SMTP server (standard is usually 587)
	- ```SenderUsername```: Username used by your SMTP server
	- ```SenderPassword```: Password used by your SMTP server
	- ```ReceiverEmail```: Enter the email address where you want to receive the nba informations

## Features
- Data Scraping: Employs HtmlAgilityPack to efficiently extract data from the target website.
- Data Processing: Leverages Handlebars templating for flexible and maintainable result formatting. Define clear and well-structured templates for the email content
- Email Notification: Utilizes the System.Net.Mail library to send email notifications.