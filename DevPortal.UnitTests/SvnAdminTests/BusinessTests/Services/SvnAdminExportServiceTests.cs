using AB.Framework.Logger.Nlog.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Framework;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.SvnAdmin.Business;
using DevPortal.SvnAdmin.Business.Abstract;
using DevPortal.SvnAdmin.Data.Abstract;
using DevPortal.SvnAdmin.Model;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class SvnAdminExportServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<ILoggingService> loggingService;

        StrictMock<ISvnAdminRepository> svnAdminRepository;

        StrictMock<ICsvService> csvService;

        StrictMock<ISvnAdminWikiExportService> svnAdminWikiExportService;

        SvnAdminExportService service;

        [SetUp]
        public void Initialize()
        {
            loggingService = new StrictMock<ILoggingService>();
            svnAdminRepository = new StrictMock<ISvnAdminRepository>();
            csvService = new StrictMock<ICsvService>();
            svnAdminWikiExportService = new StrictMock<ISvnAdminWikiExportService>();

            service = new SvnAdminExportService(
                loggingService.Object,
                svnAdminRepository.Object,
                csvService.Object,
                svnAdminWikiExportService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            loggingService.VerifyAll();
            svnAdminRepository.VerifyAll();
            csvService.VerifyAll();
            svnAdminWikiExportService.VerifyAll();
        }

        #endregion

        #region ExportRepositoriesAsWikiText

        [Test]
        public void ExportRepositoriesAsCsv_Success_ReturnSuccessCsvResult()
        {
            // Arrange
            var repoList = new List<SvnRepositoryFolderListItem>();
            byte[] listData = { 1, 2, 3 };
            var csvResult = SvnRepositoryListCsvResult.Success(listData);

            svnAdminRepository.Setup(x => x.GetRepositoryFoldersByAlphabeticalOrder()).Returns(repoList);
            csvService.Setup(x => x.ExportToCsv(repoList, CsvColumnNames.SvnRepositoryList)).Returns(listData);

            // Act
            var result = service.ExportRepositoriesAsCsv();

            // Assert
            result.Should().BeEquivalentTo(csvResult);
        }

        [Test]
        public void ExportRepositoriesAsCsv_SshClientException_ReturnErrorCsvResult()
        {
            // Arrange
            var exception = new SshClientException("test-exception");

            var csvResult = SvnRepositoryListCsvResult.Error(exception.Message);

            svnAdminRepository.Setup(x => x.GetRepositoryFoldersByAlphabeticalOrder()).Throws(exception);
            loggingService.Setup(x => x.LogError(VerifyAny<string>(), VerifyAny<string>(), exception));

            // Act
            var result = service.ExportRepositoriesAsCsv();

            // Assert
            result.Should().BeEquivalentTo(csvResult);
        }

        [Test]
        public void ExportRepositoriesAsCsv_SshCommandResultParseException_ReturnErrorCsvResult()
        {
            // Arrange
            var exception = new SshCommandResultParseException("test-exception");

            var csvResult = SvnRepositoryListCsvResult.Error(exception.Message);

            svnAdminRepository.Setup(x => x.GetRepositoryFoldersByAlphabeticalOrder()).Throws(exception);
            loggingService.Setup(x => x.LogError(VerifyAny<string>(), VerifyAny<string>(), exception));

            // Act
            var result = service.ExportRepositoriesAsCsv();

            // Assert
            result.Should().BeEquivalentTo(csvResult);
        }

        #endregion

        #region ExportRepositoriesAsWikiText

        [Test]
        public void ExportRepositoriesAsWikiText_NoCondition_ReturnWikiText()
        {
            // Arrange
            var repository = new SvnRepositoryFolderListItem();
            var repositories = new List<SvnRepositoryFolderListItem> { repository };
            var repositoriesWikiText = "repositories-wiki";

            svnAdminRepository.Setup(x => x.GetRepositoryFoldersByAlphabeticalOrder()).Returns(repositories);
            svnAdminWikiExportService.Setup(x => x.ExportRepositoriesAsWikiText(repositories)).Returns(repositoriesWikiText);

            // Act
            var result = service.ExportRepositoriesAsWikiText();

            // Assert
            result.Should().Be(repositoriesWikiText);
        }

        #endregion

        #region ExportRepositoriesAsWikiTextFile

        [Test]
        public void ExportRepositoriesAsWikiTextFile_NoCondition_ReturnFileExportData()
        {
            // Arrange
            var repository = new SvnRepositoryFolderListItem();
            var repositories = new List<SvnRepositoryFolderListItem> { repository };
            var repositoriesWikiText = "repositories-wiki";

            svnAdminRepository.Setup(x => x.GetRepositoryFoldersByAlphabeticalOrder()).Returns(repositories);
            svnAdminWikiExportService.Setup(x => x.ExportRepositoriesAsWikiText(repositories)).Returns(repositoriesWikiText);

            // Act
            var result = service.ExportRepositoriesAsWikiTextFile();

            // Assert
            var expectedData = new FileExportData
            {
                FileData = Encoding.UTF8.GetBytes(repositoriesWikiText),
                FileDownloadName = $"svn-depo-listesi-wiki.txt",
                ContentType = ContentTypes.Txt
            };
            result.Should().BeEquivalentTo(expectedData);
        }

        #endregion
    }
}