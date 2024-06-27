# Sports Results Notifier

## Project Description

This application runs as background service, scrap data from a
[NBA Basketball](https://www.basketball-reference.com/boxscores/)
and send an email every midnight at 00:00:00. To scrap the data we used
[Html Agility Pack (HAP)](https://html-agility-pack.net/).  
This Application is Part of Console Application Project
at [CSharpAcademy](https://thecsharpacademy.com/project/15/drinks).

## The Application Requirements

* This is an application where you should read sports data from a website
once a day and send it to a specific e-mail address.
* You don't need any interaction with the program. It will be a service
that runs automatically.
* The data should be collected from the Basketball Reference Website in
the resources area.
* You should use the Agility Pack library for scrapping.

## How to run the application

[Microsoft Secrets Manager](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0&tabs=linux)
is used to store the email credentials.  
Before running the app please set the below email configurations

* dotnet user-secrets set EmailSettings:SenderEmail your-email@
* dotnet user-secrets set EmailSettings:SenderPassword your-password
* dotnet user-secrets set EmailSettings:RecipientEmail recipient-email@
* dotnet user-secrets set EmailSettings:SmtpHost smtp.example.com
* dotnet user-secrets set EmailSettings:SmtpPort 587

By default the application will send an email at 00:00:00 everyday. To
change this behaviour please change the below code in
SportsResultBackgroundService.cs file as below

```csharp
var currentTime = DateTime.Now;
var nextRunTime = DateTime.Today.AddDays(1).AddHours(0).AddMinutes(0).AddSeconds(0);
var timeToWait = nextRunTime - currentTime;

if (timeToWait < TimeSpan.Zero)
{
    nextRunTime = nextRunTime.AddDays(1);
    timeToWait = nextRunTime - currentTime;
}
```

For example to run the app every one minute

```csharp
var currentTime = DateTime.Now;
var nextRunTime = DateTime.Now.AddMinutes(1);
var timeToWait = nextRunTime - currentTime;

if (timeToWait < TimeSpan.Zero)
{
    nextRunTime = nextRunTime.AddMinutes(1);
    timeToWait = nextRunTime - currentTime;
}
```
