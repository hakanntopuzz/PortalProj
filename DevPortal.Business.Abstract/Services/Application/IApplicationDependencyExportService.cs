using DevPortal.Model;

namespace DevPortal.Business.Abstract.Services
{
    public interface IApplicationDependencyExportService
    {
        CsvServiceResult ExportApplicationDependenciesAsCsv(int applicationId);

        string ExportApplicationDependenciesAsWikiText(int applicationId);

        FileExportData ExportApplicationDependenciesAsWikiTextFile(int applicationId);

        CsvServiceResult ExportDatabaseDependenciesAsCsv(int applicationId);

        CsvServiceResult ExportExternalDependenciesAsCsv(int applicationId);

        CsvServiceResult ExportNugetPackageDependenciesAsCsv(int applicationId);
    }
}