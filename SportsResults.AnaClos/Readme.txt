# Console Sports Results
## Description
In this application, sports data is read from a website once a day and sent to a specific email address.
## Google 
I had to create an app password in order to send emails from my Google account.
## Configuration
Mail settings are read from the appsettings.json file.
## Time Interval
If you want to try another time interval, go to SportsController > ExecuteAsync and modify the time span of the timer. TimeSpan.FromDays(1)
## Improvements
Create my own scrapper from a different source and with a different business case.
## Appsetting.json
I had to remove the password field to pass the tests.
