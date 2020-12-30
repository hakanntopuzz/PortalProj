using DevPortal.Model;
using DevPortal.Resources.Resources;
using FluentValidation;
using System.Globalization;

namespace DevPortal.Validation.FluentValidators
{
    public class ApplicationBuildSettingsValidator : AbstractValidator<ApplicationBuildSettings>
    {
        public ApplicationBuildSettingsValidator()
        {
            RuleFor(x => x.Workspace)
                .NotEmpty()
                .WithMessage(GenerateMessage(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.Workspace));

            RuleFor(x => x.SolutionName)
                .NotEmpty()
                .WithMessage(GenerateMessage(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.SolutionName));

            RuleFor(x => x.ProjectName)
                .NotEmpty()
                .WithMessage(GenerateMessage(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.ProjectName));

            RuleFor(x => x.DeployPath)
                .NotEmpty()
                .WithMessage(GenerateMessage(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.DeployPath));

        }

        static string GenerateMessage(string message, string property)
        {
            return string.Format(CultureInfo.CurrentCulture, message, property);
        }
    }
}