using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.SvnAdmin.Business;
using DevPortal.SvnAdmin.Model;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class SvnAdminWikiExportServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IWikiTextService> wikiTextService;

        SvnAdminWikiExportService service;

        [SetUp]
        public void Initialize()
        {
            wikiTextService = new StrictMock<IWikiTextService>();

            service = new SvnAdminWikiExportService(wikiTextService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            wikiTextService.VerifyAll();
        }

        #endregion

        #region export repositories as wiki text

        [Test]
        public void ExportRepositoriesAsWikiText_RepositoriesNull_ReturnEmptyText()
        {
            // Arrange
            List<SvnRepositoryFolderListItem> repositories = null;

            // Act
            var result = service.ExportRepositoriesAsWikiText(repositories);

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public void ExportRepositoriesAsWikiText_RepositoriesValid_ReturnWikiText()
        {
            // Arrange
            var repositories = new List<SvnRepositoryFolderListItem>
            {
                new SvnRepositoryFolderListItem{ Name = "repo-1" },
                new SvnRepositoryFolderListItem{ Name = "repo-2" }
            };

            var headerText = ExportFileContentNames.SvnRepositoryListHeaderText;
            var headerWikiText = "header-wiki-text";
            var tableHeader = new[] { ExportFileContentNames.SvnRepositoryName }; ;
            var tableHeaderWikiText = "table-header-wiki-text";
            var tableRow1 = "table-row-1-wiki-text";
            var tableRow2 = "table-row-2-wiki-text";

            wikiTextService.Setup(x => x.GenerateH2(headerText)).Returns(headerWikiText);
            wikiTextService.Setup(x => x.GenerateTableHeader(tableHeader)).Returns(tableHeaderWikiText);
            wikiTextService.Setup(x => x.GenerateTableRow(new[] { repositories[0].Name })).Returns(tableRow1);
            wikiTextService.Setup(x => x.GenerateTableRow(new[] { repositories[1].Name })).Returns(tableRow2);

            // Act
            var result = service.ExportRepositoriesAsWikiText(repositories);

            // Assert
            var expectedWikiText = $"{headerWikiText}{tableHeaderWikiText}{tableRow1}{tableRow2}";
            result.Should().Be(expectedWikiText);
        }

        #endregion
    }
}