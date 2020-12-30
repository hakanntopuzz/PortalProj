using DevPortal.Resources.Resources;
using DevPortal.SvnAdmin.Model;
using FluentValidation;
using System.Text.RegularExpressions;

namespace DevPortal.Validation.FluentValidators
{
    public class SvnRepositoryFolderValidator : AbstractValidator<SvnRepositoryFolder>
    {
        public SvnRepositoryFolderValidator()
        {
            const int minimumFolderNameLength = 2;
            const int maximumFolderNameLength = 30;

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.NotEmpty, PropertyNames.SvnRepositoryFolderName))
                .MinimumLength(minimumFolderNameLength)
                .WithMessage(string.Format(ValidationMessages.MinLength,
                PropertyNames.SvnRepositoryFolderName, minimumFolderNameLength))
                .MaximumLength<SvnRepositoryFolder>(maximumFolderNameLength)
                .WithMessage(string.Format(ValidationMessages.MaxLength,
                PropertyNames.SvnRepositoryFolderName, maximumFolderNameLength))
                .Matches("^[a-z0-9-_]+$", RegexOptions.CultureInvariant)
                .WithMessage(ValidationMessages.InvalidSvnRepositoryFolderName);
        }
    }
}