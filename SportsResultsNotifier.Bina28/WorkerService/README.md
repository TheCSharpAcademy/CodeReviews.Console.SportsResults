
# Sports Results Notifier

The **Sports Results Notifier** application is a background service that scrapes 
basketball match data from the web and sends daily email notifications with the results.
The application uses Hangfire to schedule jobs and ASP.NET Core Worker Services for background processing.

---

## Features
- Scrapes basketball match results from https://www.basketball-reference.com/boxscores/.
- Sends email notifications with the scraped data.
- Runs as a background service using Hangfire for job scheduling.

---

## Getting Started

### Prerequisites
1. **.NET SDK**: Ensure you have the latest version of the .NET SDK installed.
2. **SQL Server**: Hangfire requires a SQL Server instance for job storage. Install SQL Server if not already available.
3. **Google App Password**: If using Gmail for sending emails, 
create an App Password in your Google account.

---

### Installation
1. Clone the repository to your local machine:
   
   ```bash
   git clone https://github.com/Bina28/CodeReviews.Console.SportsResults.git
Open the project in your preferred IDE (e.g., Visual Studio, Rider, or VS Code).

Update the appsettings.json file with your SQL Server connection string:

{
  "ConnectionStrings": {
    "DefaultConnection": "Your-SQL-Server-Connection-String"
  }
}
In the EmailService class, update the email credentials:

Email email = new()
{
    SmtpAddress = "smtp.gmail.com",
    PortNumber = 587,
    EnableSSL = true,
    EmailFromAddress = "your-email@gmail.com",
    Password = "your-app-password",
    EmailToAddress = "recipient-email@gmail.com",
    Subject = subject,
    Body = body
};
Important: If you are using Gmail, you must generate an App Password instead of using your
regular password. Enter the App Password in the Password field.

### Running the Application
1. Restore dependencies:
   
bash
   dotnet restore


2. Build the application:
   
bash
   dotnet build


3. Run the application:
   
bash
   dotnet run


The application will start, and Hangfire will begin processing scheduled jobs. 
By default, a job is scheduled to run every minute (for testing purposes) 
to scrape the data and send an email.

To adjust the frequency of the job, you can modify the Cron expression in the RecurringJob.AddOrUpdate method in the Worker class. 
For example, to run it daily at 8 AM:

RecurringJob.AddOrUpdate(
    "daily-email-job",
    () => _dataScraper.ScrapeData(),
    "* 8 * * *");  // Runs daily at 8 AM