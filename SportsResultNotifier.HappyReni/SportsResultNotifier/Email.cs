using Microsoft.Extensions.Configuration;
using SportsResultNotifier.Model;
using System.Net;
using System.Net.Mail;

namespace SportsResultNotifier
{
    public class Email
    {
        private Dictionary<string, string> Prop { get; set; }
        private Dictionary<string, List<TeamModel>> TeamData { get; set; }
        private readonly IConfiguration configuration;

        public Email()
        {
            Prop = new();
            TeamData = new();
            configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            foreach (var config in configuration.GetChildren())
            {
                if (config.Key != null)
                    Prop[config.Key] = config.Value;
                if (config.Value == "")
                    Console.WriteLine("Please type your email address and password in appsettings.json in bin folder.");
            }
        }

        public void SetData(List<TeamModel> data, string seperator)
        {
            if (data is null) return;
            if (data.Count == 0) return;

            TeamData[seperator] = data;
        }

        public void Send()
        {
            try
            {
                string subject = "Today's result";
                string body = "";
                foreach (var key in TeamData.Keys)
                {
                    body = body + BuildBody(key);
                }
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(Prop["emailFromAddress"]);
                    mail.To.Add(Prop["emailToAddress"]);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient(Prop["smtpAddress"], Int32.Parse(Prop["portNumber"])))
                    {
                        smtp.Credentials = new NetworkCredential(Prop["emailFromAddress"], Prop["password"]);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
                Console.WriteLine($"The result is emailed to {Prop["emailToAddress"]}");
            }
            catch
            {
                Console.WriteLine("Error occured while emailing.");
            }
        }

        private string BuildBody(string key)
        {
            string head = """
                <html>
                <head>
                    <style>
                        table {
                            border-collapse: collapse;
                            width: 100%;
                        }

                        th, td {
                            border: 1px solid black;
                            padding: 8px;
                            text-align: center;
                        }
                        th {
                            font-weight: bold; 
                        }
                    </style>
                </head>
                """;
            string body = $"""
                <body>
                <table>
                    <tr>
                        <th>{key}</th>
                        <th>W</th>
                        <th>L</th>
                        <th>W/L%</th>
                        <th>GB</th>
                        <th>PS/G</th>
                        <th>PA/G</th>
                    </tr>
                """;
            foreach (var model in TeamData[key])
            {
                string tr = $"""
                    <tr>
                        <td>{model.Name}</td>
                        <td>{model.Win}</td>
                        <td>{model.Lose}</td>
                        <td>{model.WL}</td>
                        <td>{model.GB}</td>
                        <td>{model.PSG}</td>
                        <td>{model.PAG}</td>
                    </tr>
                    """;
                body = body + tr;
            }
            string close = $"""
                    </table>
                </body>
                </html>
                """;
            return head + body + close;
        }
    }
}
