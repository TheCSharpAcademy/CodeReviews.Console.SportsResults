using System.Net;
using System.Net.Mail;

namespace SportsResultNotifier
{
    public class MailService
    {
        public static void SendEmail()
        {
            while (true)
            {
                TimeUntilNext();

                Dictionary<string, string> games = WebCrawler.GetContentHtml();

                string smtpAddress = "smtp.gmail.com";
                int portNumber = 587;
                bool enableSSl = true;
                //test mail 
                string emailFromAddress = "";
                string password = "";
                string emailToAddress = "pedromila2000@gmail.com";
                string subject = games["title"];
                string body = $@"
            <h1>{games["title"]}</h1>
            <br>
            <h2>{games["games"]}<h2> ";
                for (int i = 1; i <= int.Parse(games["tableLenght"]); i++)
                {
                    body += $"<table style=\"border-collapse: collapse; border: 5px solid black;    padding: 30px\">" +
                        $"<tr style=\"border: 1px solid black\">" +
                        $"{games[$"teamOne{i}"]}" +
                        $"</tr>" +
                        $"<tr>{games[$"teamTwo{i}"]}" +
                        $"</tr>" +
                        $"</table>" +
                        $"<br>";
                }

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(emailFromAddress);
                    mail.To.Add(emailToAddress);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                        smtp.EnableSsl = enableSSl;
                        smtp.Send(mail);
                    }
                }
            }
        }

        public static void TimeUntilNext()
        {
            //here you declare what time of the day you want to receive it
            TimeSpan hour = new TimeSpan(17, 0, 0);
            DateTime now = DateTime.Now;
            DateTime nextSend = now.Date.Add(hour);
            if (now > nextSend)
            {
                nextSend = nextSend.AddDays(1);
            }
            TimeSpan timeUntilNextSend = nextSend - now;

            Console.WriteLine($"Próximo envio agendado para: {nextSend}");
            Task.Delay(timeUntilNextSend).Wait();
        }
    }
}
