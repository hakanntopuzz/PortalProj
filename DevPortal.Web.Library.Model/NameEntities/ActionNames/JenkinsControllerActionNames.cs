namespace DevPortal.Web.Library.Model
{
    public class JenkinsControllerActionNames : BaseActionNames
    {
        public static string Jobs => SetActionName(nameof(Jobs));

        public static string FailedJobs => "failed-jobs";
    }
}