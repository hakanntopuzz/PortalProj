using DevPortal.Model;

namespace DevPortal.Business.Abstract
{
    public interface IApplicationBuildSettingsService
    {
        ApplicationBuildSettings GetApplicationBuildSettings(int applicationId);

        ServiceResult AddOrUpdateApplicationBuildSettings(ApplicationBuildSettings buildSettings);
    }
}
