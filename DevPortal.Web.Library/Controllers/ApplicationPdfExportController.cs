using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Web.Library.Model;
using jsreport.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace DevPortal.Web.Library.Controllers
{
    public class ApplicationPdfExportController : BaseController
    {
        #region ctor

        readonly IApplicationPdfExportService applicationPdfExportService;

        public ApplicationPdfExportController(
            IUserSessionService userSessionService,
            IApplicationPdfExportService applicationPdfExportService) : base(userSessionService)
        {
            this.applicationPdfExportService = applicationPdfExportService;
        }

        #endregion

        #region Export To Pdf

        [Route("applicationpdfexport/exporttopdf/{applicationId}")]
        [MiddlewareFilter(typeof(JsReportPipeline))]
        public IActionResult ExportToPdf(int applicationId)
        {
            var result = applicationPdfExportService.ExportApplicationToPdf(applicationId);

            if (result.IsSuccess)
            {
                return View(ViewNames.ExportToPdf, result.Value);
            }

            SetErrorResultMessageTempData(result.Message);

            return RedirectToAction(ApplicationControllerActionNames.Detail, ControllerNames.Application, new { id = applicationId });
        }

        #endregion
    }
}