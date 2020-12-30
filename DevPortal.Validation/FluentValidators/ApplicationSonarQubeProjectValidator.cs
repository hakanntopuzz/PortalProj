using DevPortal.Model;
using DevPortal.Resources.Resources;
using FluentValidation;

namespace DevPortal.Validation.FluentValidators
{
    public class ApplicationSonarQubeProjectValidator : AbstractValidator<SonarqubeProject>
    {
        const int maxSonarqubeProjectNameLength = 50;

        public ApplicationSonarQubeProjectValidator()
        {
            RuleFor(x => x.SonarqubeProjectName)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.SonarqubeProjectName))
                .MaximumLength(maxSonarqubeProjectNameLength)
                .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.SonarqubeProjectName, maxSonarqubeProjectNameLength));

            RuleFor(x => x.SonarqubeProjectTypeId)
                .GreaterThan(0)
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.SonarqubeProjectType))
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.SonarqubeProjectType));
        }
    }
}