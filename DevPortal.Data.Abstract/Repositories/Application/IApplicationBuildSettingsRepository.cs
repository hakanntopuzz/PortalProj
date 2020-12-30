using DevPortal.Model;

namespace DevPortal.Data.Abstract
{
    public interface IApplicationBuildSettingsRepository
    {
        ApplicationBuildSettings GetApplicationBuildSettings(int applicationId);

        bool UpdateApplicationBuildSettings(ApplicationBuildSettings buildSettings);

        bool AddApplicationBuildSettings(ApplicationBuildSettings buildSettings);

        RecordUpdateInfo GetApplicationBuildSettingsUpdateInfo(int applicationId);
    }
}
