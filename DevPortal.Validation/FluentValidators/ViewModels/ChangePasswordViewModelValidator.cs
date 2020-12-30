using DevPortal.Model;
using DevPortal.Resources.Resources;
using DevPortal.Web.Library.Model;
using FluentValidation;

namespace DevPortal.Validation.FluentValidators
{
    public class ChangePasswordViewModelValidator : AbstractValidator<ChangePasswordViewModel>
    {
        public ChangePasswordViewModelValidator()
        {
            RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.Password))
            .MinimumLength(ConstIntegers.MinPasswordLength)
            .WithMessage(string.Format(ValidationMessages.MinLength, PropertyNames.Password, ConstIntegers.MinPasswordLength));

            RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.NewPassword))
            .MinimumLength(ConstIntegers.MinPasswordLength)
            .WithMessage(string.Format(ValidationMessages.MinLength, PropertyNames.NewPassword, ConstIntegers.MinPasswordLength));

            RuleFor(x => x.ConfirmPassword)
             .NotEmpty()
             .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.ConfirmPassword))
             .Equal(x => x.NewPassword)
             .WithMessage(ValidationMessages.PasswordsNotMatch);
        }
    }
}