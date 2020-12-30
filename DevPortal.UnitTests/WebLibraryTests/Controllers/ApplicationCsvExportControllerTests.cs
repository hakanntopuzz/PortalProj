using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Controllers;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace DevPortal.UnitTests.WebLibraryTests.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationCsvExportControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IApplicationCsvExportService> applicationCsvExportService;

        ApplicationCsvExportController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            applicationCsvExportService = new StrictMock<IApplicationCsvExportService>();

            controller = new ApplicationCsvExportController(
                userSessionService.Object,
                applicationCsvExportService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            applicationCsvExportService.VerifyAll();
        }

        #endregion

        #region ExportToCsv

        [Test]
        public void ExportToCsv_ServiceResultSuccess_ReturnCsvFile()
        {
            // Arrange
            var applicationGroupId = 1;
            var applicationName = "";
            byte[] csvData = { 1, 2, 3 };
            var serviceResult = CsvServiceResult.Success(csvData);

            applicationCsvExportService.Setup(x => x.ExportApplicationsAsCsv(applicationGroupId, applicationName)).Returns(serviceResult);

            // Act
            var result = controller.ExportToCsv(applicationGroupId, applicationName);

            // Assert
            result.Should().BeOfType<FileContentResult>().Which.ContentType.Should().Be(ContentTypes.Csv);
            result.Should().BeOfType<FileContentResult>().Which.FileContents.Should().BeSameAs(csvData);
            result.Should().BeOfType<FileContentResult>().Which.FileDownloadName.Should().Be(ExportFileNames.ApplicationsCsv);
        }

        #endregion
    }
}