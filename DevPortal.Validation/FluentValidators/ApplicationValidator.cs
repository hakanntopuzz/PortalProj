using DevPortal.Model;
using DevPortal.Resources.Resources;
using FluentValidation;
using System.Globalization;

namespace DevPortal.Validation.FluentValidators
{
    public class ApplicationValidator : AbstractValidator<Application>
    {
        const int maxNameLength = 250;
        const int maxRedmineProjectNameLength = 50;
        const int maxDescriptionLength = 2000;

        public ApplicationValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty()
               .WithMessage(GenerateMessage(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.Name))
               .MaximumLength(maxNameLength)
               .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.Name, maxNameLength));

            RuleFor(x => x.ApplicationGroupId)
               .NotEmpty()
               .WithMessage(GenerateMessage(ValidationMessages.NotEmpty, PropertyNames.ApplicationGroupId))
               .GreaterThan(0)
               .WithMessage(GenerateMessage(ValidationMessages.SelectItem, PropertyNames.ApplicationGroupId));

            RuleFor(x => x.StatusId)
               .NotEmpty()
               .WithMessage(GenerateMessage(ValidationMessages.NotEmpty, PropertyNames.ApplicationStatusId))
               .GreaterThan(0)
               .WithMessage(GenerateMessage(ValidationMessages.SelectItem, PropertyNames.ApplicationStatusId));

            RuleFor(x => x.ApplicationTypeId)
               .NotEmpty()
               .WithMessage(GenerateMessage(ValidationMessages.NotEmpty, PropertyNames.ApplicationTypeId))
               .GreaterThan(0)
               .WithMessage(GenerateMessage(ValidationMessages.SelectItem, PropertyNames.ApplicationTypeId));

            RuleFor(x => x.RedmineProjectName)
               .NotEmpty()
               .WithMessage(GenerateMessage(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.RedmineProjectName))
               .MaximumLength(maxRedmineProjectNameLength)
               .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.Name, maxRedmineProjectNameLength));

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