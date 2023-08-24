C# Academy project - Sports Results Notifier

Objective/Purpose:
------------------
The main requirement for this was to scrappe/Read information from the basketball reference website (https://www.basketball-reference.com/boxscores/), once a day, by using the Agility Pack library and to send that data by email automatically (Meaning: without user interaction the data would be scrapped and sent once a day).


How to use:
-----------
	1. The user should use the UI and select "SetUserConfigs".

	2.Then write a valid email and a password, and both will be saved to a config file locally ("NBAconfig.JSON" localled in the 	bin folder of the UI project).

Keep in mind: the password saved to the file in an encrypted format. And the key is hardcoded in the source code! The project is only meant to be used locally, but once you longer want to use the project CLEAR THE USER CONFIGS!

You can clear email "ClearUserConfigs" configs with the option.

	3. You can choose the option to get and send the results of a specific date by choosing "SendEmail".

	4. Or you can choose to activate the automatic email feature. That will create a task in the Windows Task Scheduler that 		will execute the second project to send an email everyday at 12pm of the games played on the previous day. The email configs 	need to be properly defined or the automatic emailer wont work!

Notes:
Files like the JSON config files or the executable of the second project should not be moved (inside the solution project) or the app will stop working! Also do not rename the files!



Building the App:
----------------
To achieve that purpose, the scrapper was first created (it would get return a list of all the "nodes" in a page(date) that represented all the games played on a given date and it would receive a date that would be combined with the base URL of the page).

That data would be then deserialized and list of objects/Games would be constructed (each game had 4 properties: names and scores of each team in the game). After, the content for the email was prepared, basically looping trough each object/game in the list and passing and incrementing the information into a single string that would be the body of the email. The subject of the email was also built and took into consideration the date that was passed.

The ideia was that, for each day a list of all the games played on that day would be scrapped from the site and sent by email.

Then the logic to send emails was built. It supports two domains (hotmail.com and gmail.com). Though i have only tested with hotmail.com. I also decided to create an config file that would save the email and password of the user locally, so that it can be used troughout sessions and not be lost when runtime is over and (more importantly) to enable the app to get that info when it is outside of its runtime so that it is possible to send emails automatically when the app is not running. 
Also, to provide a very basic layer of protection (or sense of security at least) i added a method to encrypt the password before saving it to the config file (and when it needs it, it will decrypt the password during runtime and use it). But the key to decrypt/encrypt is hardcoded right in the source code, which is really not ideal/recomended for security reasons and should not be done (But the APP IS INTENTED TO BE USED LOCALLY ONLY and is within the scope of a learning project meant to test my capabilities - STILL IT IS ADVISED TO CLEAR THE CONFIG FILE ONCE YOU NO LONGER INTENDED TO USE THE APP - theres an option for that in the app UI). About the matter of security, ideially the key should be generated randomly and stored away from prying eyes, as the same goes for the encrypted password (in separate locations).

To automate the procces of sending an email once a day with the results of all the games from the previous day, i used the nugget package "TaskScheduler" (by David Hall). This allowed me to create Tasks in the Windows Task Scheduler and define the time triggers and the filepath to the file that it would execute at that specific time. So, it was defined that everyday at 12pm a file (with access to all the logic it needs to send the email) will be executed. But, if the machine is not turned on at 12pm, windows wont be able to execute that task/file and it won't send the email automatically, but it will store the date of the last email sent (during previous runtimes) and it will use that to send emails up until the current date, presenting the games played on each day. The automatic email service will keep running forever until it is deactivated trough the App UI or on the Windows Task Scheduler by removing the task that executes the file. The automatic email sender feature is disabled by default.

So, basically the solution has 2 projects:
- Firstly, The aplication, requires the user to use the 1st project (which has UI) to set up his email configurations so that the app can use them to send the emails. This is the main project that the user can interact with to set its configurations and to activate or disable the automatic emails feature.

-The second project, does not require any interation at all, it runs preciselly everyday at 12pm (if the automatic email feature is enabled) and it sends all the emails it needs to send in just a runtime. 


Personal insights and Challenges taken from the project:
-------------------------------------------------------

I put alot of effort into this simple project and I had to consider a few options to achieve what i wanted in the end, a few of those options i discarded (either because they were not right for what i wanted in the end or because i simply could not get them to work), i also spent quite some time considering my options and trying to implement those option to, in the end, decide to go in another direction. Although its probably  best to stick to an option and try to make it work, i think its equally good to know when to try another direction.
To be more specific, in the part where i wanted to find a way on how to automate the email sending feature i came across several alternatives (some on the cloud and some local, there was different ways to achieve it), and i initially decided to try to go in the direction of creating an azure function that was stored in the cloud and would run the code online to send the email automatically each day at a specific set time (that was the initial ideia). First, I had to get it to work locally (for testing), which i spent a considerable ammount of time trying to do... But in the end, i stratched that option simply because that i though that for my app it would't work to because i would have to store the email user configs also in the cloud so that the function would be able to access them and send the email correctly (in the way i had it set up, there was no way that would be aceptable in regards of security, since the email and password were stored in the file and the key to decrypt and encrypt the password are hardcoded in the source code of the 1st project). There may have been of way of doing it in an aceptable manner, but with my experience i though that persuing this option further just for the sake of security would have required even more time and effort that are even outside of the scope and objectives of this learning project. Although it would have been a good opportunity to explore and learn some more, i decided to leave it for a bit later and focus on the objectives of the project.
So my next option, was creating a worker function. It would work fine and did what i wanted if i ran it in debug mode, but when it came time to deploy, install and active the service. It failed in the part of activating the service (and gave me an error that i just could not find a solution for after searching). So, i finally decided to go with the TaskScheduler library to do the job..
I dont regret spending this time trying this options (even if they did not work for me), i dont consider it wasted time, it was actually an introduction to them and maybe i will use them later in another projects. 
Each option and every decision came with its challenges (like trying to get 2 projects in the same solution to use the same JSON file) and its important to try to overcome them and be a bit stuborn.


Resources used:
---------------
	Nugget Packages:
	-Agility Pack library
	-Spectre.Console
	-TaskScheduler (by David Hall)
	-Newtonsoft.Json