# Sports Results

A service to help you collect information
about basketball matches everyday.
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

- Web Scrapper

  - The data is scrapped using the Html Agility Pack library.

- Worker
  - The application is configured to run asynchronously every 24 hours,
  thanks to the worker nature you can combine it with a web application
  without blocking the main application thread.

- Sending mail

  - From the appsettings.json you can configure the email to send and the email to receive
  the message with the basketball results.
  - ![image](papercut-image)

## Challenges

- XPath to query HTML tags.
- HtmlAgilityPack library.
- Configuring the worker.
- Sending and testing an email.

## Lessons Learned

- Scraping with HtmlAgilityPack.
- Configuring worker service at the start of an application.
- Using papercut to test the email functionality.
- XPath queries.

## Areas to Improve

- HTML and web concepts.
- HtmlAgilityPack use cases.
- Email sending options.

## Resources used

- StackOverflow posts
- ChatGPT
- More links