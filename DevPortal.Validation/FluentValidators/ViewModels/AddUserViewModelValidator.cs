using DevPortal.Model;
using DevPortal.Resources.Resources;
using DevPortal.Web.Library.Model;
using FluentValidation;
using System.Globalization;

namespace DevPortal.Validation.FluentValidators
{
    public class AddUserViewModelValidator : AbstractValidator<AddUserViewModel>
    {
        public AddUserViewModelValidator()
        {
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.Password))
                .MinimumLength(ConstIntegers.MinPasswordLength)
                .WithMessage(string.Format(CultureInfo.CurrentCulture, ValidationMessages.MinLength, PropertyNames.Password, ConstIntegers.MinPasswordLength))
                .MaximumLength(ConstIntegers.MaxPasswordLength)
                .WithMessage(string.Format(CultureInfo.CurrentCulture, ValidationMessages.MaxLength, PropertyNames.Password, ConstIntegers.MaxPasswordLength));

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.ConfirmPassword))
                .Equal(x => x.Password)
                .WithMessage(ValidationMessages.PasswordsNotMatch)
                .MinimumLength(ConstIntegers.MinPasswordLength)
                .WithMessage(string.Format(CultureInfo.CurrentCulture, ValidationMessages.MinLength, PropertyNames.ConfirmPassword, ConstIntegers.MinPasswordLength))
                .MaximumLength(ConstIntegers.MaxPasswordLength)
                .WithMessage(string.Format(CultureInfo.CurrentCulture, ValidationMessages.MaxLength, PropertyNames.ConfirmPassword, ConstIntegers.MaxPasswordLength));
        }
    }
}