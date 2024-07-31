using FluentValidation;

namespace SportsResults.Mail.Models;

/// <summary>
/// Validation rules for an email address using FluentValidation.
/// </summary>
internal class EmailAddressValidator : AbstractValidator<EmailAddress>
{
    #region Constructors

    internal EmailAddressValidator()
    {
        RuleFor(x => x.Address).EmailAddress();
    }

    #endregion
}
