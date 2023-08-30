using AutomaticNBAResultsEmailer;
using SportsResultNotifierCarDioLogic;
using System.Globalization;

EmailService emailService = new EmailService();

TextArt.ShowImage1();

Thread.Sleep(1000);

Console.WriteLine(@"CarDioLogic's Sports Notifier App.
NBA results will now be sent.....");

Thread.Sleep(2000);

string dateOfLastSentEmailString = JsonHelpers.ReadFromJSON();
DateTime dateOfLastSentEmail;

if(dateOfLastSentEmailString == "" || dateOfLastSentEmailString == null)
{
    dateOfLastSentEmail= DateTime.Now.AddDays(-1);
}
else
{
    dateOfLastSentEmail = DateTime.ParseExact(dateOfLastSentEmailString, "yyyy/MM/dd", CultureInfo.InvariantCulture);
}

while(dateOfLastSentEmail <= DateTime.Now.AddDays(-1))
{
    string dateOfGamesToSend = dateOfLastSentEmail.ToString("yyyy/MM/dd");

    (string subject, string body) = emailService.PrepareContent(dateOfGamesToSend);
    emailService.SendEmailService(subject, body);

    Console.WriteLine($"Email with list of NBA games played on {dateOfGamesToSend} was sent.");

    dateOfLastSentEmail = dateOfLastSentEmail.AddDays(1);
    
}


dateOfLastSentEmailString = dateOfLastSentEmail.ToString("yyyy/MM/dd");
JsonHelpers.WriteToJSON(dateOfLastSentEmailString);

Console.WriteLine("All emails were sent: Lists of games played up until yesterday were sent!");
Console.WriteLine("Remeber: You can deactivate automatic emails on the App or in the Windows Task Scheduler!");

Thread.Sleep(4000);