# Sports Results Notifier


### Explanation of the README Content

- **Introduction**: Provides a brief overview of the project.
- **Requirements**: Lists the software and tools required to run the project.
- **Libraries Used**: Lists the NuGet packages used in the project.
- **Getting Started**: Step-by-step instructions for cloning the repository, installing dependencies, configuring the application, and running it.
- **How It Works**: Explains the core functionality of the project.

This README should provide clear and concise documentation for anyone who wants to understand, use, or contribute to your project.


## Introduction

Sports Results Notifier is a C# console application built with .NET 8 that scrapes sports data from a non-API source and sends it to a specific email address. This project demonstrates the ability to harvest data from websites and send emails programmatically.

## Requirements

- .NET 8 SDK
- Microsoft Outlook email accounts for sending and receiving emails

## Libraries Used

- HtmlAgilityPack
- MimeKit
- MailKit
- Microsoft.Extensions.Configuration
- Microsoft.Extensions.Configuration.Json
- Microsoft.Extensions.Configuration.Binder

## Getting Started

1. ### Clone the Repository
2. ### Install Dependencies
   Ensure you have the .NET 8 SDK installed. Then, restore the NuGet packages:

```bash 
dotnet restore 
```

3. ### Configure the Application
   Edit the appsettings.json file in the root of the project directory with the following content:

```bash
{
  "EmailSettings": {
    "SmtpServer": "smtp.office365.com",
    "SmtpPort": 587,
    "SenderEmail": "your-email@outlook.com",
    "SenderPassword": "your-email-password",
    "RecipientEmail": "recipient-email@outlook.com"
  }
}
```
Replace the placeholder values with your actual email credentials.

4. ### Build and Run the Application

Build the project:

```bash
dotnet build
```

Run the project:

```bash
dotnet run
```

## How It Works

1. Scraping Data: The DataScraper service scrapes basketball game data from the Basketball Reference website.
2. Sending Email: The EmailSender service formats the scraped data and sends it to the specified recipient via email.
3. Configuration: The ConfigurationHelper class loads email settings from the appsettings.json file.

### Note
This application has only been tested with Microsoft Outlook email accounts. Compatibility with other email providers has not been verified.