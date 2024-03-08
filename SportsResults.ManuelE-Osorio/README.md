# Sports Results Notifier

The app is an implementation of a web crawler to notify via email the daily basketball scores. The app uses the packages
HtmlAgilityPack for the data extraction from the web and MailKit for creating the SMTP client and sending the email.

## Usage

The user has to configure their own appsettings.json before running the app. The settings are as follow:

1. LastScrappedDate: The last date the results were stored in the web, if it hasn't been run its better to leave it as null. It spects a date format "yyyy/MM/dd"
2. LastRunDate: The last date the program was run. If it hasn't been used previosly it's better to leave it as null. It spects a data format "yyyy/MM/dd".
3. SourceEmail: The email address that will send the results.
4. SourceEmailPassword: Source email address.
5. DestinationEmail: The email address that will receive the results.
6. Host (optional): Host smtp address, by default it uses outlook smtp address smtp-mail.outlook.com
7. Port (optional): SMTP server port, by default it uses outlook default port 587
8. WebPage (optional): Web page to crawl. Currently it only works with the following webpage: https://www.basketball-reference.com/boxscores/

After the appsettings.json has been configurated, the user can run the app and will receive the results in their destination email.

## To do

1. Implement diferent webpages
2. Add encryption to the user password

## References

1. <https://stackoverflow.com/questions/35349414/how-to-use-a-default-value-for-json-net-for-properties-with-invalid-values>
2. <https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/how-to-cancel-a-task-and-its-children>
3. <https://dotnettutorials.net/lesson/how-to-create-synchronous-method-using-task-in-csharp/>
4. <https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/delegates/>
5. <https://devblogs.microsoft.com/pfxteam/should-i-expose-asynchronous-wrappers-for-synchronous-methods/>
6. <https://stackoverflow.com/questions/73449652/how-to-encrypt-decrypt-connection-string-in-appsettings-json>
7. <https://learn.microsoft.com/en-us/dotnet/api/system.configuration.configuration?view=dotnet-plat-ext-8.0>
8. <https://stackoverflow.com/questions/59274418/add-appsettings-configuration-di-to-hostbuilder-console-app>
