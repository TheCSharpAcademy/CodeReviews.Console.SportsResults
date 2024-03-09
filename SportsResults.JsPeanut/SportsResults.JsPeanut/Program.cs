using HtmlAgilityPack;
using System.Net;
using System.Net.Mail;

namespace SendMail
{
    class Program
    {
        static string smtpAddress = "smtp.gmail.com";
        static int portNumber = 587;
        static bool enableSSL = true;
        static string emailFromAddress = ""; //Sender Email Address  
        static string password = ""; //Sender Password  
        static string emailToAddress = ""; //Receiver Email Address
        static void Main(string[] args)
        {
            string mailContent = ExtractDataToSend();
            SendEmail("NBA Results and Standings", mailContent);
        }
        public static void SendEmail(string subject, string body)
        {
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
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                }
            }
        }
        public static string ExtractDataToSend()
        {
            HtmlWeb web = new();
            HtmlDocument document = web.Load("https://www.basketball-reference.com/boxscores/");

            string conferenceStandings = "";
            var title = document.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[2]").OuterHtml;
            var NBAGameInfo =  document.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[3]/div").ToList().First().OuterHtml;
            var conferenceStandings_ = document.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[4]");
            conferenceStandings_.ToList().ForEach(i => conferenceStandings += i.OuterHtml);

            string mailContent = title + NBAGameInfo + conferenceStandings;

            return mailContent;
        }
    }
}
