# Sports Result Notifier

A script that retrieves basketball games played and sends out an email containing the scores to a specified email address.

## Running locally

**A config file will need to be set up due to privacy reasons before
the app can run.**

1. Copy + rename `SportsScraper/appsettings.example.json` -> `SportsScraper/appsettings.json`
2. Change `SportsScraper/appsettings.json` config to set data retrieval and email settings.
   - If you are using Gmail for sending emails, an app password will need to be created to send emails
     (https://support.google.com/mail/answer/185833?hl=en). Regular password will not work.
3. `cd SportsScraper`
4. `dotnet run`
5. Scripts runs
   - Latest data will be fetched
   - Data will be parsed to extract game info
   - Email template will be populated with data
   - Email will be sent out based on the `SportsScraper/appsettings.json` config.
   - Please **DO NOT** commit `appsettings.json` with your info since it contains sensitive info.

## For convenience, the program also outputs a local HTML file `SportsScraper/email-sent-{time}.html` containing the email contents that were sent out.

## A sample screenshot of the email is stored in the repo `/sample-email-screenshot.png`
