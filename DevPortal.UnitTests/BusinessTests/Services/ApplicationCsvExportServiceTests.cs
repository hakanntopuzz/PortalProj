using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Business.Services;
using DevPortal.Framework;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationCsvExportServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<ICsvService> csvService;

        StrictMock<IApplicationFactory> applicationFactory;

        StrictMock<IApplicationReaderService> applicationReaderService;

        ApplicationCsvExportService service;

        [SetUp]
        public void Initialize()
        {
            csvService = new StrictMock<ICsvService>();
            applicationFactory = new StrictMock<IApplicationFactory>();
            applicationReaderService = new StrictMock<IApplicationReaderService>();

            service = new ApplicationCsvExportService(
                csvService.Object,
                applicationFactory.Object,
                applicationReaderService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            csvService.VerifyAll();
            applicationFactory.VerifyAll();
            applicationReaderService.VerifyAll();
        }

        #endregion

        #region  export application to pdf

        [Test]
        public void ExportApplicationToPdf_ApplicationFullModelExists_ReturnPdfServiceResultSuccess()
        {
            // Arrange
            var applicationGroupId = 1;
            var applicationName = "name";

            var applications = new List<ApplicationListItem>();
            var applicationExportListItems = new List<ApplicationExportListItem>();
            byte[] applicationsAsCsv = { 1, 2, 3 };

            var expectedResult = CsvServiceResult.Success(applicationsAsCsv);

            applicationReaderService.Setup(x => x.FilterApplications(applicationGroupId, applicationName)).Returns(applications);
            applicationFactory.Setup(x => x.CreateApplicationExportListItems(applications)).Returns(applicationExportListItems);
            csvService.Setup(x => x.ExportToCsv(applicationExportListItems, CsvColumnNames.ApplicationList)).Returns(applicationsAsCsv);

            // Act
            var result = service.ExportApplicationsAsCsv(applicationGroupId, applicationName);

            // Assert

            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion
    }
}