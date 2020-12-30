namespace DevPortal.Web.Library.Model
{
    public class LogControllerActionNames : BaseActionNames
    {
        public static string LogDetails => SetActionName(nameof(LogDetails));

        public static string DownloadLogFile => SetActionName(nameof(DownloadLogFile));
    }
}