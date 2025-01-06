# Sports Data Scraper and Email Sender

## Introduction

This project involves creating an application that scrapes sports data from the website once a day and sends the collected data to a specific email address. The application will run as a background service, performing both tasks automatically without user interaction. This project combines two important programming tasks: harvesting data from non-API sources and sending emails.

The data will be scraped using the **Agility Pack** library, and the email will be sent using SMTP.

## Requirements

-This is an application where you should read sports data from a website once a day and send it to a specific e-mail address.

-You don't need any interaction with the program. It will be a service that runs automatically.

-The data should be collected from the Basketball Reference Website in the resources area.

-You should use the Agility Pack library for scrapping.

## Technologies used

- Dependency Injection (DI)
- C# / .NET
- HtmlAgilityPack
- SMTP (Simple Mail Transfer Protocol)
- Task Scheduling

## Challenges
- It was hard to understand how to let service keep going.

## How to use

- In app.config file write all values that is empty(sender and password, reciever)
- In DailyTaskService.cs class write time that you want to send data once a day


