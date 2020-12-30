using DevPortal.Model;
using DevPortal.Resources.Resources;
using FluentValidation;
using System.Globalization;

namespace DevPortal.Validation.FluentValidators
{
    public class NugetPackageDependencyValidator : AbstractValidator<NugetPackageDependency>
    {
        public NugetPackageDependencyValidator()
        {
            RuleFor(x => x.NugetPackageName)
                .NotEmpty()
                .WithMessage(GenerateMessage(ValidationMessages.SelectItem, PropertyNames.NugetPackageName));
        }

        static string GenerateMessage(string message, string property)
        {
            return string.Format(CultureInfo.CurrentCulture, message, property);
        }
    }
}