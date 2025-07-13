Sports Results & Event Scraper

# A C# web scraping application that monitors and notifies users about:
# - Basketball game results
# - Halestorm concert dates and events

# Requirements
# 1. This is an application where you should read sports data from a website once a day and send it to a specific e-mail address.
# 2. You don't need any interaction with the program. It will be a service that runs automatically.
# 3. The data should be collected from the Basketball Reference Website in the resources area.
# 4. You should use the Agility Pack library for scraping.

# Features

# - Automated web scraping with configurable timer intervals
# - Email notifications for new updates
# - Command-line interface with simple flags
# - Configurable settings via appsettings.json

# Getting Started

# Prerequisites

# - .NET Core SDK
# - Valid email configuration (for notifications)

# Installation

# 1. Clone the repository:
# 2. git clone [https://github.com/ryanw84/Sports-Results-aka-WebScraper.git](https://github.com/ryanw84/Sports-Results-aka-WebScraper.git)
# 3. Configure the application settings in `appsettings.json`
# 4. Set up your email credentials in user secrets

# Usage

# Run the application with one of the following flags:
## For basketball results
# dotnet run -- -b
##For Halestorm events
# dotnet run -- -h

# Project Structure

# - `UI/` - User interface components
# - `Models/` - Data models
# - `Service/` - Core services including scrapers and email functionality
# - `Program.cs` - Application entry point

# Configuration

# The application uses `appsettings.json` for configuration and user secrets for sensitive data like email credentials.

# Nuget Packages
# - DotNetSeleniumExtras.WaitHelpers
# - HtmlAgilityPack
# - MailKit
# - Microsoft.Extensions.Configuration
# - Microsoft.Extensions.Configuration.Json
# - Microsoft.Extensions.Configuration.UserSecrets
# - Microsoft.Extensions.DependencyInjection
# - Microsoft.Extensions.Hosting
# - MimeKit
# - Selenium.WebDriver
# - Spectre.Console

# Sources
# - https://www.thecsharpacademy.com/project/19/sports-results
# - https://html-agility-pack.net/online-examples
# - https://dotnetfiddle.net/j808HD
# - https://www.c-sharpcorner.com/blogs/send-email-using-gmail-smtp
# - https://www.youtube.com/watch?v=B6I2VGB9LpM
# - https://www.youtube.com/watch?v=wbBuB7-BaXw
# - https://www.youtube.com/watch?v=5wCADuDnj7A
# - https://www.basketball-reference.com/boxscores/
# - https://webscraping.ai/faq/html-agility-pack/can-i-use-html-agility-pack-to-extract-data-from-tables-in-an-html-document
# - https://www.zenrows.com/blog/html-agility-pack#extract-a-single-element
# - https://www.halestormrocks.com/#tour
# - https://saucelabs.com/resources/blog/getting-started-with-webdriver-in-c-using-visual-studio

# Credits
# - Pablo DeSouza - for mentoring
# - The C Sharp Academy - for the learning resources

# Contact
# ryanweavers@gmail.com
