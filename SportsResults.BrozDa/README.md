# Sports Results Notifier project by C# Academy

## Project Overview:

This application automatically scrapes daily sports results from a website and sends them via email. It is designed to run as a background service with no user interaction required.

Project link: [Sports Results Notifier](https://www.thecsharpacademy.com/project/19/sports-results)

## Project Requirements:
- This is an application where you should read sports data from a website once a day and send it to a specific e-mail address.
- You don't need any interaction with the program. It will be a service that runs automatically.
- The data should be collected from the Basketball Reference Website in the resources area.
- You should use the Agility Pack library for scrapping.

## Lessons Learned:
- Gained a solid understanding of static web scraping and mapping results to models.
- Learned how XPath works and how to adjust it to target the correct HTML elements.
- Learned how to send emails from a C# application using SmtpClient.

## Areas for improvement:
- Implement automatic daily email sending (every 24 hours).
- current smtp setting works only for temporary email addresses and needs to be adjusted to contain authentication details etc.

## Main resources used:
[HTML Agility pack documentation](https://html-agility-pack.net/documentation)

[Free SNMP server](https://www.wpoven.com/tools/free-smtp-server-for-testing)

## Packages Used
| Package | Version |
|---------|---------|
| HtmlAgilityPack | 1.12.1
| Microsoft.Extensions.DependencyInjection | 9.0.5 |                                                                 
| Microsoft.Extensions.Configuration | 9.0.5 |
| Microsoft.Extensions.Configuration.Json | 9.0.5 |
| Microsoft.Extensions.Configuration.Binder | 9.0.5 |
