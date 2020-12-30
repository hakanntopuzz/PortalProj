using DevPortal.JenkinsManager.Model;
using DevPortal.Resources.Resources;
using FluentValidation;
using System.Globalization;

namespace DevPortal.Validation.FluentValidators
{
    public class BuildScriptRequestValidator : AbstractValidator<BuildScriptRequest>
    {
        public BuildScriptRequestValidator()
        {
            RuleFor(x => x.ApplicationId)
                .NotEmpty()
                .WithMessage(GenerateMessage(ValidationMessages.NotEmpty, PropertyNames.ApplicationId))
                .GreaterThan(0)
                .WithMessage(GenerateMessage(ValidationMessages.SelectItem, PropertyNames.ApplicationId));

            RuleFor(x => x.BuildTypeId)
                .NotEmpty()
                .WithMessage(GenerateMessage(ValidationMessages.NotEmpty, PropertyNames.BuildTypeId))
                .GreaterThan(0)
                .WithMessage(GenerateMessage(ValidationMessages.SelectItem, PropertyNames.BuildTypeId));

            RuleFor(x => x.JobTypeId)
                .NotEmpty()
                .WithMessage(GenerateMessage(ValidationMessages.NotEmpty, PropertyNames.JobTypeId))
                .GreaterThan(0)
                .WithMessage(GenerateMessage(ValidationMessages.SelectItem, PropertyNames.JobTypeId));

            RuleFor(x => x.EnvironmentId)
                .NotEmpty()
                .WithMessage(GenerateMessage(ValidationMessages.NotEmpty, PropertyNames.EnvironmentId))
                .GreaterThan(0)
                .WithMessage(GenerateMessage(ValidationMessages.SelectItem, PropertyNames.EnvironmentId));
        }

        private static string GenerateMessage(string message, string property)
        {
            return string.Format(CultureInfo.CurrentCulture, message, property);
        }
    }
}