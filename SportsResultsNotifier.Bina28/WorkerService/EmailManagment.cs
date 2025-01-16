using System.Net.Mail;
using System.Net;

namespace WorkerService;

internal class EmailManagment
{
	public static void SendEmail(Email email)
	{
		using (MailMessage mail = new MailMessage())
		{
			mail.From = new MailAddress(email.EmailFromAddress);
			mail.To.Add(email.EmailToAddress);
			mail.Subject = email.Subject;
			mail.Body = email.Body;
			mail.IsBodyHtml = true;

			using (SmtpClient smtp = new SmtpClient(email.SmtpAddress, email.PortNumber))
			{
				smtp.Credentials = new NetworkCredential(email.EmailFromAddress, email.Password);
				smtp.EnableSsl = email.EnableSSL;

				smtp.Send(mail);
			}
		}
	}
}
