using System.Globalization;
using System.Net.Mail;

namespace SportsResultsNotifier.Validation;

public class DataValidation
{
    private const string DefaultDateFormat = "yyyy/MM/dd";

    public static bool DateValidation(string date, out DateOnly scrappedDate)
    {
        return DateOnly.TryParseExact(date, DefaultDateFormat, 
            CultureInfo.InvariantCulture, DateTimeStyles.None, out scrappedDate);
    }

    public static bool DateValidation(string date, string? format, out DateOnly scrappedDate)
    {
        return DateOnly.TryParseExact(date, format, 
            CultureInfo.InvariantCulture, DateTimeStyles.None, out scrappedDate);
    }

    public static bool UriValidation(string? uri, out Uri? uriResult)
    {
        if(Uri.TryCreate(uri, UriKind.Absolute, out uriResult))
            return true;
        return false;
    }

    public static bool EmailValidation(string? email, out MailAddress? validEmail)
    {
        if(MailAddress.TryCreate(email, out validEmail))
        {
            if(email.Contains(' '))
                return false;
            else
                return true;
        }
        return false;
    }
}