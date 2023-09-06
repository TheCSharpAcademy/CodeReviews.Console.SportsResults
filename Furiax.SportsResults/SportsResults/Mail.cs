using System.Net;
using System.Net.Mail;

namespace SportsResults
{
	internal class Mail
	{
		internal static void SendMail(string date, List<GameModel> games)
		{
			string smtpAddress = "smtp.gmail.com";
			int portNumber = 587;
			bool enableSSL = true;

			// fill in the emailFromAddress, password and emailToAddress before using
			string emailFromAddress = "";
			string password = "";
			string emailToAddress = "";
			string subject = "Sport results";
			string body = $"<p>{date}</p>";

			body += "<hr />";

			foreach (GameModel game in games)
			{
				body += $"<p>{game.HomeTeam} vs {game.AwayTeam}: {game.HomeScore} - {game.AwayScore}</p>";
			}

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
