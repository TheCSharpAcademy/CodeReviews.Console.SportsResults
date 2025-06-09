using Microsoft.Extensions.DependencyInjection;
using SportsResults.BrozDa.Services;
using Microsoft.Extensions.Configuration;
using SportsResults.BrozDa.Models;

namespace SportsResults.BrozDa
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            Console.WriteLine(Environment.CurrentDirectory);

            var services = new ServiceCollection();

            var serviceProvider = BuildServices(services);

            var controller = serviceProvider.GetRequiredService<SportNotifierController>();

            controller.SendReport();

        }

        public static ServiceProvider BuildServices(IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var smtpSettings = config.GetSection("Smtp").Get<SmtpSettings>();

            if (smtpSettings is null)
            {
                Console.WriteLine("Invalid settings! Please contact admin");
            }

            services.AddScoped<EmailService>(
                sp => new EmailService(smtpSettings!)
                );
            services.AddScoped<ScrapingService>();

            services.AddScoped<SportNotifierController>(sp => new SportNotifierController(
                sp.GetRequiredService<ScrapingService>(),
                sp.GetRequiredService<EmailService>()
                ));

            return services.BuildServiceProvider();
        }
    }
}