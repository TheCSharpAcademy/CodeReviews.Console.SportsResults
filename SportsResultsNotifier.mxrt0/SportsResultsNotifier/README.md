# Sports Results Notifier
Simple notification app that scrapes NBA games data from the Basketball Reference website and sends it over email once per day. Developed with C# using MailKit package.

## Given Requirements
* The application should read sports data from the website once a day and send it to a specific e-mail address.
* No interaction with the program is needed. It will be a service that runs automatically.
* The data should be collected from the Basketball Reference Website.
* Agility Pack library should be used for scrapping.

## Features 
* Functional lightweight web scraper that sends emails with content neatly formatted in tables.
* No-interaction simple progress console UI.

## Challenges
* Setting up the SMTP Server
* Picking up how to send emails as a whole.
* Structuring XPath

## Lessons Learned
* Knowing HTML even on a basic level is important.

## Areas To Improve
* XPath
* Formatting HTML tables
* HTMLAgilityPack and MailKit usage

## Resources
* Basketball Reference Website
* YT Videos on how to use AgilityPack and MailKit
* GMail (for SMTP server and authentication)

## Configuration Instructions
* In the project folder is an 'email-creds.json.example'. Assuming the user is using G-Mail as a host (server provider), it suffices to replace the 'to' and 'from' addresses with the desired ones, as well as replace the 'user' with the username of the account they wish to authenticate with. Important last step is to replace the 'pass' with your own 'App Password'(a 16-letter key) which can be obtained in your G-Mail settings in 'App Passwords' (enter a name of choice and click 'Generate'). Then simply copy the contents of the .example file onto an actual .json file in the same directory.
* Optionally, the user can replace the sender/recipient display name in the SendEmail method in the EmailSender class (messages.From and messages.To respectively).
