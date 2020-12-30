using DevPortal.Model;

namespace DevPortal.Business.Abstract.Services
{
    public interface IDatabaseExportService
    {
        CsvServiceResult ExportDatabasesAsCsv(int databaseGroupId, string databaseName);

        string ExportDatabasesAsWikiText(int databaseGroupId, string databaseName);

        FileExportData ExportDatabasesAsWikiTextFile(int databaseGroupId, string databaseName);
    }
}