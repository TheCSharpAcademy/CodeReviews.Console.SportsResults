# NBA Sports Results Email Service

A simple console application that scrapes NBA game results and sends them via email daily.

## Features

- **Web Scraping**: Scrapes NBA game results from basketball-reference.com
- **Email Notifications**: Sends formatted game results via email
- **Daily Scheduling**: Automatically sends emails every 24 hours
- **Configuration**: Uses appsettings.json for email configuration

## Setup

### 1. Prerequisites
- .NET 9.0 or later
- Valid email account (Gmail recommended)

### 2. Configuration
1. Copy `appsettings.sample.json` to `appsettings.json`
2. Update email settings in `appsettings.json`:

```json
{
  "Mail": {
    "From": "your-email@gmail.com",
    "To": "recipient@gmail.com", 
    "Host": "smtp.gmail.com",
    "Email": "your-email@gmail.com",
    "Subject": "Daily NBA Results",
    "Password": "your-app-password",
    "Port": 587,
    "EnableSsl": true
  }
}
```

### 3. Installation
```bash
# Clone the repository
git clone <your-repo-url>
cd SportResults.yemiodetola

# Restore packages
dotnet restore

# Build the project  
dotnet build
```

## Usage

```bash
# Run the application
dotnet run
```

## Project Structure

```
SportResults.yemiodetola/
├── Services/
│   └── ScraperService.cs    # Web scraping logic
├── Models/
│   └── Result.cs            # Game result model
├── Helpers/
│   └── MailHelper.cs        # Email formatting
├── Mailer.cs                # Email service
├── Program.cs               # Main application
└── appsettings.json         # Configuration
```

## Dependencies

- **HtmlAgilityPack** - For web scraping
- **Microsoft.Extensions.Configuration** - For configuration management
- **System.Net.Mail** - For email functionality