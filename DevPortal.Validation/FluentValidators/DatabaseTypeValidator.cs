using DevPortal.Model;
using DevPortal.Resources.Resources;
using FluentValidation;

namespace DevPortal.Validation.FluentValidators
{
    public class DatabaseTypeValidator : AbstractValidator<DatabaseType>
    {
        const int maxNameLength = 50;

        public DatabaseTypeValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.Name))
                .MaximumLength(maxNameLength)
                .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.Name, maxNameLength));
        }
    }
}