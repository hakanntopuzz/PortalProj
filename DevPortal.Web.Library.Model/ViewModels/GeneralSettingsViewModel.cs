using DevPortal.Model;

namespace DevPortal.Web.Library.Model
{
    public class GeneralSettingsViewModel
    {
        public GeneralSettingsViewModel()
        {
            GeneralSettings = new GeneralSettings();
        }

        public GeneralSettings GeneralSettings { get; set; }

        public BreadCrumbViewModel BreadCrumbViewModel { get; set; }
    }
}