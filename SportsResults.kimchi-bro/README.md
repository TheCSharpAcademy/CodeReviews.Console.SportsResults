## Setup
#### appsettings.json
- Set values for SMTP server properties in `"Smtp"` object:
    - SMTP server: `"Server"`
    - Port number: `"Port"`
    - Sender email address: `"EmailFrom"`
    - Receiver Email address: `"EmailTo"` (you can use same email address to send and receive emails)
    - Sender password in `"Password"` (you may need to generate separate password to use it in external application, check your SMTP server configuration)
- Set `"EmailSendingTime"` one or two minutes past current time in:
    - Sending hour `"Hours"`
    - Sending minutes `"Minutes"`
- Test the app running it in `Debugging` mode
- If email was successfully sent and received, set preferred `"EmailSendingTime"`
#### Service registration
- Publish the app to preferred location as self-contained single `.exe` file
- Run **Windows Terminal as Administrator**
- Create new service with command:
    ```PowerShell
    sc.exe create ".NET Sports Results Emailing Service" binpath= "C:\Path\To\SportsResultsService.exe"
    ```
    - You can name the service whatever you like, make sure filepath to app's `.exe` file is correct
- Check if service was created in **Services** (search for `Services` in Start menu)
    - You should see `.NET Sports Results Emailing Service` in services list
#### Service start
- Start service with command:
    ```PowerShell
    sc.exe start ".NET Sports Results Emailing Service"
    ```
    - `.NET Sports Results Emailing Service`'s status should change to `Running`
- Check logs in **Event Logger** (search for `Event Viewer` in Start menu, select **Windows Logs > Application** node), you should see that `Application started` in **Event Properties** for `Sports Results Emailing Service` Source
- The service works and sends you emails with sports results once a day at a certain time
#### Service stop
- Run command in terminal:
    ```PowerShell
    sc.exe stop ".NET Sports Results Emailing Service"
    ```
#### Service delete
- Run command:
    ```PowerShell
    sc.exe delete ".NET Sports Results Emailing Service"
    ```
## Features
- An application reads sports data from a website once a day and sends it to a specific email address
- An application is a service that runs automatically
- HtmlAgilityPack library for scrapping
- MailKit for email sending