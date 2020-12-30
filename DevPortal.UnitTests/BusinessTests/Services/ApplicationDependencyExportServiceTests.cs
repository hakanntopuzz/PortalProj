using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Business.Services;
using DevPortal.Framework;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.UnitTests.TestHelpers;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationDependencyExportServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IApplicationDependencyReaderService> applicationDependencyReaderService;

        StrictMock<ICsvService> csvService;

        StrictMock<IApplicationWikiService> applicationWikiExportService;

        StrictMock<IDatabaseDependencyService> databaseDependencyService;

        StrictMock<IExternalDependencyService> externalDependencyService;

        ApplicationDependencyExportService service;

        [SetUp]
        public void Initialize()
        {
            applicationDependencyReaderService = new StrictMock<IApplicationDependencyReaderService>();
            csvService = new StrictMock<ICsvService>();
            applicationWikiExportService = new StrictMock<IApplicationWikiService>();
            databaseDependencyService = new StrictMock<IDatabaseDependencyService>();
            externalDependencyService = new StrictMock<IExternalDependencyService>();

            service = new ApplicationDependencyExportService(
                applicationDependencyReaderService.Object,
                csvService.Object,
                applicationWikiExportService.Object,
                databaseDependencyService.Object,
                externalDependencyService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            applicationDependencyReaderService.VerifyAll();
            csvService.VerifyAll();
            applicationWikiExportService.VerifyAll();
            databaseDependencyService.VerifyAll();
        }

        #endregion

        #region ExportApplicationDependenciesAsCsv

        [Test]
        public void ExportApplicationDependenciesAsCsv_NoCondition_ReturnDependencies()
        {
            // Arrange
            var applicationId = 0;
            var applicationExportListItems = new List<ApplicationDependenciesExportListItem>();
            byte[] encodedBytes = { 1, 2, 3 };

            applicationDependencyReaderService.Setup(x => x.GetApplicationDependenciesExportList(applicationId)).Returns(applicationExportListItems);
            csvService.Setup(x => x.ExportToCsv(applicationExportListItems, CsvColumnNames.ApplicationDependenciesList)).Returns(encodedBytes);

            // Act
            var result = service.ExportApplicationDependenciesAsCsv(applicationId);

            // Assert
            AssertHelpers.AssertCsvServiceResult(encodedBytes, result);
        }

        #endregion

        #region ExportDatabaseDependenciesAsCsv

        [Test]
        public void ExportDatabaseDependenciesAsCsv_NoCondition_ReturnDependencies()
        {
            // Arrange
            var applicationId = 0;
            var databaseExportListItems = new List<DatabaseDependenciesExportListItem>();
            byte[] encodedBytes = { 1, 2, 3 };

            applicationDependencyReaderService.Setup(x => x.GetDatabaseDependenciesExportList(applicationId)).Returns(databaseExportListItems);
            csvService.Setup(x => x.ExportToCsv(databaseExportListItems, CsvColumnNames.DatabaseDependenciesList)).Returns(encodedBytes);

            // Act
            var result = service.ExportDatabaseDependenciesAsCsv(applicationId);

            // Assert
            AssertHelpers.AssertCsvServiceResult(encodedBytes, result);
        }

        #endregion

        #region ExportExternalDependenciesAsCsv

        [Test]
        public void ExportExternalDependenciesAsCsv_NoCondition_ReturnDependencies()
        {
            // Arrange
            var applicationId = 0;
            var externalExportListItems = new List<ExternalDependenciesExportListItem>();
            byte[] encodedBytes = { 1, 2, 3 };

            externalDependencyService.Setup(x => x.GetExternalDependencies(applicationId)).Returns(externalExportListItems);
            csvService.Setup(x => x.ExportToCsv(externalExportListItems, CsvColumnNames.ExternalDependenciesList)).Returns(encodedBytes);

            // Act
            var result = service.ExportExternalDependenciesAsCsv(applicationId);

            // Assert
            AssertHelpers.AssertCsvServiceResult(encodedBytes, result);
        }

        #endregion

        #region ExportNugetPackageDependenciesAsCsv

        [Test]
        public void ExportNugetPackageDependenciesAsCsv_NoCondition_ReturnDependencies()
        {
            // Arrange
            var applicationId = 0;
            var externalExportListItems = new List<NugetPackageDependenciesExportListItem>();
            byte[] encodedBytes = { 1, 2, 3 };

            applicationDependencyReaderService.Setup(x => x.GetNugetPackageDependenciesExportList(applicationId)).Returns(externalExportListItems);
            csvService.Setup(x => x.ExportToCsv(externalExportListItems, CsvColumnNames.NugetPackageDependenciesList)).Returns(encodedBytes);

            // Act
            var result = service.ExportNugetPackageDependenciesAsCsv(applicationId);

            // Assert
            AssertHelpers.AssertCsvServiceResult(encodedBytes, result);
        }

        #endregion

        #region ExportApplicationDependenciesAsWikiText

        [Test]
        public void ExportApplicationDependenciesAsWikiText_NoCondition_ReturnWikiText()
        {
            // Arrange
            var applicationId = 45;

            var application = new ApplicationDependency();
            var applications = new List<ApplicationDependency> { application };
            var database = new DatabaseDependency();
            var databases = new List<DatabaseDependency> { database };
            var externalDependency = new ExternalDependency();
            var externalDependencies = new List<ExternalDependency> { externalDependency };
            var nugetPackageDependency = new NugetPackageDependency();
            var nugetPackageDependencies = new List<NugetPackageDependency> { nugetPackageDependency };
            var applicationsWikiText = "applications-wiki";
            var databasesWikiText = "databases-wiki";
            var externalDependenciesWikiText = "externals-wiki";
            var nugetPackageDependenciesWikiText = "nugetPackages-wiki";

            applicationDependencyReaderService.Setup(x => x.GetApplicationDependencies(applicationId)).Returns(applications);
            databaseDependencyService.Setup(x => x.GetDatabaseDependenciesByApplicationId(applicationId)).Returns(databases);
            applicationDependencyReaderService.Setup(x => x.GetExternalDependencies(applicationId)).Returns(externalDependencies);
            applicationDependencyReaderService.Setup(x => x.GetNugetPackageDependencies(applicationId)).Returns(nugetPackageDependencies);
            applicationWikiExportService.Setup(x => x.ExportApplicationDependenciesAsWikiText(applications)).Returns(applicationsWikiText);
            applicationWikiExportService.Setup(x => x.ExportDatabaseDependenciesAsWikiText(databases)).Returns(databasesWikiText);
            applicationWikiExportService.Setup(x => x.ExportExternalDependenciesAsWikiText(externalDependencies)).Returns(externalDependenciesWikiText);
            applicationWikiExportService.Setup(x => x.ExportNugetPackageDependenciesAsWikiText(nugetPackageDependencies)).Returns(nugetPackageDependenciesWikiText);

            // Act
            var result = service.ExportApplicationDependenciesAsWikiText(applicationId);

            // Assert
            result.Should().Be($"{applicationsWikiText}\r\n{databasesWikiText}\r\n{externalDependenciesWikiText}\r\n{nugetPackageDependenciesWikiText}");
        }

        #endregion

        #region ExportApplicationDependenciesAsWikiTextFile

        [Test]
        public void ExportApplicationDependenciesAsWikiTextFile_NoCondition_ReturnFileExportData()
        {
            // Arrange
            var applicationId = 45;

            var application = new ApplicationDependency();
            var applications = new List<ApplicationDependency> { application };
            var database = new DatabaseDependency();
            var databases = new List<DatabaseDependency> { database };
            var externalDependency = new ExternalDependency();
            var externalDependencies = new List<ExternalDependency> { externalDependency };
            var nugetPackageDependency = new NugetPackageDependency();
            var nugetPackageDependencies = new List<NugetPackageDependency> { nugetPackageDependency };
            var applicationsWikiText = "applications-wiki";
            var databasesWikiText = "databases-wiki";
            var externalDependenciesWikiText = "externals-wiki";
            var nugetPackageDependenciesWikiText = "nugetPackages-wiki";

            applicationDependencyReaderService.Setup(x => x.GetApplicationDependencies(applicationId)).Returns(applications);
            databaseDependencyService.Setup(x => x.GetDatabaseDependenciesByApplicationId(applicationId)).Returns(databases);
            applicationDependencyReaderService.Setup(x => x.GetExternalDependencies(applicationId)).Returns(externalDependencies);
            applicationDependencyReaderService.Setup(x => x.GetNugetPackageDependencies(applicationId)).Returns(nugetPackageDependencies);
            applicationWikiExportService.Setup(x => x.ExportApplicationDependenciesAsWikiText(applications)).Returns(applicationsWikiText);
            applicationWikiExportService.Setup(x => x.ExportDatabaseDependenciesAsWikiText(databases)).Returns(databasesWikiText);
            applicationWikiExportService.Setup(x => x.ExportExternalDependenciesAsWikiText(externalDependencies)).Returns(externalDependenciesWikiText);
            applicationWikiExportService.Setup(x => x.ExportNugetPackageDependenciesAsWikiText(nugetPackageDependencies)).Returns(nugetPackageDependenciesWikiText);

            // Act
            var result = service.ExportApplicationDependenciesAsWikiTextFile(applicationId);

            // Assert
            var wikiText = $"{applicationsWikiText}\r\n{databasesWikiText}\r\n{externalDependenciesWikiText}\r\n{nugetPackageDependenciesWikiText}";
            var expectedData = new FileExportData
            {
                FileData = Encoding.UTF8.GetBytes(wikiText),
                FileDownloadName = $"uygulama-bagimliliklari-wiki-{applicationId}.txt",
                ContentType = ContentTypes.Txt
            };
            result.Should().BeEquivalentTo(expectedData);
        }

        #endregion
    }
}