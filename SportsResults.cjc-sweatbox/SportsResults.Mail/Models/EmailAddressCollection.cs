using System.Collections.ObjectModel;
using SportsResults.Mail.Exceptions;

namespace SportsResults.Mail.Models;

/// <summary>
/// Holds a collections of email addresses.
/// </summary>
public class EmailAddressCollection : Collection<EmailAddress>
{
    #region Constants

    private readonly static char[] EmailAddressDelimiters = [';', ','];

    #endregion
    #region Constructors

    public EmailAddressCollection() : base() { }

    #endregion
    #region Methods

    /// <summary>
    /// Adds a collections of email addresses.
    /// Use either ';' or ',' to separate each individual address.
    /// </summary>
    /// <param name="emailAddresses">Can be one or more email addresses, separated with the ';' or ',' character.</param>
    /// <exception cref="InvalidEmailAddressException">Throw if any email address is not valid.</exception>
    public void Add(string emailAddresses)
    {
        ArgumentNullException.ThrowIfNull(emailAddresses);
        ArgumentException.ThrowIfNullOrWhiteSpace(emailAddresses);

        foreach (var emailAddressString in emailAddresses.Split(EmailAddressDelimiters))
        {
            var emailAddress = new EmailAddress(emailAddressString);

            var validator = new EmailAddressValidator();
            var result = validator.Validate(emailAddress);
            if (!result.IsValid)
            {
                throw new InvalidEmailAddressException(result.ToString());
            }

            Items.Add(emailAddress);
        }
    }

    #endregion
}
