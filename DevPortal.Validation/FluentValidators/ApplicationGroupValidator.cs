using DevPortal.Model;
using DevPortal.Resources.Resources;
using FluentValidation;
using System;

namespace DevPortal.Validation.FluentValidators
{
    public class ApplicationGroupValidator : AbstractValidator<ApplicationGroup>
    {
        const int maxNameLength = 100;

        public ApplicationGroupValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.PropertyValueShouldNotBeEmpty, PropertyNames.ApplicationGroupName))
                .MaximumLength(maxNameLength)
                .WithMessage(string.Format(ValidationMessages.MaxLength, PropertyNames.ApplicationGroupName, maxNameLength));

            RuleFor(x => x.StatusId)
                .NotEmpty()
                .WithMessage(String.Format(ValidationMessages.NotEmpty, PropertyNames.ApplicationGroupStatusId))
                .GreaterThan(0)
                .WithMessage(String.Format(ValidationMessages.SelectItem, PropertyNames.ApplicationGroupStatusId));
        }
    }
}