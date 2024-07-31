namespace SportsResults.Mail.Models;

/// <summary>
/// Represents a body of an email message.
/// </summary>
public class EmailBody
{
    #region Constructors

    public EmailBody() : this(string.Empty, false) { }

    public EmailBody(string bodyText) : this(bodyText, false) { }

    public EmailBody(string bodyText, bool isHtml)
    {
        ArgumentNullException.ThrowIfNull(bodyText, nameof(bodyText));

        Text = bodyText;
        IsHtml = isHtml;
    }

    #endregion
    #region Methods

    public string Text { get; set; }

    public bool IsHtml { get; set; }

    #endregion
}
