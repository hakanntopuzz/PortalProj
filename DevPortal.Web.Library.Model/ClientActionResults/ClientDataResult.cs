namespace DevPortal.Web.Library.Model
{
    public class ClientDataResult : BaseResult
    {
        #region members

        public object Data { get; set; }

        #endregion

        #region ctor

        protected ClientDataResult(bool isSuccess, object data, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        #endregion

        #region factory methods

        public static ClientDataResult Success(object data)
        {
            return new ClientDataResult(true, data, null);
        }

        public static ClientDataResult Error()
        {
            return new ClientDataResult(false, null, null);
        }

        public static ClientDataResult Error(string message)
        {
            return new ClientDataResult(false, null, message);
        }

        public static ClientDataResult Error(object data)
        {
            return new ClientDataResult(false, data, null);
        }

        #endregion
    }
}