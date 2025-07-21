# Sports Results Notifier

A .NET Console App that scrapes daily basketball game results from [Basketball Reference](https://www.basketball-reference.com/boxscores/) and emails the latest game summary to a configured recipient.

## Features

- Scrapes the most recent basketball game results.
- Sends an email with the results to the address specified in `appsettings.json`.

## Setup

1. **Clone the repository**

   ```bash
   git clone https://github.com/andymartinez1/Sports-Results-Notifier
   ```

2. **Move to the project directory:**

   ```bash
   cd Sports-Results-Notifier
   ```

3. **In your IDE, change the working directory to the current directory:**

   ```bash
   Example: "X:/C#/.NET-Console-Apps/Exercise-Tracker/Sports-Results-Notifier/"
   ```

> [!TIP]
> You can right click on the project in your IDE and copy the absolute path

4. **Configure Email Settings**

   Edit `appsettings.json`:

   ```json
   {
     "EmailService": {
       "FromEmailName": "Example Name",
       "FromEmailAddress": "example@gmail.com",
       "Password": "READ TIP BELOW",
       "ToEmailAddress": "example@gmail.com",
       "SmtpAddress": "smtp.gmail.com",
       "SmtpPort": "587"
     }
   }
   ```

> [!TIP]
> You must get a Google app password from [Google App Passwords](https://www.google.com/url?sa=t&source=web&rct=j&opi=89978449&url=https://myaccount.google.com/apppasswords&ved=2ahUKEwipuar3gM-OAxV5ElkFHRZuKfEQFnoECBcQAQ&usg=AOvVaw1rVibBR6kQTiUjqa0l_f8W)

5. **Restore dependencies:**

   ```bash
   dotnet restore
   ```

6. **Build and Run**
   ```bash
   dotnet build
   dotnet run
   ```

## How It Works

- The app fetches the latest game results from [basketball-reference.com/boxscores](https://www.basketball-reference.com/boxscores/).
- It parses the results of the last game played.
- An email with the game summary is sent to the configured recipient.

## Technologies Used

- C#
- MailKit
- HtmlAgilityPack
