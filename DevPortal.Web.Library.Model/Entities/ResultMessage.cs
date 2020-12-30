namespace DevPortal.Web.Library.Model
{
    public class ResultMessage
    {
        #region members

        public MessageType MessageType { get; set; }

        public string Message { get; set; }

        #endregion

        #region ctor

        ResultMessage(MessageType messageType, string message)
        {
            MessageType = messageType;
            Message = message;
        }

        #endregion

        #region factory methods

        public static ResultMessage Success(string message)
        {
            return new ResultMessage(MessageType.Success, message);
        }

        public static ResultMessage Error(string message)
        {
            return new ResultMessage(MessageType.Error, message);
        }

        #endregion
    }
}