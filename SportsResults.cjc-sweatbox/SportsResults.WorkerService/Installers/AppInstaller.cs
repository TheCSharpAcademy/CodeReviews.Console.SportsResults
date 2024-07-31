namespace SportsResults.WorkerService.Installers;

/// <summary>
/// Register the services required by the console application to the DI container.
/// </summary>
public class AppInstaller : IInstaller
{
    #region Methods

    public void InstallServices(IHostApplicationBuilder builder)
    {
        builder.Services.AddHostedService<Worker>();
    }

    #endregion
}
