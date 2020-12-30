using DevPortal.Business.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;
using System.Text;

namespace DevPortal.Business.Services
{
    public class ApplicationWikiExportService : IApplicationWikiExportService
    {
        #region ctor

        readonly IApplicationFactory applicationFactory;

        readonly IApplicationWikiService applicationWikiExportService;

        readonly IApplicationReaderService applicationReaderService;

        public ApplicationWikiExportService(
            IApplicationFactory applicationFactory,
            IApplicationWikiService applicationWikiExportService,
            IApplicationReaderService applicationReaderService)
        {
            this.applicationFactory = applicationFactory;
            this.applicationWikiExportService = applicationWikiExportService;
            this.applicationReaderService = applicationReaderService;
        }

        #endregion

        #region export applications as wiki text

        public string ExportApplicationsAsWikiText(int applicationGroupId, string applicationName)
        {
            var applications = FilterApplicationsForWiki(applicationGroupId, applicationName);

            return applicationWikiExportService.ExportApplicationsAsWikiText(applications);
        }

        ICollection<ApplicationWikiExportListItem> FilterApplicationsForWiki(int applicationGroupId, string applicationName)
        {
            var filterApplications = applicationReaderService.FilterApplications(applicationGroupId, applicationName);

            return applicationFactory.CreateApplicationWikiExportListItems(filterApplications);
        }

        #endregion

        #region export applications as wiki file

        public FileExportData ExportApplicationsAsWikiTextFile(int applicationGroupId, string applicationName)
        {
            string wikiText = ExportApplicationsAsWikiText(applicationGroupId, applicationName);

            return new FileExportData
            {
                FileData = Encoding.UTF8.GetBytes(wikiText),
                FileDownloadName = $"uygulama-listesi-wiki.txt",
                ContentType = ContentTypes.Txt
            };
        }

        #endregion
    }
}