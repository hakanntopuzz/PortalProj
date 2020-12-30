using DevPortal.Model;

namespace DevPortal.Business.Abstract
{
    public interface IApplicationWikiExportService
    {
        string ExportApplicationsAsWikiText(int applicationGroupId, string applicationName);

        FileExportData ExportApplicationsAsWikiTextFile(int applicationGroupId, string applicationName);
    }
}