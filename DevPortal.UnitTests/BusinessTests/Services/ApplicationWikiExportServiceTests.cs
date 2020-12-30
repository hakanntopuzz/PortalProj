using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Business.Services;
using DevPortal.Model;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationWikiExportServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IApplicationFactory> applicationFactory;

        StrictMock<IApplicationWikiService> applicationWikiExportService;

        StrictMock<IApplicationReaderService> applicationReaderService;

        ApplicationWikiExportService service;

        [SetUp]
        public void Initialize()
        {
            applicationFactory = new StrictMock<IApplicationFactory>();
            applicationWikiExportService = new StrictMock<IApplicationWikiService>();
            applicationReaderService = new StrictMock<IApplicationReaderService>();

            service = new ApplicationWikiExportService(
                applicationFactory.Object,
                applicationWikiExportService.Object,
                applicationReaderService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            applicationFactory.VerifyAll();
            applicationWikiExportService.VerifyAll();
            applicationReaderService.VerifyAll();
        }

        #endregion

        [Test]
        public void ExportApplicationsAsWikiText_NoCondition_ReturnWikiText()
        {
            // Arrange
            int applicationGroupId = 14;
            string applicationName = "name";

            var application = new ApplicationListItem();
            var applications = new List<ApplicationListItem> { application };
            var applicationWikiExportListItems = new List<ApplicationWikiExportListItem>();
            var applicationsWikiText = "applications-wiki";

            applicationReaderService.Setup(x => x.FilterApplications(applicationGroupId, applicationName)).Returns(applications);
            applicationFactory.Setup(x => x.CreateApplicationWikiExportListItems(applications)).Returns(applicationWikiExportListItems);
            applicationWikiExportService.Setup(x => x.ExportApplicationsAsWikiText(applicationWikiExportListItems)).Returns(applicationsWikiText);

            // Act
            var result = service.ExportApplicationsAsWikiText(applicationGroupId, applicationName);

            // Assert
            result.Should().Be(applicationsWikiText);
        }

        [Test]
        public void ExportApplicationsAsWikiTextFile_NoCondition_ReturnFileExportData()
        {
            // Arrange
            int applicationGroupId = 14;
            string applicationName = "name";

            var application = new ApplicationListItem();
            var applications = new List<ApplicationListItem> { application };
            var applicationWikiExportListItems = new List<ApplicationWikiExportListItem>();
            var applicationsWikiText = "applications-wiki";

            applicationReaderService.Setup(x => x.FilterApplications(applicationGroupId, applicationName)).Returns(applications);
            applicationFactory.Setup(x => x.CreateApplicationWikiExportListItems(applications)).Returns(applicationWikiExportListItems);
            applicationWikiExportService.Setup(x => x.ExportApplicationsAsWikiText(applicationWikiExportListItems)).Returns(applicationsWikiText);

            // Act
            var result = service.ExportApplicationsAsWikiTextFile(applicationGroupId, applicationName);

            // Assert
            var expectedData = new FileExportData
            {
                FileData = Encoding.UTF8.GetBytes(applicationsWikiText),
                FileDownloadName = $"uygulama-listesi-wiki.txt",
                ContentType = ContentTypes.Txt
            };
            result.Should().BeEquivalentTo(expectedData);
        }
    }
}