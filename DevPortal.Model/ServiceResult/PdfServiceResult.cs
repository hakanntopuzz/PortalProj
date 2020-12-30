namespace DevPortal.Model
{
    public class PdfServiceResult : GenericServiceResult<ApplicationFullModel>
    {
        PdfServiceResult(bool isSuccess, string message, ApplicationFullModel value)
           : base(isSuccess, message, value)
        {
        }

        #region factory methods

        public static PdfServiceResult Success(ApplicationFullModel value)
        {
            return new PdfServiceResult(true, null, value);
        }

        public static PdfServiceResult Error(string message)
        {
            return new PdfServiceResult(false, message, null);
        }

        #endregion
    }
}