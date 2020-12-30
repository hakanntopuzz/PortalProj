using DevPortal.Model;

namespace DevPortal.Business.Abstract
{
    public interface IFullApplicationReaderService
    {
        ApplicationFullModel GetApplicationWithAllMembers(int applicationId);
    }
}