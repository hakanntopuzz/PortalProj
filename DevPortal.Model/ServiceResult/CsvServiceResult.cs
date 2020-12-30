namespace DevPortal.Model
{
    public class CsvServiceResult : GenericServiceResult<byte[]>
    {
        #region ctor

        CsvServiceResult(bool isSuccess, string message, byte[] value)
            : base(isSuccess, message, value)
        {
        }

        #endregion

        #region factory methods

        public static CsvServiceResult Success(byte[] value)
        {
            return new CsvServiceResult(true, null, value);
        }

        public static CsvServiceResult Error(string message)
        {
            return new CsvServiceResult(false, message, null);
        }

        #endregion
    }
}