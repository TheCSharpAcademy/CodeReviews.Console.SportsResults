using System.Text.Json;
using Quartz;
using Quartz.Impl;
using SportsResults.cacheMe512;

class Program
{
    static async Task Main(string[] args)
    {
        bool isDebugMode = LoadDebugSetting();
        await ScheduleJob(isDebugMode);
    }

    private static async Task ScheduleJob(bool isDebugMode)
    {
        StdSchedulerFactory factory = new StdSchedulerFactory();
        IScheduler scheduler = await factory.GetScheduler();
        await scheduler.Start();

        IJobDetail job = JobBuilder.Create<ScraperJob>()
            .WithIdentity("scraperJob", "group1")
            .Build();

        ITrigger trigger;

        if (isDebugMode)
        {
            trigger = TriggerBuilder.Create()
                .WithIdentity("testTrigger", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(10).RepeatForever())
                .Build();

            Console.WriteLine("Running in DEBUG mode. Jobs will run every 10 seconds.");
        }
        else
        {
            trigger = TriggerBuilder.Create()
                .WithIdentity("normalTrigger", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInHours(24).RepeatForever())
                .Build();

            Console.WriteLine("Running in NORMAL mode. Jobs will run every 24 hours.");
        }

        await scheduler.ScheduleJob(job, trigger);

        Console.WriteLine("Scheduler started. Press [Enter] to stop.");
        Console.ReadLine();
    }

    private static bool LoadDebugSetting()
    {
        string configPath = "appsettings.json";
        if (!File.Exists(configPath))
        {
            Console.WriteLine("Configuration file not found. Defaulting to normal mode.");
            return false;
        }

        string json = File.ReadAllText(configPath);
        var config = JsonSerializer.Deserialize<AppConfig>(json);

        return config?.Debug ?? false;
    }
}
