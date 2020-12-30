namespace DevPortal.Validation.Abstract
{
    public interface IRequestValidator
    {
        ValidationResult Validate<T>(T request) where T : class;
    }
}
