namespace DevPortal.Web.Library.Model
{
    public class AccountControllerActionNames : BaseActionNames
    {
        public static string Login => SetActionName(nameof(Login));

        public static string Profile => SetActionName(nameof(Profile));

        public static string AccessDenied => SetActionName(nameof(AccessDenied));

        public static string Password => SetActionName(nameof(Password));

        public static string ForgotPassword => SetActionName(nameof(ForgotPassword));

        public static string ResetPassword => SetActionName(nameof(ResetPassword));

        public static string LogOut => SetActionName(nameof(LogOut));
    }
}