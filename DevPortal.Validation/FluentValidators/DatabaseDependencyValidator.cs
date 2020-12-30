using DevPortal.Model;
using DevPortal.Resources.Resources;
using FluentValidation;
using System.Globalization;

namespace DevPortal.Validation.FluentValidators
{
    public class DatabaseDependencyValidator : AbstractValidator<DatabaseDependency>
    {
        const int maxDescriptionLength = 500;

        public DatabaseDependencyValidator()
        {
            RuleFor(x => x.DatabaseGroupId)
              .NotEmpty()
              .WithMessage(GenerateMessage(ValidationMessages.NotEmpty, PropertyNames.DatabaseGroupId))
              .GreaterThan(0)
              .WithMessage(GenerateMessage(ValidationMessages.SelectItem, PropertyNames.DatabaseGroupId));

            RuleFor(x => x.DatabaseId)
                .NotEmpty()
                .WithMessage(GenerateMessage(ValidationMessages.NotEmpty, PropertyNames.Database))
                .GreaterThan(0)
                .WithMessage(GenerateMessage(ValidationMessages.SelectItem, PropertyNames.Database));

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