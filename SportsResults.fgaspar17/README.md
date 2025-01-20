# Sports Results

A service to help you collect information
about basketball matches every day.
Developed using C# and Html Agility Pack.

## Given Requirements

- [x] This is an application where you should read sports data
from a website once a day and send it to a specific e-mail address.
- [x] You don't need any interaction with the program.
It will be a service that runs automatically.
- [x] The data should be collected from the Basketball
Reference Website in the resources area.
- [x] You should use the Agility Pack library for scrapping.

## Features

- Web Scraper

  - The data is scraped using the Html Agility Pack library.

- Worker
  - The application is configured to run asynchronously every 24 hours,
  - Thanks to the worker nature, you can combine it with a web application
  without blocking the main application thread.

- Sending mail

  - From the `appsettings.json` you can configure the email to send and the email to receive
  the message with the basketball results.
  - ![image](https://github.com/user-attachments/assets/39d8c73d-4e0d-41fb-9151-614c2de6cdcf)

## Challenges

- XPath to query HTML tags.
- HtmlAgilityPack library.
- Configuring the worker.
- Sending and testing email.

## Lessons Learned

- Scraping with HtmlAgilityPack.
- Configuring a worker service at the start of an application.
- Using Papercut to test the email functionality.
- XPath queries.

## Areas to Improve

- HTML and web concepts.
- HtmlAgilityPack use cases.
- Email sending options.

## Resources used

- StackOverflow posts
- ChatGPT
- [Sending Mail using FluentMail Video](https://www.youtube.com/watch?v=qSeO9886nRM&t=1030s)
- [Papercut](https://github.com/ChangemakerStudios/Papercut-SMTP)
- [HtmlAgilityPack Documentation](https://html-agility-pack.net/documentation)
- [WebScraper C# Video](https://www.youtube.com/watch?v=wbBuB7-BaXw)
- [Background Worker Services Video](https://www.youtube.com/watch?v=8Sy69b6-nj0&t=726s)
