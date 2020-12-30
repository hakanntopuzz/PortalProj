using DevPortal.Business.Abstract.Services;
using DevPortal.Framework.Abstract;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;

namespace DevPortal.Web.Library.Controllers
{
    public class DependenciesExportController : BaseController
    {
        #region ctor

        readonly IApplicationDependencyExportService applicationDependencyExportService;

        public DependenciesExportController(
            IUserSessionService userSessionService,
            IApplicationDependencyExportService dependenciesExportService) : base(userSessionService)
        {
            this.applicationDependencyExportService = dependenciesExportService;
        }

        #endregion

        #region export application dependencies to csv

        public IActionResult ExportApplicationDependenciesCsv(int applicationId)
        {
            var exportResult = applicationDependencyExportService.ExportApplicationDependenciesAsCsv(applicationId);

            return File(exportResult.Value, ContentTypes.Csv, ExportFileNames.ApplicationDependenciesCsv);
        }

        #endregion

        #region export database dependencies to csv

        public IActionResult ExportDatabaseDependenciesCsv(int applicationId)
        {
            var exportResult = applicationDependencyExportService.ExportDatabaseDependenciesAsCsv(applicationId);

            return File(exportResult.Value, ContentTypes.Csv, ExportFileNames.DatabaseDependenciesCsv);
        }

        #endregion

        #region export external dependencies to csv

        public IActionResult ExportExternalDependenciesCsv(int applicationId)
        {
            var exportResult = applicationDependencyExportService.ExportExternalDependenciesAsCsv(applicationId);

            return File(exportResult.Value, ContentTypes.Csv, ExportFileNames.ExternalDependenciesCsv);
        }

        #endregion

        #region export nuget package dependencies to csv

        public IActionResult ExportNugetPackageDependenciesCsv(int applicationId)
        {
            var exportResult = applicationDependencyExportService.ExportNugetPackageDependenciesAsCsv(applicationId);

            return File(exportResult.Value, ContentTypes.Csv, ExportFileNames.NugetPackageDependenciesCsv);
        }

        #endregion

        #region export dependencies as wiki

        public IActionResult ExportDependenciesAsWiki(int applicationId)
        {
            var wikiText = applicationDependencyExportService.ExportApplicationDependenciesAsWikiText(applicationId);

            return Content(wikiText);
        }

        public IActionResult ExportDependenciesAsWikiFile(int applicationId)
        {
            var fileExportData = applicationDependencyExportService.ExportApplicationDependenciesAsWikiTextFile(applicationId);

            return File(fileExportData.FileData, fileExportData.ContentType, fileExportData.FileDownloadName);
        }

        #endregion
    }
}