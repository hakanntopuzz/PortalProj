using FluentValidation.Validators;
using System.Text.RegularExpressions;

namespace DevPortal.Validation
{
    public class UrlAddressValidator : PropertyValidator, IUrlAddressValidator
    {
        public UrlAddressValidator() : base(nameof(NotNullValidator))
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            Regex regex = new Regex("^http(s)?://[-a-zA-Z0-9@:%_\\+.~#?&//=]{2,256}\\.[a-z]{2,15}\\b(\\/[-a-zA-Z0-9@:%_\\+.~#?&//=]*)?$");

            if (context.PropertyValue != null && !regex.IsMatch((string)context.PropertyValue))
            {
                return false;
            }
            return true;
        }

        static public string UrlValidatorWithoutHttp
        {
            get
            {
                return "^[-a-zA-Z0-9@:%_\\+.~#?&//=]{2,256}\\.[a-z]{2,15}\\b(\\/[-a-zA-Z0-9@:%_\\+.~#?&//=]*)?$";
            }
        }
    }

    public interface IUrlAddressValidator : IPropertyValidator
    {
    }
}