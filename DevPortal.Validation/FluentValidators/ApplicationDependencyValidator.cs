using DevPortal.Model;
using DevPortal.Resources.Resources;
using FluentValidation;
using System.Globalization;

namespace DevPortal.Validation.FluentValidators
{
    public class ApplicationDependencyValidator : AbstractValidator<ApplicationDependency>
    {
        const int maxDescriptionLength = 500;

        public ApplicationDependencyValidator()
        {
            RuleFor(x => x.ApplicationGroupId)
                .NotEmpty()
                .WithMessage(GenerateMessage(ValidationMessages.NotEmpty, PropertyNames.ApplicationGroupId))
                .GreaterThan(0)
                .WithMessage(GenerateMessage(ValidationMessages.SelectItem, PropertyNames.ApplicationGroupId));

            RuleFor(x => x.DependedApplicationId)
                .NotEmpty()
                .WithMessage(GenerateMessage(ValidationMessages.NotEmpty, PropertyNames.ApplicationId))
                .GreaterThan(0)
                .WithMessage(GenerateMessage(ValidationMessages.SelectItem, PropertyNames.ApplicationId));

            RuleFor(x => x.Description)
                .MaximumLength(maxDescriptionLength)
                .WithMessage(string.Format(CultureInfo.CurrentCulture, ValidationMessages.MaxLength, PropertyNames.Description, maxDescriptionLength));
        }

        static string GenerateMessage(string message, string property)
        {
            return string.Format(CultureInfo.CurrentCulture, message, property);
        }
    }
}