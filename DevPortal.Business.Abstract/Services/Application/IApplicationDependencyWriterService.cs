using DevPortal.Model;

namespace DevPortal.Business.Abstract
{
    public interface IApplicationDependencyWriterService
    {
        ServiceResult AddApplicationDependency(ApplicationDependency applicationDependency);

        ServiceResult UpdateApplicationDependency(ApplicationDependency applicationDependency);

        ServiceResult DeleteApplicationDependency(int applicationDependencyId);
    }
}