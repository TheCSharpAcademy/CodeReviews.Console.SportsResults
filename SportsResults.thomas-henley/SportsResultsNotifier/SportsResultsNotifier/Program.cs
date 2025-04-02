using Microsoft.Extensions.Configuration;
using SportsResultsNotifier;

Console.WriteLine("Starting up...");

var builder = new ConfigurationBuilder();
builder.AddJsonFile("appsettings.json");
var config = builder.Build();

// The email, app password, and subscriber address are pulled from appsettings.json.
var emailClient = new EmailClient(
    config["Email"] ?? "", 
    config["AppPassword"] ?? "",
    config["Subscriber"] ?? "");

// The URL is configurable in appsettings.json, although the scraper is designed around this specific endpoint.
var url = config["Url"] ?? "http://html-agility-pack.net/";
var scraper = new BasketballScraper(emailClient, url);

// The "LastFired" DateTime is how the updater thread knows if an update has been sent for the day.
// Initializing LastFired to yesterday means the program will send an update out immediately on start.
object stateInfo = new TimerState() { LastFired = DateTime.Now.AddDays(-1) };

// Start the timer thread to get and send out score updates. It will wake up every hour to check if it's time
// to send out a new update. The hour is pretty arbitrary, since the update only goes out once a day.
// When the thread wakes, it checks the current date against the date of the last update.
// If an update hasn't happened since yesterday, it sends a new one out.
var timer = new Timer(
    callback: scraper.CheckForScoreUpdate,
    state: stateInfo,
    dueTime: 0,
    period: 1000 * 60 * 60);

// Pressing "Enter" at the console will end the updater program.
Console.ReadLine();
timer.Dispose();
