using DevPortal.Model;
using DevPortal.Resources.Resources;
using DevPortal.Web.Library.Model;
using FluentValidation;

namespace DevPortal.Validation.FluentValidators
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        const int maxEmailAddressLength = 100;

        public LoginViewModelValidator()
        {
            RuleFor(x => x.EmailAddress)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.Email))
                .EmailAddress()
                .WithMessage(ValidationMessages.ValidEmail)
                .MaximumLength(maxEmailAddressLength)
                .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.Email, maxEmailAddressLength));

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.Password))
                .MinimumLength(ConstIntegers.MinPasswordLength)
                .WithMessage(string.Format(ValidationMessages.MinLength, PropertyNames.Password, ConstIntegers.MinPasswordLength));
        }
    }
}