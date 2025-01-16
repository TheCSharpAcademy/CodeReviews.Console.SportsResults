using HtmlAgilityPack;
using System.Text;

namespace WorkerService.Services;

public class EmailService
{
	public static void SendEmail(string subject, string body)
	{
		try
		{
			Email email = new()
			{
				SmtpAddress = "smtp.gmail.com",
				PortNumber = 587,
				EnableSSL = true,
				EmailFromAddress = "dyakovabina@gmail.com",
				Password = "kstp iktc zods dlvf",
				EmailToAddress = "dyakovabina@gmail.com",
				Subject = subject,
				Body = body
			};

			EmailManagment.SendEmail(email);
			Console.WriteLine("Email sent successfully.");
		}
		catch (Exception ex)
		{
			File.AppendAllText("error.log", $"{DateTime.Now}: {ex.Message}\n{ex.StackTrace}\n");
		}
	}
}

