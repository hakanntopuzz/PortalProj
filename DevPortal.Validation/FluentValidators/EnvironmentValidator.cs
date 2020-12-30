using DevPortal.Model;
using DevPortal.Resources.Resources;
using FluentValidation;

namespace DevPortal.Validation.FluentValidators
{
    public class EnvironmentValidator : AbstractValidator<Environment>
    {
        const int maxNameLength = 20;

        public EnvironmentValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.Name))
                .MaximumLength(maxNameLength)
                .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.Name, maxNameLength));
        }
    }
}