using DevPortal.Resources.Resources;
using DevPortal.Web.Library.Model;
using FluentValidation;

namespace DevPortal.Validation.FluentValidators
{
    public class HashViewModelValidator : AbstractValidator<HashViewModel>
    {
        public HashViewModelValidator()
        {
            RuleFor(model => model.HashToText)
                .NotEmpty()
                .WithMessage(ValidationMessages.HashToTextNotBeEmpty);
        }
    }
}