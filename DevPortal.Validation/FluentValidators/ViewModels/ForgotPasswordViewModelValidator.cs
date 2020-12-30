using DevPortal.Resources.Resources;
using DevPortal.Web.Library.Model;
using FluentValidation;

namespace DevPortal.Validation.FluentValidators
{
    public class ForgotPasswordViewModelValidator : AbstractValidator<ForgotPasswordViewModel>
    {
        const int maxEmailAddressLength = 100;

        public ForgotPasswordViewModelValidator()
        {
            RuleFor(x => x.EmailAddress)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.Email))
                .EmailAddress()
                .WithMessage(ValidationMessages.ValidEmail)
                 .MaximumLength(maxEmailAddressLength)
                .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.Email, maxEmailAddressLength));
        }
    }
}