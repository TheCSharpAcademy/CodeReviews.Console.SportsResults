# SportsResults

SportsResults is a C# application that scrapes sports data from a specified website, processes the information, and sends daily email updates with the latest sports results.

## Features

- Web scraping of sports data
- Automated daily email notifications
- Configurable settings via appsettings.json
- Background service for scheduled execution

## Project Structure

- `Controllers/`: Contains the ScraperController for orchestrating the scraping and email processes
- `Models/`: Defines data models used in the application
- `Services/`: Implements core functionalities like scraping and email sending
- `Utilities/`: Houses utility classes for configuration reading and HTML parsing

## Getting Started

### Prerequisites

- .NET 6.0 or later
- SMTP server access for sending emails

### Configuration

1. Clone the repository
2. Navigate to the project directory
3. Update the `appsettings.json` file with your specific settings:

```json
{
  "Scraping": {
    "WebsiteUrl": "https://www.basketball-reference.com/boxscores/"
  },
  "Email": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "Password": "your-email-password",
    "FromAddress": "sender@example.com",
    "ToAddress": "recipient@example.com"
  }
}
```

## Running the application
1. Open a terminal in the project directory
2. Run the following command: `dotnet run`

The application will start and run as a background service, scraping data and sending emails daily.

## Key Components

* ScraperController: Orchestrates the scraping and email sending processes
* ScraperService: Handles the web scraping functionality
* EmailService: Manages email composition and sending
* HtmlParser: Parses the scraped HTML to extract relevant sports data
* ConfigReader: Reads configuration settings from appsettings.json
* WorkerService: Implements the background service for scheduled execution

## Customization

1. To change the scraping frequency, modify the delay in `WorkerService.cs`
2. To scrape from a different website, update the `WebsiteUrl` in `appsettings.json` and adjust the `HtmlParser` accordingly.