namespace DevPortal.Model
{
    public sealed class StringServiceResult : GenericServiceResult<string>
    {
        StringServiceResult(bool isSuccess, string message, string value)
           : base(isSuccess, message, value)
        {
        }

        #region factory methods

        public static StringServiceResult Success(string value)
        {
            return new StringServiceResult(true, null, value);
        }

        public static StringServiceResult Success(string message, string value)
        {
            return new StringServiceResult(true, message, value);
        }

        public static StringServiceResult Error(string message)
        {
            return new StringServiceResult(false, message, string.Empty);
        }

        #endregion
    }
}