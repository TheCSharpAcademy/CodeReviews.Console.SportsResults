using SportsResults.KamilKolanowski.Controllers;

namespace SportsResults.KamilKolanowski;

class Program
{
    static void Main(string[] args)
    {
        SportsNotifierController controller = new();
        controller.TriggerSportsResultsMessageSenderAsync("<your_recipient_email_address>");
    }
}
