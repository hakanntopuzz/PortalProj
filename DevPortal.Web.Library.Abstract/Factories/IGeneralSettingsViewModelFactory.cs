using DevPortal.Model;
using DevPortal.Web.Library.Model;

namespace DevPortal.Web.Library.Abstract
{
    public interface IGeneralSettingsViewModelFactory
    {
        GeneralSettingsViewModel CreateGeneralSettingsViewModel(GeneralSettings generalSettings);
    }
}