using DevPortal.Model;
using DevPortal.Resources.Resources;
using FluentValidation;

namespace DevPortal.Validation.FluentValidators
{
    public class ApplicationEnvironmentFormValidator : AbstractValidator<ApplicationEnvironment>
    {
        public ApplicationEnvironmentFormValidator()
        {
            RuleFor(x => x.EnvironmentId)
              .GreaterThan(0)
              .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.Environment))
              .NotEmpty()
              .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.Environment));

            RuleFor(x => x.Url)
              .MaximumLength(ConstIntegers.MaxUrlLength)
              .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.Url, ConstIntegers.MaxUrlLength))
              .Matches(UrlAddressValidator.UrlValidatorWithoutHttp)
              .WithMessage(string.Format(ValidationMessages.PropertyValueShouldBeUrl, PropertyNames.Url));

            RuleFor(x => x.PhysicalPath)
              .NotEmpty()
              .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.PhysicalPath))
              .MaximumLength(ConstIntegers.MaxPhysicalPathLength)
              .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.PhysicalPath, ConstIntegers.MaxPhysicalPathLength));

            RuleFor(x => x.LogFilePath)
              .NotEmpty()
              .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.LogFilePath))
              .MaximumLength(ConstIntegers.MaxLogFilePathLength)
              .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.LogFilePath, ConstIntegers.MaxLogFilePathLength));
        }
    }
}