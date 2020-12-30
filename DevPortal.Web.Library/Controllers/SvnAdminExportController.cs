using DevPortal.Framework.Abstract;
using DevPortal.SvnAdmin.Business.Abstract;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;

namespace DevPortal.Web.Library.Controllers
{
    public class SvnAdminExportController : BaseController
    {
        #region ctor

        readonly ISvnAdminExportService svnAdminExportService;

        public SvnAdminExportController(
            IUserSessionService userSessionService,
            ISvnAdminExportService svnAdminExportService) : base(userSessionService)
        {
            this.svnAdminExportService = svnAdminExportService;
        }

        #endregion

        #region export repository list as csv

        public IActionResult ExportToCsv()
        {
            var result = svnAdminExportService.ExportRepositoriesAsCsv();

            if (!result.IsSuccess)
            {
                return View(ViewNames.Error, result.Message);
            }

            return File(result.Value, ContentTypes.Csv, ExportFileNames.SvnRepositoriesCsv);
        }

        #endregion

        #region export repository list as wiki

        public IActionResult ExportAsWiki()
        {
            var wikiText = svnAdminExportService.ExportRepositoriesAsWikiText();

            return Content(wikiText);
        }

        public IActionResult ExportAsWikiFile()
        {
            var fileExportData = svnAdminExportService.ExportRepositoriesAsWikiTextFile();

            return File(fileExportData.FileData, fileExportData.ContentType, fileExportData.FileDownloadName);
        }

        #endregion
    }
}