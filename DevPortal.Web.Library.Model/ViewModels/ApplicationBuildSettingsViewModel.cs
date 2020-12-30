using DevPortal.Model;

namespace DevPortal.Web.Library.Model
{
    public class ApplicationBuildSettingsViewModel : AuthorizedBaseViewModel
    {
        public string ApplicationName { get; set; }

        public ApplicationBuildSettings BuildSettings { get; set; }
    }
}