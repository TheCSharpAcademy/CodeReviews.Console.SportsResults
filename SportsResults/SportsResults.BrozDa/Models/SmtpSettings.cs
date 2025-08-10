namespace SportsResults.BrozDa.Models
{
    internal class SmtpSettings
    {
        public string Host { get; set; } = null!;
        public int Port { get; set; }
        public string Sender { get; set; } = null!;
        public string Recipient { get; set; } = null!;


    }
}
