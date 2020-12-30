using AB.Framework.UnitTests;
using DevPortal.Business.Abstract.Services;
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
    public class DependenciesExportControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IApplicationDependencyExportService> applicationDependencyExportService;

        DependenciesExportController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            applicationDependencyExportService = new StrictMock<IApplicationDependencyExportService>();

            controller = new DependenciesExportController(
                userSessionService.Object,
                applicationDependencyExportService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            applicationDependencyExportService.VerifyAll();
        }

        #endregion

        #region ExportToCsv

        [Test]
        public void ExportApplicationDependenciesCsv_NoCondition_ReturnCsvFile()
        {
            // Arrange
            var applicationId = 1;
            byte[] csvData = { 1, 2, 3 };
            var serviceResult = CsvServiceResult.Success(csvData);

            applicationDependencyExportService.Setup(x => x.ExportApplicationDependenciesAsCsv(applicationId)).Returns(serviceResult);

            // Act
            var result = controller.ExportApplicationDependenciesCsv(applicationId);

            // Assert
            result.Should().BeOfType<FileContentResult>().Which.ContentType.Should().Be(ContentTypes.Csv);
            result.Should().BeOfType<FileContentResult>().Which.FileContents.Should().BeSameAs(csvData);
            result.Should().BeOfType<FileContentResult>().Which.FileDownloadName.Should().Be(ExportFileNames.ApplicationDependenciesCsv);
        }

        #endregion

        #region ExportDatabaseDependenciesCsv

        [Test]
        public void ExportDatabaseDependenciesCsv_NoCondition_ReturnCsvFile()
        {
            // Arrange
            var applicationId = 1;
            byte[] csvData = { 1, 2, 3 };
            var serviceResult = CsvServiceResult.Success(csvData);

            applicationDependencyExportService.Setup(x => x.ExportDatabaseDependenciesAsCsv(applicationId)).Returns(serviceResult);

            // Act
            var result = controller.ExportDatabaseDependenciesCsv(applicationId);

            // Assert
            result.Should().BeOfType<FileContentResult>().Which.ContentType.Should().Be(ContentTypes.Csv);
            result.Should().BeOfType<FileContentResult>().Which.FileContents.Should().BeSameAs(csvData);
            result.Should().BeOfType<FileContentResult>().Which.FileDownloadName.Should().Be(ExportFileNames.DatabaseDependenciesCsv);
        }

        #endregion

        #region ExportExternalDependenciesCsv

        [Test]
        public void ExportExternalDependenciesCsv_NoCondition_ReturnCsvFile()
        {
            // Arrange
            var applicationId = 1;
            byte[] csvData = { 1, 2, 3 };
            var serviceResult = CsvServiceResult.Success(csvData);

            applicationDependencyExportService.Setup(x => x.ExportExternalDependenciesAsCsv(applicationId)).Returns(serviceResult);

            // Act
            var result = controller.ExportExternalDependenciesCsv(applicationId);

            // Assert
            result.Should().BeOfType<FileContentResult>().Which.ContentType.Should().Be(ContentTypes.Csv);
            result.Should().BeOfType<FileContentResult>().Which.FileContents.Should().BeSameAs(csvData);
            result.Should().BeOfType<FileContentResult>().Which.FileDownloadName.Should().Be(ExportFileNames.ExternalDependenciesCsv);
        }

        #endregion

        #region ExportNugetPackageDependenciesCsv

        [Test]
        public void ExportNugetPackageDependenciesCsv_NoCondition_ReturnCsvFile()
        {
            // Arrange
            var applicationId = 1;
            byte[] csvData = { 1, 2, 3 };
            var serviceResult = CsvServiceResult.Success(csvData);

            applicationDependencyExportService.Setup(x => x.ExportNugetPackageDependenciesAsCsv(applicationId)).Returns(serviceResult);

            // Act
            var result = controller.ExportNugetPackageDependenciesCsv(applicationId);

            // Assert
            result.Should().BeOfType<FileContentResult>().Which.ContentType.Should().Be(ContentTypes.Csv);
            result.Should().BeOfType<FileContentResult>().Which.FileContents.Should().BeSameAs(csvData);
            result.Should().BeOfType<FileContentResult>().Which.FileDownloadName.Should().Be(ExportFileNames.NugetPackageDependenciesCsv);
        }

        #endregion

        #region ExportDependenciesAsWiki

        [Test]
        public void ExportDependenciesAsWiki_NoCondition_ReturnWikiFile()
        {
            //Arrange
            var applicationId = 45;
            var wikiText = "wiki-text";

            applicationDependencyExportService.Setup(x => x.ExportApplicationDependenciesAsWikiText(applicationId)).Returns(wikiText);

            //Act
            var result = controller.ExportDependenciesAsWiki(applicationId);

            //Assert
            result.Should().BeOfType<ContentResult>().Which.Content.Should().Be(wikiText);
        }

        #endregion

        #region ExportDependenciesAsWikiFile

        [Test]
        public void ExportDependenciesAsWikiFile_NoCondition_ReturnWikiFile()
        {
            //Arrange
            var applicationId = 45;
            var fileExportData = new FileExportData
            {
                FileData = new byte[] { 1, 2, 3 },
                FileDownloadName = "file",
                ContentType = ContentTypes.Txt
            };

            applicationDependencyExportService.Setup(x => x.ExportApplicationDependenciesAsWikiTextFile(applicationId)).Returns(fileExportData);

            //Act
            var result = controller.ExportDependenciesAsWikiFile(applicationId);

            //Assert
            result.Should().BeOfType<FileContentResult>().Which.ContentType.Should().Be(fileExportData.ContentType);
            result.Should().BeOfType<FileContentResult>().Which.FileContents.Should().BeEquivalentTo(fileExportData.FileData);
            result.Should().BeOfType<FileContentResult>().Which.FileDownloadName.Should().Be(fileExportData.FileDownloadName);
        }

        #endregion
    }
}