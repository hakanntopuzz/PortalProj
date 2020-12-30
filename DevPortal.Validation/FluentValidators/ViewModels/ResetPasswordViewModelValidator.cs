using DevPortal.Model;
using DevPortal.Resources.Resources;
using DevPortal.Web.Library.Model;
using FluentValidation;

namespace DevPortal.Validation.FluentValidators
{
    public class ResetPasswordViewModelValidator : AbstractValidator<ResetPasswordViewModel>
    {
        public ResetPasswordViewModelValidator()
        {
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.NewPassword))
                .MinimumLength(ConstIntegers.MinPasswordLength)
                .WithMessage(string.Format(ValidationMessages.MinLength, PropertyNames.NewPassword, ConstIntegers.MinPasswordLength))
                .MaximumLength(ConstIntegers.MaxPasswordLength)
                .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.NewPassword, ConstIntegers.MaxPasswordLength));

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.ConfirmPassword))
                .Equal(x => x.NewPassword)
                .WithMessage(ValidationMessages.PasswordsNotMatch);
        }
    }
}