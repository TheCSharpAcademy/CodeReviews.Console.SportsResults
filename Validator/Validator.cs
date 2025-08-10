using System.Net.Mail;

namespace SportsResultsNotifier.UI.Validator;

public class Validator
{
    public static bool IsValidEmail(string email)
    {
        var valid = true;

		try
		{
			var emailAddress = new MailAddress(email);
		}
		catch 
		{
			valid = false;
		}

		return valid;
    }
}
