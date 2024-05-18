using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Study.ConsoleP.Sports
{
    class Progam
    {
        static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
              .ConfigureServices((hostContext, services) =>
              {
                  services.AddHostedService<Worker>();
              });

            await builder.RunConsoleAsync();
        }
    }
    public class Worker : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var scraper = new StaticWebScrapper();
                    var url = "https://www.basketball-reference.com/boxscores/";
                    var results = scraper.GetResults(url);

                    string fromEmail = "duddnwkfgo7@gmail.com";
                    string appPassword = "zshr ytec fgau ztqd";

                    string toEmail = "kevinleea1b2c2@gmail.com";

                    string subject = "NBA Results";
                    string body = "NBA Results:\n\n" + JsonConvert.SerializeObject(results, Formatting.Indented);

                    var smtpClient = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        Credentials = new NetworkCredential(fromEmail, appPassword),
                        EnableSsl = true,
                    };

                    var message = new MailMessage(fromEmail, toEmail, subject, body);

                    smtpClient.Send(message);
                    Console.WriteLine("Email sent successfully!");

                    await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // Change delay to 1 minute
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }        
}