using DevPortal.Model;
using DevPortal.Web.Library.Model;

namespace DevPortal.Web.Library.Abstract.Factories
{
    public interface IApplicationBuildSettingsViewModelFactory
    {
        ApplicationBuildSettingsViewModel CreateApplicationBuildSettingsViewModel(ApplicationBuildSettings buildSettings, Application application);
    }
}
