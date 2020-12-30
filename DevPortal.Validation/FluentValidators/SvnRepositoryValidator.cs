using DevPortal.Model;
using DevPortal.Resources.Resources;
using FluentValidation;

namespace DevPortal.Validation.FluentValidators
{
    public class SvnRepositoryValidator : AbstractValidator<SvnRepository>
    {
        const int maxNameLength = 50;

        public SvnRepositoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.SvnName))
                .MaximumLength(maxNameLength)
                .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.SvnName, maxNameLength));

            RuleFor(x => x.SvnRepositoryTypeId)
                .GreaterThan(0)
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.SvnRepositoryType))
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.SvnRepositoryType));
        }
    }
}