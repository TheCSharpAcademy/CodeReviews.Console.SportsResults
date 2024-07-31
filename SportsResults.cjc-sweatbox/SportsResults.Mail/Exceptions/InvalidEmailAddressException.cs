namespace SportsResults.Mail.Exceptions;

/// <summary>
/// Exception thrown when an email address can not be proved valid.
/// </summary>
internal class InvalidEmailAddressException : Exception
{
    #region Constructors

    public InvalidEmailAddressException(string? message) : base(message) { }

    #endregion
}
