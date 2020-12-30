using DevPortal.Model;
using DevPortal.Resources.Resources;
using FluentValidation;
using System.Globalization;

namespace DevPortal.Validation.FluentValidators
{
    public class DatabaseValidator : AbstractValidator<Database>
    {
        const int maxNameLength = 250;

        const int maxDescriptionLength = 500;

        public DatabaseValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(GenerateMessage(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.Name))
                .MaximumLength(maxNameLength)
                .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.Name, maxNameLength));

            RuleFor(x => x.DatabaseTypeId)
                .NotEmpty()
                .WithMessage(GenerateMessage(ValidationMessages.NotEmpty, PropertyNames.DatabaseTypeId))
                .GreaterThan(0)
                .WithMessage(GenerateMessage(ValidationMessages.SelectItem, PropertyNames.DatabaseTypeId));

            RuleFor(x => x.DatabaseGroupId)
                .NotEmpty()
                .WithMessage(GenerateMessage(ValidationMessages.NotEmpty, PropertyNames.DatabaseGroupId))
                .GreaterThan(0)
                .WithMessage(GenerateMessage(ValidationMessages.SelectItem, PropertyNames.DatabaseGroupId));

            RuleFor(x => x.Description)
               .MaximumLength(maxDescriptionLength)
               .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.Description, maxDescriptionLength));
        }

        static string GenerateMessage(string message, string property)
        {
            return string.Format(CultureInfo.CurrentCulture, message, property);
        }
    }
}