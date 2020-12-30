using DevPortal.Cryptography.Model;
using FluentValidation;

namespace DevPortal.Validation
{
    public class PasswordModelValidator : AbstractValidator<PasswordModel>
    {
        public PasswordModelValidator()
        {
            RuleFor(x => x.IncludeLowerCase).NotEqual(false)
            .When(t => t.IncludeNumeric.Equals(false))
            .When(t => t.IncludeSpecialCharacters.Equals(false))
            .When(t => t.IncludeUpperCase.Equals(false))
            .WithMessage("You need to select one");
        }
    }
}