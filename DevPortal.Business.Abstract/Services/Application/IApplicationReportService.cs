using DevPortal.Model;

namespace DevPortal.Business.Abstract
{
    public interface IApplicationReportService
    {
        ApplicationStats GetApplicationStats();
    }
}