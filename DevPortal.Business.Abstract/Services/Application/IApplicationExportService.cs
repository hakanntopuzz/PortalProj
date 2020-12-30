using DevPortal.Model;

namespace DevPortal.Business.Abstract
{
    public interface IApplicationExportService
    {
        PdfServiceResult ExportApplicationToPdf(int applicationId);

        CsvServiceResult ExportApplicationsAsCsv(int applicationGroupId, string applicationName);

        string ExportApplicationsAsWikiText(int applicationGroupId, string applicationName);

        FileExportData ExportApplicationsAsWikiTextFile(int applicationGroupId, string applicationName);
    }
}