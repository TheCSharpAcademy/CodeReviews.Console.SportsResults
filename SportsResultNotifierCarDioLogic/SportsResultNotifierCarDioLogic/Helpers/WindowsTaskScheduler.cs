using Microsoft.Win32.TaskScheduler;

namespace SportsResultNotifierCarDioLogic.Helpers;

static public class WindowsTaskScheduler
{
    static string taskName = "Send Email NBA results";

    static public void CreateTask()
    {
        TaskDefinition td = TaskService.Instance.NewTask();

        string solutionPath = NBAconfigHelpers.GetSolutionFolderPath();

        td.RegistrationInfo.Author = "CarDioLogic";

        td.RegistrationInfo.Description = "Send email";

        td.Actions.Add(new ExecAction(solutionPath + @"\AutomaticNBAResultsEmailer\bin\Debug\net7.0\AutomaticNBAResultsEmailer.exe"));

        TimeTrigger tt = new TimeTrigger();
        if (DateTime.Now.Hour < 12)
        {
            tt.StartBoundary = DateTime.Today.Add(TimeSpan.FromHours(12));
        }
        else
        {
            tt.StartBoundary = DateTime.Today.AddDays(1).Add(TimeSpan.FromHours(12));
        }
        tt.Repetition.Interval = TimeSpan.FromDays(1);
        tt.Repetition.Duration = TimeSpan.Zero;
        td.Triggers.Add(tt);

        TaskService.Instance.RootFolder.RegisterTaskDefinition(taskName, td);
    }

    static public void DeleteTask()
    {
        var folder = TaskService.Instance.RootFolder;

        folder.DeleteTask(taskName);
    }
}