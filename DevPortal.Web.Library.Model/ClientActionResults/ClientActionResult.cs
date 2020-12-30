namespace DevPortal.Web.Library.Model
{
    public class ClientActionResult : BaseResult
    {
        #region ctor

        protected ClientActionResult(bool isSuccess, string message = null)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        #endregion

        #region factory methods

        public static ClientActionResult Success(string message = null)
        {
            return new ClientActionResult(true, message);
        }

        public static ClientActionResult Error(string message)
        {
            return new ClientActionResult(false, message);
        }

        #endregion
    }
}