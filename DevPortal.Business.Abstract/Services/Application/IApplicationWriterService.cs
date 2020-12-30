using DevPortal.Model;

namespace DevPortal.Business.Abstract
{
    public interface IApplicationWriterService
    {
        Int32ServiceResult AddApplication(Application application);

        ServiceResult UpdateApplication(Application application);

        ServiceResult DeleteApplication(int applicationId);
    }
}