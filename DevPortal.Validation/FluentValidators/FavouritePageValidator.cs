using DevPortal.Model;
using DevPortal.Resources.Resources;
using FluentValidation;
using System.Globalization;

namespace DevPortal.Validation.FluentValidators
{
    public class FavouritePageValidator : AbstractValidator<FavouritePage>
    {
        const int maxPageNameLength = 100;
        const int maxPageUrlLength = 500;

        public FavouritePageValidator()
        {
            RuleFor(x => x.PageName)
                .NotEmpty()
                .WithMessage(GenerateMessage(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.PageName))
                .MaximumLength(maxPageNameLength)
                .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.PageName, maxPageNameLength));

            RuleFor(x => x.PageUrl)
                .NotEmpty()
                .WithMessage(GenerateMessage(ValidationMessages.NotEmpty, PropertyNames.PageUrl))
                .MaximumLength(maxPageUrlLength)
                .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.PageUrl, maxPageUrlLength));
        }

        static string GenerateMessage(string message, string property)
        {
            return string.Format(CultureInfo.CurrentCulture, message, property);
        }
    }
}