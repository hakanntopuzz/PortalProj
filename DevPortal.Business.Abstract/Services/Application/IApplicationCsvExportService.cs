using DevPortal.Model;

namespace DevPortal.Business.Abstract
{
    public interface IApplicationCsvExportService
    {
        CsvServiceResult ExportApplicationsAsCsv(int applicationGroupId, string applicationName);
    }
}