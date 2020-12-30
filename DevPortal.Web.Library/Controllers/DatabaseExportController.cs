using DevPortal.Business.Abstract.Services;
using DevPortal.Framework.Abstract;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;

namespace DevPortal.Web.Library.Controllers
{
    public class DatabaseExportController : BaseController
    {
        #region ctor

        readonly IDatabaseExportService databaseExportService;

        public DatabaseExportController(
            IUserSessionService userSessionService,
            IDatabaseExportService databaseExportService) : base(userSessionService)
        {
            this.databaseExportService = databaseExportService;
        }

        #endregion

        #region export to csv

        public IActionResult ExportToCsv(int databaseGroupId, string databaseName)
        {
            var exportResult = databaseExportService.ExportDatabasesAsCsv(databaseGroupId, databaseName);

            return File(exportResult.Value, ContentTypes.Csv, ExportFileNames.DatabasesCsv);
        }

        #endregion

        #region export applications as wiki

        public IActionResult ExportAsWiki(int databaseGroupId, string databaseName)
        {
            var wikiText = databaseExportService.ExportDatabasesAsWikiText(databaseGroupId, databaseName);

            return Content(wikiText);
        }

        public IActionResult ExportAsWikiFile(int databaseGroupId, string databaseName)
        {
            var fileExportData = databaseExportService.ExportDatabasesAsWikiTextFile(databaseGroupId, databaseName);

            return File(fileExportData.FileData, fileExportData.ContentType, fileExportData.FileDownloadName);
        }

        #endregion
    }
}