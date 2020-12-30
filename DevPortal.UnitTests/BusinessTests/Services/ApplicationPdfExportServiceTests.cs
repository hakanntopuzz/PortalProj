using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Business.Services;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationPdfExportServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IHttpContextWrapper> httpContextWrapper;

        StrictMock<IFullApplicationReaderService> fullApplicationReaderService;

        ApplicationPdfExportService service;

        [SetUp]
        public void Initialize()
        {
            httpContextWrapper = new StrictMock<IHttpContextWrapper>();
            fullApplicationReaderService = new StrictMock<IFullApplicationReaderService>();

            service = new ApplicationPdfExportService(
                httpContextWrapper.Object,
                fullApplicationReaderService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            httpContextWrapper.VerifyAll();
            fullApplicationReaderService.VerifyAll();
        }

        #endregion

        #region  export application to pdf

        [Test]
        public void ExportApplicationToPdf_ApplicationFullModelExists_ReturnPdfServiceResultSuccess()
        {
            // Arrange
            var applicationId = 1;
            var applicationFullModel = new ApplicationFullModel();
            var expectedResult = PdfServiceResult.Success(applicationFullModel);
            fullApplicationReaderService.Setup(x => x.GetApplicationWithAllMembers(applicationId)).Returns(applicationFullModel);
            string fileName = $"uygulama-bilgileri-{applicationId}";
            httpContextWrapper.Setup(x => x.DownloadAsPdf(fileName));

            // Act
            var result = service.ExportApplicationToPdf(applicationId);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void GetApplicationDependencyById_ApplicationFullModelDoesNotExist_ReturnPdfServiceResultError()
        {
            // Arrange
            var applicationId = 1;
            ApplicationFullModel applicationFullModel = null;
            var expectedResult = PdfServiceResult.Error(Messages.GeneralError);
            fullApplicationReaderService.Setup(x => x.GetApplicationWithAllMembers(applicationId)).Returns(applicationFullModel);

            // Act
            var result = service.ExportApplicationToPdf(applicationId);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion
    }
}