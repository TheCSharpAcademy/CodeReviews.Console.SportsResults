# Sport Results Notifier
_by __smkP13__ for __thecsharpacademy___

## Introduction
This project is made to learn how to send email via an appliication using C#.
Three method are used to fulfill it.

## Requirements
- This is an application where you should read sports data from a website once a day and send it to a specific e-mail address.

- You don't need any interaction with the program. It will be a service that runs automatically.

- The data should be collected from the Basketball Reference Website in the resources area.

- You should use the Agility Pack library for scrapping.

## Structure
The App consist of a simple menu where the user can:
- Set the type of email used and necessary values(like Application Password)
- Send Sport results using diffrent methods(Local directory,Papercut SMTP and Gmail SMTP)

The structure is a simple MVC structure with services to make the link between the UI and the controllers.<p>
The application is fetching data from a website(HTML) and format them to be presented send as an email.
As the program is small, email and user controllers/services are made into single files.

## Thoughts and Remarks
Local emails are sent as .eml and require a .eml reader to see the uncrypted results<p>
Basic HTML is not hard to read but it can be confusing to find the right element to use and to parse it correctly.<p>
The Gmail SMTP requires the user to first create an Application Password to allow the App to send mail using it.<p>
Other SMTP can be usedmand may require other elements to send emails(such as tokens).<p>
I learned that email are protected in various ways to prevent them to be used by unthrustworthy users.

### Ressources
- Html Agility Pack (HAP) : [link](https://html-agility-pack.net/)
- Send Email Using Gmail SMTP :[link](https://www.c-sharpcorner.com/blogs/send-email-using-gmail-smtp)
- C# Html Reader | C# HTML Parser | HTMLAgilityPack : [video](https://www.youtube.com/watch?v=oMM0yzyi4Do)
- How to make a FAST WebScraper C# : [video](https://www.youtube.com/watch?v=wbBuB7-BaXw)
- Basketball References (fetched data) : [link](https://www.basketball-reference.com/boxscores/)
- Papercut-SMTP :[link](https://github.com/ChangemakerStudios/Papercut-SMTP)
- How to send emails from C#/.NET - The definitive tutorial : [link](https://blog.elmah.io/how-to-send-emails-from-csharp-net-the-definitive-tutorial/)