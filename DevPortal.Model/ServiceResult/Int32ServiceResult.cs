namespace DevPortal.Model
{
    public sealed class Int32ServiceResult : GenericServiceResult<int>
    {
        Int32ServiceResult(bool isSuccess, string message, int value)
            : base(isSuccess, message, value)
        {
        }

        #region factory methods

        public static Int32ServiceResult Success(int value)
        {
            return new Int32ServiceResult(true, null, value);
        }

        public static Int32ServiceResult Success(string message, int value)
        {
            return new Int32ServiceResult(true, message, value);
        }

        public static Int32ServiceResult Error(string message)
        {
            return new Int32ServiceResult(false, message, 0);
        }

        #endregion
    }
}