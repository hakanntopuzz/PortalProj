using DevPortal.Model;
using DevPortal.Resources.Resources;
using FluentValidation;

namespace DevPortal.Validation.FluentValidators
{
    public class ExternalDependencyValidator : AbstractValidator<ExternalDependency>
    {
        const int maxNameLength = 250;
        const int maxDescriptionLength = 500;

        public ExternalDependencyValidator()
        {
            RuleFor(x => x.Name)
              .NotEmpty()
              .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.ExternalDependencyName))
              .MaximumLength(maxNameLength)
              .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.ExternalDependencyName, maxNameLength));

            RuleFor(x => x.Description)
             .MaximumLength(maxDescriptionLength)
             .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.Description, maxDescriptionLength));
        }
    }
}