using System.Net.Mail;

namespace SportsResults.Mail.Models;

/// <summary>
/// Represents an email address.
/// </summary>
public class EmailAddress
{
    #region Constructors

    public EmailAddress(string address) : this(address, "")
    {
    }

    public EmailAddress(string address, string displayName)
    {
        ArgumentNullException.ThrowIfNull(address, nameof(address));
        ArgumentException.ThrowIfNullOrWhiteSpace(address, nameof(address));

        Address = address;
        DisplayName = displayName;
    }

    #endregion
    #region Properties

    public string Address { get; set; }

    public string DisplayName { get; set; }

    public bool HasDisplayName => !string.IsNullOrWhiteSpace(DisplayName);

    #endregion
    #region Methods

    internal MailAddress ToMailAddress()
    {
        return HasDisplayName
            ? new MailAddress(Address, DisplayName)
            : new MailAddress(Address);
    }

    #endregion
}
