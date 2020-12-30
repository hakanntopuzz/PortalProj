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
    public class DatabaseExportControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IDatabaseExportService> databaseExportService;

        DatabaseExportController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            databaseExportService = new StrictMock<IDatabaseExportService>();

            controller = new DatabaseExportController(
                userSessionService.Object,
                databaseExportService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            databaseExportService.VerifyAll();
        }

        #endregion

        #region ExportToCsv

        [Test]
        public void ExportToCsv_ServiceResultSuccess_ReturnView()
        {
            // Arrange
            var databaseGroupId = 1;
            var databaseName = "databaseName";
            byte[] csvData = { 1, 2, 3 };
            var serviceResult = CsvServiceResult.Success(csvData);

            databaseExportService.Setup(x => x.ExportDatabasesAsCsv(databaseGroupId, databaseName)).Returns(serviceResult);

            // Act
            var result = controller.ExportToCsv(databaseGroupId, databaseName);

            // Assert
            result.Should().BeOfType<FileContentResult>().Which.ContentType.Should().Be(ContentTypes.Csv);
            result.Should().BeOfType<FileContentResult>().Which.FileDownloadName.Should().Be(ExportFileNames.DatabasesCsv);
        }

        #endregion

        #region ExportAsWiki

        [Test]
        public void ExportAsWiki_NoCondition_ReturnWikiFile()
        {
            //Arrange
            int databaseGroupId = 14;
            string databaseName = "name";

            var wikiText = "wiki-text";

            databaseExportService.Setup(x => x.ExportDatabasesAsWikiText(databaseGroupId, databaseName)).Returns(wikiText);

            //Act
            var result = controller.ExportAsWiki(databaseGroupId, databaseName);

            //Assert
            result.Should().BeOfType<ContentResult>().Which.Content.Should().Be(wikiText);
        }

        #endregion

        #region ExportAsWikiFile

        [Test]
        public void ExportAsWikiFile_NoCondition_ReturnWikiFile()
        {
            //Arrange
            int databaseGroupId = 14;
            string databaseName = "name";

            var fileExportData = new FileExportData
            {
                FileData = new byte[] { 1, 2, 3 },
                FileDownloadName = "file",
                ContentType = ContentTypes.Txt
            };

            databaseExportService.Setup(x => x.ExportDatabasesAsWikiTextFile(databaseGroupId, databaseName)).Returns(fileExportData);

            //Act
            var result = controller.ExportAsWikiFile(databaseGroupId, databaseName);

            //Assert
            result.Should().BeOfType<FileContentResult>().Which.ContentType.Should().Be(fileExportData.ContentType);
            result.Should().BeOfType<FileContentResult>().Which.FileContents.Should().BeEquivalentTo(fileExportData.FileData);
            result.Should().BeOfType<FileContentResult>().Which.FileDownloadName.Should().Be(fileExportData.FileDownloadName);
        }

        #endregion
    }
}