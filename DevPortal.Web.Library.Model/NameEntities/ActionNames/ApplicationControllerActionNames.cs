namespace DevPortal.Web.Library.Model
{
    public class ApplicationControllerActionNames : BaseActionNames
    {
        public static string Search => SetActionName(nameof(Search));

        public static string GetSvnRepositories => SetActionName(nameof(GetSvnRepositories));

        public static string GetJenkinsJobs => SetActionName(nameof(GetJenkinsJobs));

        public static string LogDetails => SetActionName(nameof(LogDetails));

        public static string BuildSettings => SetActionName(nameof(BuildSettings));
    }
}