using SportsResults.WorkerService.Installers;

namespace SportsResults.WorkerService;

/// <summary>
/// Main insertion point for the worker service.
/// Configures and runs the application as a HostedService.
/// </summary>
public class Program
{
    #region Methods

    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        // Add services to the container.
        builder.InstallServicesInAssembly();

        var host = builder.Build();
        host.Run();
    }

    #endregion
}