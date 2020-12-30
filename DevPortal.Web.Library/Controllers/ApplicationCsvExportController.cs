using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;

namespace DevPortal.Web.Library.Controllers
{
    public class ApplicationCsvExportController : BaseController
    {
        #region ctor

        readonly IApplicationCsvExportService applicationCsvExportService;

        public ApplicationCsvExportController(
            IUserSessionService userSessionService,
            IApplicationCsvExportService applicationCsvExportService) : base(userSessionService)
        {
            this.applicationCsvExportService = applicationCsvExportService;
        }

        #endregion

        #region export to csv

        public IActionResult ExportToCsv(int applicationGroupId, string applicationName)
        {
            var result = applicationCsvExportService.ExportApplicationsAsCsv(applicationGroupId, applicationName);

            return File(result.Value, ContentTypes.Csv, ExportFileNames.ApplicationsCsv);
        }

        #endregion
    }
}