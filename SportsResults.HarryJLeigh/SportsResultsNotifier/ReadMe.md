# Sports Results Notifier

---

## Overview
Sports Results Notifier(basketball-reference.com) is a .NET application designed to send email notifications about basketball game results. It retrieves game results, formats them into an email-friendly structure, and sends the results to a configured recipient.

--- 

## Requirements
- This is an application where you should read sports data from a website once a day and send it to a specific e-mail address.
- You don't need any interaction with the program. It will be a service that runs automatically.
- The data should be collected from the Basketball Reference Website in the resources area. 
- You should use the Agility Pack library for scrapping.

---
## Features
- Retrieves game results using Html Agility Pack
- Formats results into an HTML email body.
- Sends email notifications using SMTP.
- Customizable email settings through appSettings.json.

---
## Prerequisites
To run this project, ensure you have the following installed:

- .NET SDK
- A valid email account with SMTP support (gmail recommended)

---
## Configuration
1. Setting Up appSettings.json
- The application uses an appSettings.json file for configuration. This file should be placed in the project root directory and include the following structure:

2. appSettings.json
- Change the following details for email settings:

1. SmtpServer: SMTP server address (e.g., smtp.gmail.com for Gmail).
2. Port: SMTP server port (usually 587 for secure email transmission).
3. SenderEmail: Email address of the sender.
4. SenderName: Name to appear in the email.
5. ReceiverEmail: Recipient's email address. 
6. Password: Password for the sender email account.
7. UseSSL: Boolean flag to enable SSL for secure email transmission.

*Note*: You may need to generate an app-specific password in your Gmail account settings and use it instead of your regular password.

---
## Code Structure
1. Program.cs: Entry point of the application.
2. EmailService.cs: Handles email sending and formatting.
3. GameResult.cs: Represents a game result model.
4. appSettings.json: Configuration file for email settings.


---
## Lessons Learned
1. How to use HTML Agility Pack to scrape data from a website.
2. How to set up settings and sending an email with the scraped data.

---
## Challenges faced
1. Understanding and Implementing the HTML Agility Pack:
- Learning to navigate and effectively utilise the HTML Agility Pack library.
2. Configuring Email Settings:
- Setting up email functionality required configuring SMTP, ensuring secure connections, and handling compatibility with email providers, including app-specific passwords.
