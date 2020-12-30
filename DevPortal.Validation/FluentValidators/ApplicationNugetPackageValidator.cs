using DevPortal.Model;
using DevPortal.Resources.Resources;
using FluentValidation;

namespace DevPortal.Validation.FluentValidators
{
    public class ApplicationNugetPackageValidator : AbstractValidator<ApplicationNugetPackage>
    {
        const int maxNugetPackageNameLength = 250;

        public ApplicationNugetPackageValidator()
        {
            RuleFor(x => x.NugetPackageName)
           .NotEmpty()
           .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.NugetPackageName))
           .MaximumLength(maxNugetPackageNameLength)
           .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.NugetPackageName, maxNugetPackageNameLength));
        }
    }
}