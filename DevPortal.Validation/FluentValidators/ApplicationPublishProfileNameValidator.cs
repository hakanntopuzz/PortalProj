using DevPortal.Model;
using FluentValidation;

namespace DevPortal.Validation.FluentValidators
{
    public class ApplicationPublishProfileNameValidator : AbstractValidator<ApplicationPublishProfileName>
    {
        public ApplicationPublishProfileNameValidator()
        {
            //TODO: Validasyon durumu netleştiğinde açılacak ya da silinecek.

            //RuleFor(x => x.Dev)
            //    .NotEmpty()
            //    .WithMessage(GenerateMessage(ValidationMessages.NotEmpty, "Dev"));

            //RuleFor(x => x.Test)
            //    .NotEmpty()
            //    .WithMessage(GenerateMessage(ValidationMessages.NotEmpty, "Test"));

            //RuleFor(x => x.PreProd)
            //    .NotEmpty()
            //    .WithMessage(GenerateMessage(ValidationMessages.NotEmpty, "PreProd"));

            //RuleFor(x => x.Prod)
            //    .NotEmpty()
            //    .WithMessage(GenerateMessage(ValidationMessages.NotEmpty, "Prod"));
        }

        //static string GenerateMessage(string message, string property)
        //{
        //    return string.Format(CultureInfo.CurrentCulture, message, $"{property} {PropertyNames.ProjectPublishProfileName}");
        //}
    }
}