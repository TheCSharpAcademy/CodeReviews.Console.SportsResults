using Microsoft.Extensions.DependencyInjection;
using SportsResults.BrozDa.Services;
namespace SportsResults.BrozDa
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
           
            var serviceProvider = BuildServices(services);


            var controller = serviceProvider.GetRequiredService<SportNotifierController>();

            controller.SendReport();

        }
        public static ServiceProvider BuildServices(IServiceCollection services)
        {
            services.AddScoped<EmailService>(
                sp => new EmailService(
                    "smtp.freesmtpservers.com", 
                    25, 
                    "yedaked211@eduhed.com", 
                    "sportresults@test.com"
                    )
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
