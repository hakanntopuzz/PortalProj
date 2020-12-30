using DevPortal.Model;

namespace DevPortal.Business.Abstract
{
    public interface IApplicationPdfExportService
    {
        PdfServiceResult ExportApplicationToPdf(int applicationId);
    }
}