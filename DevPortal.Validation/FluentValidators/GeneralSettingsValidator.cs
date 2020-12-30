using DevPortal.Model;
using DevPortal.Resources.Resources;
using FluentValidation;
using System.Globalization;

namespace DevPortal.Validation.FluentValidators
{
    public class GeneralSettingsValidator : AbstractValidator<GeneralSettings>
    {
        const int maxLengthFiveHundred = 500;

        const int maxLengthTwoHundredAndFifty = 250;

        public GeneralSettingsValidator()
        {
            RuleFor(model => model.RedmineUrl)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.RedmineUrl))
                .MaximumLength(maxLengthFiveHundred)
                .WithMessage(string.Format(CultureInfo.CurrentCulture, ValidationMessages.MaxLength, PropertyNames.RedmineUrl, maxLengthFiveHundred));

            RuleFor(model => model.SvnUrl)
                .NotEmpty()
                .WithMessage(string.Format(CultureInfo.CurrentCulture, ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.SvnUrl))
                .MaximumLength(maxLengthFiveHundred)
                .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.SvnUrl, maxLengthFiveHundred));

            RuleFor(model => model.JenkinsUrl)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.JenkinsUrl))
                .MaximumLength(maxLengthFiveHundred)
                .WithMessage(string.Format(CultureInfo.CurrentCulture, ValidationMessages.MaxLength, PropertyNames.JenkinsUrl, maxLengthFiveHundred));

            RuleFor(model => model.SonarQubeUrl)
                .NotEmpty()
                .WithMessage(string.Format(CultureInfo.CurrentCulture, ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.SonarQubeUrl))
                .MaximumLength(maxLengthFiveHundred)
                .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.SonarQubeUrl, maxLengthFiveHundred));

            RuleFor(model => model.NugetUrl)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.NugetUrl))
                .MaximumLength(maxLengthFiveHundred)
                .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.NugetUrl, maxLengthFiveHundred));

            RuleFor(model => model.NugetApiKey)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.NugetApiKey))
                .MaximumLength(maxLengthTwoHundredAndFifty)
                .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.NugetApiKey, maxLengthTwoHundredAndFifty));

            RuleFor(model => model.NugetPackageArchiveFolderPath)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.NugetPackageArchiveFolderPath))
                .MaximumLength(maxLengthTwoHundredAndFifty)
                .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.NugetPackageArchiveFolderPath, maxLengthTwoHundredAndFifty));

            RuleFor(model => model.ApplicationVersionPackageProdFolderPath)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.ApplicationVersionPackageProdFolderPath))
                .MaximumLength(maxLengthTwoHundredAndFifty)
                .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.ApplicationVersionPackageProdFolderPath, maxLengthTwoHundredAndFifty));

            RuleFor(model => model.ApplicationVersionPackagePreProdFolderPath)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.ApplicationVersionPackagePreProdFolderPath))
                .MaximumLength(maxLengthTwoHundredAndFifty)
                .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.ApplicationVersionPackagePreProdFolderPath, maxLengthTwoHundredAndFifty));

            RuleFor(model => model.DatabaseDeploymentPackageProdFolderPath)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.DatabaseDeploymentPackageProdFolderPath))
                .MaximumLength(maxLengthTwoHundredAndFifty)
                .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.DatabaseDeploymentPackageProdFolderPath, maxLengthTwoHundredAndFifty));

            RuleFor(model => model.DatabaseDeploymentPackagePreProdFolderPath)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.DatabaseDeploymentPackagePreProdFolderPath))
                .MaximumLength(maxLengthTwoHundredAndFifty)
                .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.DatabaseDeploymentPackagePreProdFolderPath, maxLengthTwoHundredAndFifty));
        }
    }
}