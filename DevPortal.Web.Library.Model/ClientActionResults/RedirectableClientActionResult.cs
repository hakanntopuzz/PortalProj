namespace DevPortal.Web.Library.Model
{
    public class RedirectableClientActionResult : BaseResult
    {
        #region members

        public bool Redirect { get; set; }

        public string RedirectUrl { get; set; }

        #endregion

        #region ctor

        protected RedirectableClientActionResult(bool isSuccess, bool redirect, string redirectUrl = null, string message = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            Redirect = redirect;
            RedirectUrl = redirectUrl;
        }

        #endregion

        #region factory methods

        public static RedirectableClientActionResult Success(string redirectUrl, string message = null)
        {
            return new RedirectableClientActionResult(true, true, redirectUrl, message);
        }

        public static RedirectableClientActionResult Error(string redirectUrl, string message = null)
        {
            return new RedirectableClientActionResult(false, true, redirectUrl, message);
        }

        public static RedirectableClientActionResult CompanyFieldsRequired(string redirectUrl, string message = null)
        {
            return new RedirectableClientActionResult(false, false, redirectUrl, message);
        }

        #endregion
    }
}