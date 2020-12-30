using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace DevPortal.Web.Library.Controllers
{
    public class ApplicationWikiExportController : BaseController
    {
        #region ctor

        readonly IApplicationWikiExportService applicationWikiService;

        public ApplicationWikiExportController(
            IUserSessionService userSessionService,
            IApplicationWikiExportService applicationWikiService) : base(userSessionService)
        {
            this.applicationWikiService = applicationWikiService;
        }

        #endregion

        #region export applications as wiki text

        public IActionResult ExportAsWiki(int applicationGroupId, string applicationName)
        {
            var wikiText = applicationWikiService.ExportApplicationsAsWikiText(applicationGroupId, applicationName);

            return Content(wikiText);
        }

        #endregion

        #region export applications as wiki file

        public IActionResult ExportAsWikiFile(int applicationGroupId, string applicationName)
        {
            var fileExportData = applicationWikiService.ExportApplicationsAsWikiTextFile(applicationGroupId, applicationName);

            return File(fileExportData.FileData, fileExportData.ContentType, fileExportData.FileDownloadName);
        }

        #endregion
    }
}