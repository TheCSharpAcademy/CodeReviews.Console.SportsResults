
using System.Runtime.InteropServices.JavaScript;

namespace SportsResults.samggannon
{
    internal static class Helpers
    {
        internal static List<string> CheckEmailSettings(string? smtpAddress, string? smtpPassword, int smtpPort, string? fromAddress, string? toAddress)
        {
           List<string> errorMessages = new();

            if (string.IsNullOrEmpty(smtpAddress))
            {
                errorMessages.Add("SMTP address is null. Configure the smtpAddress in your app.config file");
            }

            if (string.IsNullOrEmpty(smtpPassword))
            {
                errorMessages.Add("SMTP password is null. Configure the smptPassword in your app.config file");
            }

            if (string.IsNullOrEmpty(fromAddress))
            {
                errorMessages.Add("From address is null. Pease configure the sender in your app.config file");
            }

            if (string.IsNullOrEmpty(toAddress))
            {
                errorMessages.Add("To address is null.  Pease configure the recipient in your app.config file");
            }

            // Additional checks for non-nullable types
            if (smtpPort == 0)
            {
                errorMessages.Add("SMTP port is not set.");
            }

            return errorMessages;

        }
    }
}