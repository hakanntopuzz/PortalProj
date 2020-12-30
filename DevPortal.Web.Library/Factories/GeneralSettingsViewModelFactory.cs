using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;

namespace DevPortal.Web.Library
{
    public class GeneralSettingsViewModelFactory : IGeneralSettingsViewModelFactory
    {
        #region ctor

        readonly IBreadCrumbFactory breadCrumbFactory;

        public GeneralSettingsViewModelFactory(
            IBreadCrumbFactory breadCrumbFactory)
        {
            this.breadCrumbFactory = breadCrumbFactory;
        }

        #endregion

        public GeneralSettingsViewModel CreateGeneralSettingsViewModel(GeneralSettings generalSettings)
        {
            return new GeneralSettingsViewModel
            {
                GeneralSettings = generalSettings,
                BreadCrumbViewModel = breadCrumbFactory.CreateGeneralSettingsModel()
            };
        }
    }
}