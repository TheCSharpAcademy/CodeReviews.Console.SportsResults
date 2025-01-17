using SportsNotifier;
using SportsNotifier.Models;
using SportsNotifierWorkerService;

var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.Sources.Clear();
IHostEnvironment env = builder.Environment;
builder.Configuration
    .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"), optional: true, reloadOnChange: true)
    .AddJsonFile(Path.Combine(AppContext.BaseDirectory, $"appsettings.{env.EnvironmentName}.json"), optional: true, reloadOnChange: true);
EmailDetails options = new ();
builder.Configuration.GetSection(nameof(EmailDetails)).Bind(options);
builder.Services.AddHostedService<Worker>();
builder.Services.AddTransient<INotificationService, NotificationService>();
var host = builder.Build();
host.Run();