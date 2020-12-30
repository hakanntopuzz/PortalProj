using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Business.Services;
using DevPortal.Model;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class DatabaseWikiExportServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IWikiTextService> wikiTextService;

        DatabaseWikiExportService service;

        [SetUp]
        public void Initialize()
        {
            wikiTextService = new StrictMock<IWikiTextService>();

            service = new DatabaseWikiExportService(wikiTextService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            wikiTextService.VerifyAll();
        }

        #endregion

        #region export applications as wiki text

        [Test]
        public void ExportDatabasesAsWikiText_DependenciesNull_ReturnEmptyText()
        {
            // Arrange
            List<DatabaseWikiExportListItem> databases = null;

            // Act
            var result = service.ExportDatabasesAsWikiText(databases);

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public void ExportDatabasesAsWikiText_NoCondition_ReturnWikiText()
        {
            // Arrange
            var databases = new List<DatabaseWikiExportListItem>
            {
                new DatabaseWikiExportListItem{ Name = "database-1" },
                new DatabaseWikiExportListItem{ Name = "database-2" }
            };

            var headerText = ExportFileContentNames.DatabaseListHeaderText;
            var headerWikiText = "header-wiki-text";
            var tableHeader = new[] {
                ExportFileContentNames.DatabaseName,
                ExportFileContentNames.DatabaseGroup,
                ExportFileContentNames.DatabaseType };
            var tableHeaderWikiText = "table-header-wiki-text";
            var tableRow1 = "table-row-1-wiki-text";
            var tableRow2 = "table-row-2-wiki-text";

            wikiTextService.Setup(x => x.GenerateH2(headerText)).Returns(headerWikiText);
            wikiTextService.Setup(x => x.GenerateTableHeader(tableHeader)).Returns(tableHeaderWikiText);
            wikiTextService.Setup(x => x.GenerateTableRow(new[] { databases[0].Name, databases[0].DatabaseGroupName, databases[0].DatabaseTypeName })).Returns(tableRow1);
            wikiTextService.Setup(x => x.GenerateTableRow(new[] { databases[1].Name, databases[1].DatabaseGroupName, databases[1].DatabaseTypeName })).Returns(tableRow2);

            // Act
            var result = service.ExportDatabasesAsWikiText(databases);

            // Assert
            var expectedWikiText = $"{headerWikiText}{tableHeaderWikiText}{tableRow1}{tableRow2}";
            result.Should().Be(expectedWikiText);
        }

        [Test]
        public void ExportDatabasesAsWikiText_DatabaseWithRedmineProject_ReturnWikiText()
        {
            // Arrange
            var databases = new List<DatabaseWikiExportListItem>
            {
                new DatabaseWikiExportListItem{ Name = "database-1", RedmineProjectName = "database-1" },
                new DatabaseWikiExportListItem{ Name = "database-2" }
            };

            var headerText = ExportFileContentNames.DatabaseListHeaderText;
            var headerWikiText = "header-wiki-text";
            var tableHeader = new[] {
                ExportFileContentNames.DatabaseName,
                ExportFileContentNames.DatabaseGroup,
                ExportFileContentNames.DatabaseType };
            var tableHeaderWikiText = "table-header-wiki-text";
            var tableRow1 = "table-row-1-wiki-text";
            var tableRow2 = "table-row-2-wiki-text";
            var database1NameLink = "database-1-name-link";

            wikiTextService.Setup(x => x.GenerateH2(headerText)).Returns(headerWikiText);
            wikiTextService.Setup(x => x.GenerateTableHeader(tableHeader)).Returns(tableHeaderWikiText);
            wikiTextService.Setup(x => x.GenerateWikiLink(databases[0].RedmineProjectName, databases[0].Name)).Returns(database1NameLink);
            wikiTextService.Setup(x => x.GenerateTableRow(new[] { database1NameLink, databases[0].DatabaseGroupName, databases[0].DatabaseTypeName })).Returns(tableRow1);
            wikiTextService.Setup(x => x.GenerateTableRow(new[] { databases[1].Name, databases[1].DatabaseGroupName, databases[1].DatabaseTypeName })).Returns(tableRow2);

            // Act
            var result = service.ExportDatabasesAsWikiText(databases);

            // Assert
            var expectedWikiText = $"{headerWikiText}{tableHeaderWikiText}{tableRow1}{tableRow2}";
            result.Should().Be(expectedWikiText);
        }

        #endregion

        #region setup helpers

        static List<Application> CreateTwoApplicationsOneWithRedmineProject()
        {
            return new List<Application>
            {
                CreateApplicationWithRedmineProject("test-application-with-redmine-project-1"),
                CreateApplicationWithoutRedmineProject("test-application-without-redmine-project-1")
            };
        }

        static List<Application> CreateTwoApplicationsWithoutRedmineProject()
        {
            return new List<Application>
            {
                CreateApplicationWithoutRedmineProject("test-application-without-redmine-project-1"),
                CreateApplicationWithoutRedmineProject("test-application-without-redmine-project-2")
            };
        }

        static Application CreateApplicationWithRedmineProject(string name)
        {
            return new Application
            {
                Name = name,
                ApplicationGroupName = "test-application-group-1",
                RedmineProjectName = "project-name"
            };
        }

        static Application CreateApplicationWithoutRedmineProject(string name)
        {
            return new Application
            {
                Name = name,
                ApplicationGroupName = "test-application-group"
            };
        }

        static List<Database> CreateTwoDatabasesOneWithRedmineProject()
        {
            return new List<Database>
            {
                CreateDatabaseWithRedmineProject(),
                CreateDatabaseWithoutRedmineProject("test-database-without-redmine-project-1")
            };
        }

        static List<Database> CreateTwoDatabasesWithoutRedmineProject()
        {
            return new List<Database>
            {
                CreateDatabaseWithoutRedmineProject("test-database-without-redmine-project-1"),
                CreateDatabaseWithoutRedmineProject("test-database-without-redmine-project-2")
            };
        }

        static Database CreateDatabaseWithRedmineProject()
        {
            return new Database
            {
                Name = "test-database-with-redmine-project",
                DatabaseGroupName = "test-database-group-1",
                RedmineProjectName = "project-name"
            };
        }

        static Database CreateDatabaseWithoutRedmineProject(string name)
        {
            return new Database
            {
                Name = name,
                DatabaseGroupName = "test-database-group"
            };
        }

        static List<ExternalDependency> CreateTwoExternalDependencies()
        {
            return new List<ExternalDependency> {
                new ExternalDependency{
                    Name = "external-dependency-1",
                    Description = "external-dependency-1-description"
                },
                new ExternalDependency{
                    Name = "external-dependency-2",
                    Description = "external-dependency-2-description"
                }
            };
        }

        #endregion
    }
}