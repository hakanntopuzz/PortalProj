using AB.Framework.UnitTests;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.SvnAdmin.Business.Abstract;
using DevPortal.SvnAdmin.Model;
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
    public class SvnAdminExportControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<ISvnAdminExportService> svnAdminExportService;

        SvnAdminExportController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            svnAdminExportService = new StrictMock<ISvnAdminExportService>();

            controller = new SvnAdminExportController(
                userSessionService.Object,
                svnAdminExportService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            svnAdminExportService.VerifyAll();
        }

        #endregion

        #region export repository list as csv

        [Test]
        public void ExportToCsv_ServiceResultFails_ReturnErrorView()
        {
            // Arrange
            var serviceResult = SvnRepositoryListCsvResult.Error("message");

            svnAdminExportService.Setup(x => x.ExportRepositoriesAsCsv()).Returns(serviceResult);

            // Act
            var result = controller.ExportToCsv();

            // Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Error).ModelAs<string>().Should().Be(serviceResult.Message);
        }

        [Test]
        public void ExportToCsv_ServiceResultSuccess_ReturnView()
        {
            // Arrange
            byte[] csvData = { 1, 2, 3 };
            var serviceResult = SvnRepositoryListCsvResult.Success(csvData);

            svnAdminExportService.Setup(x => x.ExportRepositoriesAsCsv()).Returns(serviceResult);

            // Act
            var result = controller.ExportToCsv();

            // Assert
            result.Should().BeOfType<FileContentResult>().Which.ContentType.Should().Be(ContentTypes.Csv);
            result.Should().BeOfType<FileContentResult>().Which.FileDownloadName.Should().Be(ExportFileNames.SvnRepositoriesCsv);
        }

        #endregion

        #region ExportAsWiki

        [Test]
        public void ExportAsWiki_NoCondition_ReturnWikiFile()
        {
            //Arrange
            var wikiText = "wiki-text";

            svnAdminExportService.Setup(x => x.ExportRepositoriesAsWikiText()).Returns(wikiText);

            //Act
            var result = controller.ExportAsWiki();

            //Assert
            result.Should().BeOfType<ContentResult>().Which.Content.Should().Be(wikiText);
        }

        #endregion

        #region ExportAsWikiFile

        [Test]
        public void ExportAsWikiFile_NoCondition_ReturnWikiFile()
        {
            //Arrange
            var fileExportData = new FileExportData
            {
                FileData = new byte[] { 1, 2, 3 },
                FileDownloadName = "file",
                ContentType = ContentTypes.Txt
            };

            svnAdminExportService.Setup(x => x.ExportRepositoriesAsWikiTextFile()).Returns(fileExportData);

            //Act
            var result = controller.ExportAsWikiFile();

            //Assert
            result.Should().BeOfType<FileContentResult>().Which.ContentType.Should().Be(fileExportData.ContentType);
            result.Should().BeOfType<FileContentResult>().Which.FileContents.Should().BeEquivalentTo(fileExportData.FileData);
            result.Should().BeOfType<FileContentResult>().Which.FileDownloadName.Should().Be(fileExportData.FileDownloadName);
        }

        #endregion
    }
}