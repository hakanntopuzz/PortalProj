namespace DevPortal.Web.Library.Model
{
    public class ErrorControllerActionNames : BaseActionNames
    {
        public static string InternalServerError => SetActionName(nameof(InternalServerError));

        public static string Forbidden => SetActionName(nameof(Forbidden));
    }
}