using DevPortal.Model;

namespace DevPortal.Data.Abstract
{
    public interface IApplicationDependencyRepository
    {
        bool AddApplicationDependency(ApplicationDependency applicationDependency);

        ApplicationDependency GetApplicationDependencyById(int applicationId);

        RecordUpdateInfo GetApplicationDependencyUpdateInfo(int id);

        bool UpdateApplicationDependency(ApplicationDependency applicationDependency);

        bool DeleteApplicationDependency(int applicationDependencyId);
    }
}