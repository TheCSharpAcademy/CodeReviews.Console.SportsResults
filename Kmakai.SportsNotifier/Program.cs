using Kmakai.SportsNotifier;
string email = "";
string password = "";
string toEmail = "";

while (true)
{
    var scraper = new ResultScraper();

    while (string.IsNullOrWhiteSpace(email) && string.IsNullOrEmpty(password) && string.IsNullOrEmpty(toEmail))
    {
        Console.WriteLine("Enter your Gmail address:");
        email = Console.ReadLine();

        while (string.IsNullOrEmpty(email))
        {
            Console.WriteLine("Please enter a valid email address:");
            email = Console.ReadLine();
        }

        Console.WriteLine("Please enter your Gmail password:");
        password = Console.ReadLine();

        while (string.IsNullOrEmpty(password))
        {
            Console.WriteLine("Please enter a valid password:");
            password = Console.ReadLine();
        }



        Console.WriteLine("Enter the email address you want to send the results to:");
        toEmail = Console.ReadLine();

        while (string.IsNullOrEmpty(toEmail))
        {
            Console.WriteLine("Please enter a valid email address:");
            toEmail = Console.ReadLine();
        }

    }

    var mailer = new Mailer(email, password);


    try
    {
        var game = scraper.GetResults();
        var body = $"The {game.WinningTeam} beat the {game.LosingTeam} {game.WinningScore} to {game.LosingScore}.\n - {game.WinningTeam} : {game.WinningScore}\n - {game.LosingTeam} : {game.LosingScore}";

        mailer.SendEmail(toEmail, body);

        Console.WriteLine("Email sent successfully!");

        Thread.Sleep(1000 * 60 * 60 * 24);
    }
    catch (Exception e) { Console.WriteLine(e.Message); break; }
}

