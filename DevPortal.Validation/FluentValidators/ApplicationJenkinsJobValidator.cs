using DevPortal.Model;
using DevPortal.Resources.Resources;
using FluentValidation;

namespace DevPortal.Validation.FluentValidators
{
    public class ApplicationJenkinsJobValidator : AbstractValidator<JenkinsJob>
    {
        const int maxJenkinsJobNameLength = 50;

        public ApplicationJenkinsJobValidator()
        {
            RuleFor(x => x.JenkinsJobName)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.JenkinsJobName))
                .MaximumLength(maxJenkinsJobNameLength)
                .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.JenkinsJobName, maxJenkinsJobNameLength));

            RuleFor(x => x.JenkinsJobTypeId)
             .GreaterThan(0)
             .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.JenkinsJobType))
             .NotEmpty()
             .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.JenkinsJobType));
        }
    }
}