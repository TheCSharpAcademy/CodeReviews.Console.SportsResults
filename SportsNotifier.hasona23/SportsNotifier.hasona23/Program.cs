using SportsNotifier.hasona23;
using SportsNotifier.hasona23.EmailSender;
using SportsNotifier.hasona23.Scrapper;
try
{
    var builder = Host.CreateApplicationBuilder(args);
    builder.Services.AddHostedService<SportsNotifierService>()
        .AddSingleton<EmailSender>()
        .AddSingleton<Scrapper>();
        
    var host = builder.Build();
    host.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}