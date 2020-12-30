using DevPortal.Model;
using DevPortal.Resources.Resources;
using FluentValidation;

namespace DevPortal.Validation.FluentValidators
{
    public class MenuModelValidator : AbstractValidator<MenuModel>
    {
        const int maxNameLength = 50;
        const int maxLinkLength = 250;
        const int maxDescriptionLength = 250;
        const int maxIconLength = 50;

        public MenuModelValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty()
               .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.Name))
               .MaximumLength(maxNameLength)
               .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.Name, maxNameLength));

            RuleFor(x => x.Link)
               .MaximumLength(maxLinkLength)
               .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.Link, maxLinkLength));

            RuleFor(x => x.Description)
               .MaximumLength(maxDescriptionLength)
               .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.Description, maxDescriptionLength));

            RuleFor(x => x.Icon)
               .MaximumLength(maxIconLength)
               .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.Icon, maxIconLength));

            RuleFor(x => x.MenuGroupId)
             .GreaterThan(0)
             .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.MenuGroup))
             .NotEmpty()
             .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.MenuGroup));
        }
    }
}