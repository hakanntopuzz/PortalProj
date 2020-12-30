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
    public class ApplicationExportControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IApplicationWikiExportService> applicationWikiService;

        ApplicationWikiExportController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            applicationWikiService = new StrictMock<IApplicationWikiExportService>();

            controller = new ApplicationWikiExportController(
                userSessionService.Object,
                applicationWikiService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            applicationWikiService.VerifyAll();
        }

        #endregion

        #region ExportAsWiki

        [Test]
        public void ExportAsWiki_NoCondition_ReturnWikiFile()
        {
            //Arrange
            int applicationGroupId = 14;
            string applicationName = "name";

            var wikiText = "wiki-text";

            applicationWikiService.Setup(x => x.ExportApplicationsAsWikiText(applicationGroupId, applicationName)).Returns(wikiText);

            //Act
            var result = controller.ExportAsWiki(applicationGroupId, applicationName);

            //Assert
            result.Should().BeOfType<ContentResult>().Which.Content.Should().Be(wikiText);
        }

        #endregion

        #region ExportAsWikiFile

        [Test]
        public void ExportAsWikiFile_NoCondition_ReturnWikiFile()
        {
            //Arrange
            int applicationGroupId = 14;
            string applicationName = "name";

            var fileExportData = new FileExportData
            {
                FileData = new byte[] { 1, 2, 3 },
                FileDownloadName = "file",
                ContentType = ContentTypes.Txt
            };

            applicationWikiService.Setup(x => x.ExportApplicationsAsWikiTextFile(applicationGroupId, applicationName)).Returns(fileExportData);

            //Act
            var result = controller.ExportAsWikiFile(applicationGroupId, applicationName);

            //Assert
            result.Should().BeOfType<FileContentResult>().Which.ContentType.Should().Be(fileExportData.ContentType);
            result.Should().BeOfType<FileContentResult>().Which.FileContents.Should().BeEquivalentTo(fileExportData.FileData);
            result.Should().BeOfType<FileContentResult>().Which.FileDownloadName.Should().Be(fileExportData.FileDownloadName);
        }

        #endregion
    }
}