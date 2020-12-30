using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;

namespace DevPortal.Business.Services
{
    public class ApplicationPdfExportService : IApplicationPdfExportService
    {
        #region ctor

        readonly IHttpContextWrapper httpContextWrapper;

        readonly IFullApplicationReaderService fullApplicationReaderService;

        public ApplicationPdfExportService(
            IHttpContextWrapper httpContextWrapper,
            IFullApplicationReaderService fullApplicationReaderService)
        {
            this.httpContextWrapper = httpContextWrapper;
            this.fullApplicationReaderService = fullApplicationReaderService;
        }

        #endregion

        #region export application to pdf

        public PdfServiceResult ExportApplicationToPdf(int applicationId)
        {
            var applicationFullModel = fullApplicationReaderService.GetApplicationWithAllMembers(applicationId);

            if (applicationFullModel == null)
            {
                return PdfServiceResult.Error(Messages.GeneralError);
            }

            string fileName = $"uygulama-bilgileri-{applicationId}";
            httpContextWrapper.DownloadAsPdf(fileName);

            return PdfServiceResult.Success(applicationFullModel);
        }

        #endregion
    }
}