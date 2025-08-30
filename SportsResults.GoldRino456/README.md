# Sports Results Notification App

A Windows service that runs in the background, scrapes basketball game results from https://www.basketball-reference.com/boxscores once a day, then sends that information in an email via SMTP.
## Features

- Accesses the live webpage on https://www.basketball-reference.com/boxscores each day to fetch the most up-to-date scores. Results are then stored and formatted to be sent over email!
- Allows the user to connect the SMTP server of their choice, including gmail, and set a desired destination address for the daily updates.



## Tech Stack

**Runtime & Framework:** .NET 8

**Web Scraping Utility:** HTML Agility Pack


## Lessons Learned

- I've used Python for scraping web pages in a previous project, so I feel having some familiarity with that helped me pick up HTML Agility Pack a bit quicker. I see a lot of the same limitations here that I ran into with that other project, in that if the HTML structure of the web page changes it would break this code or if it were to load data more dynamically this approach likely wouldn't work at all. This is a good start with web scraping in C#, but for a larger project I think I would look at other existing libraries (depending on the target website though).

- Testing locally with Papercut SMTP from the start was absolutely the best thing to do here. Though I did switch and test with gmail SMTP near the end of development, sticking with Papercut helped me figure out how I wanted to format the email's body and make sure my logic for catching errors and sending email at all was functional. Had I started out with gmail, I'd have to worry more about their filters and the rate limit among other things. Much easier to switch to gmail near the end and make minor corrections as needed to work with their service.

- This project was my first time actually creating a service within Windows to run in the background, but thankfully Microsoft has some fairly straight forward documentation explaining how to create one. When it came time to set up the background service, this part ended up more or less just requiring me to click in my already existing web scraper and email logic.
## Acknowledgements

 - [The C# Academy](https://www.thecsharpacademy.com/)
 - [README Editor](https://readme.so/editor)

