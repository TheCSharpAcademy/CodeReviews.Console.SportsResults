# Sports Results Notifier

Sports Results Notifier is a C# console application that scrapes basketball results from [Basketball Reference](https://www.basketball-reference.com/) once a day and sends them by email automatically.  
It uses **Html Agility Pack** for web scraping and **System.Net.Mail** for sending emails.

---

## Features

- Scrapes the latest basketball game results from Basketball Reference.
- Sends the results to a configured email address.
- Modular code structure for easy maintenance.

---

## Configuration

This project requires an appsettings.json file in the root directory.
This file is not included in the repository for security reasons.

Create a file named appsettings.json with the following structure:

```json
{
  "Smtp": {
    "Host": "smtp.example.com",
    "Port": 587,
    "EnableSsl": "true",
    "User": "example@gmail.com",
    "Password": "examplePass",
    "From":  "example@gmail.com"
  }
}
```