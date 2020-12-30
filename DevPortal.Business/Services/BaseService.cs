namespace DevPortal.Business
{
    public class BaseService
    {
        protected string SetMethodNameForLogMessage(string methodName)
        {
            return $"{GetType().Name}.{methodName}";
        }
    }
}