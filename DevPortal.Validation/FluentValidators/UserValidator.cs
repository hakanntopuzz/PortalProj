using DevPortal.Model;
using DevPortal.Resources.Resources;
using FluentValidation;

namespace DevPortal.Validation
{
    public class UserValidator : AbstractValidator<User>
    {
        const int maxFirstNameLength = 50;
        const int maxLastNameLength = 50;
        const int maxSvnUserNameLength = 20;
        const int maxEmailAddresLength = 100;

        public UserValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.Name))
                .MaximumLength(maxFirstNameLength)
                .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.Name, maxFirstNameLength));

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.LastName))
                .MaximumLength(maxLastNameLength)
                .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.LastName, maxLastNameLength));

            RuleFor(x => x.SvnUserName)
              .MaximumLength(maxSvnUserNameLength)
              .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.SvnUserName, maxSvnUserNameLength));

            RuleFor(x => x.EmailAddress)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.Email))
                .EmailAddress()
                .WithMessage(ValidationMessages.ValidEmail)
                .MaximumLength(maxEmailAddresLength)
                .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.Email, maxEmailAddresLength));

            RuleFor(x => x.UserTypeId)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.UserTypeId));

            RuleFor(x => x.UserStatusId)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.UserStatusId));
        }
    }
}