using System.Net;
using System.Net.Mail;

namespace SportsResults
{
	internal class Mail
	{
		public static void SendMail()
		{
			string smtpAddress = "smtp.gmail.com";
			int portNumber = 587;
			bool enableSSL = true;
			string emailFromAddress = "furiaxtest@gmail.com";
			string password = "";
			string emailToAddress = "carlmalfliet@proximus.be";
			string subject = "Sport results";
			string body = "Hello, this is a test email. Soon this will contain the sport results";
			using (MailMessage mail = new MailMessage())
			{
				mail.From = new MailAddress(emailFromAddress);
				mail.To.Add(emailToAddress);
				mail.Subject = subject;
				mail.Body = body;
				mail.IsBodyHtml = true;
				try
				{
					using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
					{
						smtp.Credentials = new NetworkCredential(emailFromAddress, password);
						smtp.EnableSsl = enableSSL;
						smtp.Send(mail);
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error mail not send: {ex.Message}");
				}

			}
		}
	}
}
