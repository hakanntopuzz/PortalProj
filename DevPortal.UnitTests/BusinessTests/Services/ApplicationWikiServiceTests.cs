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
    public class ApplicationWikiServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IWikiTextService> wikiTextService;

        ApplicationWikiService service;

        [SetUp]
        public void Initialize()
        {
            wikiTextService = new StrictMock<IWikiTextService>();

            service = new ApplicationWikiService(wikiTextService.Object);
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
        public void ExportApplicationsAsWikiText_ApplicationsNull_ReturnWikiText()
        {
            // Arrange
            List<ApplicationWikiExportListItem> applications = null;

            // Act
            var result = service.ExportApplicationsAsWikiText(applications);

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public void ExportApplicationsAsWikiText_ApplicationsValid_ReturnWikiText()
        {
            // Arrange
            var applications = new List<ApplicationWikiExportListItem>
            {
                new ApplicationWikiExportListItem{ Name = "application-1" },
                new ApplicationWikiExportListItem{ Name = "application-2" }
            };

            var headerText = ExportFileContentNames.ApplicationListHeaderText;
            var headerWikiText = "header-wiki-text";
            var tableHeader = new[] {
                ExportFileContentNames.ApplicationName,
                ExportFileContentNames.ApplicationGroup,
                ExportFileContentNames.ApplicationType,
                ExportFileContentNames.Status }; ;
            var tableHeaderWikiText = "table-header-wiki-text";
            var tableRow1 = "table-row-1-wiki-text";
            var tableRow2 = "table-row-2-wiki-text";

            wikiTextService.Setup(x => x.GenerateH2(headerText)).Returns(headerWikiText);
            wikiTextService.Setup(x => x.GenerateTableHeader(tableHeader)).Returns(tableHeaderWikiText);
            wikiTextService.Setup(x => x.GenerateTableRow(new[] { applications[0].Name, applications[0].ApplicationGroupName, applications[0].ApplicationTypeName, applications[0].Status })).Returns(tableRow1);
            wikiTextService.Setup(x => x.GenerateTableRow(new[] { applications[1].Name, applications[1].ApplicationGroupName, applications[1].ApplicationTypeName, applications[1].Status })).Returns(tableRow2);

            // Act
            var result = service.ExportApplicationsAsWikiText(applications);

            // Assert
            var expectedWikiText = $"{headerWikiText}{tableHeaderWikiText}{tableRow1}{tableRow2}";
            result.Should().Be(expectedWikiText);
        }

        [Test]
        public void ExportApplicationsAsWikiText_ApplicationWithRedmineProject_ReturnWikiText()
        {
            // Arrange
            var applications = new List<ApplicationWikiExportListItem>
            {
                new ApplicationWikiExportListItem{ Name = "application-1", RedmineProjectName = "application-1" },
                new ApplicationWikiExportListItem{ Name = "application-2" }
            };

            var headerText = ExportFileContentNames.ApplicationListHeaderText;
            var headerWikiText = "header-wiki-text";
            var tableHeader = new[] {
                ExportFileContentNames.ApplicationName,
                ExportFileContentNames.ApplicationGroup,
                ExportFileContentNames.ApplicationType,
                ExportFileContentNames.Status }; ;
            var tableHeaderWikiText = "table-header-wiki-text";
            var tableRow1 = "table-row-1-wiki-text";
            var tableRow2 = "table-row-2-wiki-text";
            var application1NameLink = "application-1-name-link";

            wikiTextService.Setup(x => x.GenerateH2(headerText)).Returns(headerWikiText);
            wikiTextService.Setup(x => x.GenerateTableHeader(tableHeader)).Returns(tableHeaderWikiText);
            wikiTextService.Setup(x => x.GenerateWikiLink(applications[0].RedmineProjectName, applications[0].Name)).Returns(application1NameLink);
            wikiTextService.Setup(x => x.GenerateTableRow(new[] { application1NameLink, applications[0].ApplicationGroupName, applications[0].ApplicationTypeName, applications[0].Status })).Returns(tableRow1);
            wikiTextService.Setup(x => x.GenerateTableRow(new[] { applications[1].Name, applications[1].ApplicationGroupName, applications[1].ApplicationTypeName, applications[1].Status })).Returns(tableRow2);

            // Act
            var result = service.ExportApplicationsAsWikiText(applications);

            // Assert
            var expectedWikiText = $"{headerWikiText}{tableHeaderWikiText}{tableRow1}{tableRow2}";
            result.Should().Be(expectedWikiText);
        }

        #endregion

        #region export application dependencies as wiki text

        [Test]
        public void ExportApplicationDependenciesAsWikiText_DependenciesNull_ReturnEmptyText()
        {
            // Arrange
            List<ApplicationDependency> applications = null;

            // Act
            var result = service.ExportApplicationDependenciesAsWikiText(applications);

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public void ExportApplicationDependenciesAsWikiText_HasRedmineProject_ReturnWikiTextWithRedmineLinks()
        {
            // Arrange
            var applications = CreateTwoApplicationDependenciesOneWithRedmineProject();

            var headerText = ExportFileContentNames.ApplicationDependenciesHeaderText;
            var headerWikiText = "header-wiki-text";
            var tableHeader = new[] { ExportFileContentNames.ApplicationName, ExportFileContentNames.ApplicationGroup };
            var tableHeaderWikiText = "table-header-wiki-text";
            var tableRow1 = "table-row-1-wiki-text";
            var tableRow2 = "table-row-2-wiki-text";
            var application1NameLink = "application-1-name-link";

            wikiTextService.Setup(x => x.GenerateH2(headerText)).Returns(headerWikiText);
            wikiTextService.Setup(x => x.GenerateTableHeader(tableHeader)).Returns(tableHeaderWikiText);
            wikiTextService.Setup(x => x.GenerateWikiLink(applications[0].RedmineProjectName, applications[0].Name)).Returns(application1NameLink);
            wikiTextService.Setup(x => x.GenerateTableRow(new[] { application1NameLink, applications[0].ApplicationGroupName })).Returns(tableRow1);
            wikiTextService.Setup(x => x.GenerateTableRow(new[] { applications[1].Name, applications[1].ApplicationGroupName })).Returns(tableRow2);

            // Act
            var result = service.ExportApplicationDependenciesAsWikiText(applications);

            // Assert
            var expectedWikiText = $"{headerWikiText}{tableHeaderWikiText}{tableRow1}{tableRow2}";
            result.Should().Be(expectedWikiText);
        }

        [Test]
        public void ExportApplicationDependenciesAsWikiText_HasNoRedmineProject_ReturnWikiTextWithoutRedmineLinks()
        {
            // Arrange
            var applications = CreateTwoApplicationDependenciesWithoutRedmineProject();

            var headerText = ExportFileContentNames.ApplicationDependenciesHeaderText;
            var headerWikiText = "header-wiki-text";
            var tableHeader = new[] { ExportFileContentNames.ApplicationName, ExportFileContentNames.ApplicationGroup };
            var tableHeaderWikiText = "table-header-wiki-text";
            var tableRow1 = "table-row-1-wiki-text";
            var tableRow2 = "table-row-2-wiki-text";

            wikiTextService.Setup(x => x.GenerateH2(headerText)).Returns(headerWikiText);
            wikiTextService.Setup(x => x.GenerateTableHeader(tableHeader)).Returns(tableHeaderWikiText);
            wikiTextService.Setup(x => x.GenerateTableRow(new[] { applications[0].Name, applications[0].ApplicationGroupName })).Returns(tableRow1);
            wikiTextService.Setup(x => x.GenerateTableRow(new[] { applications[1].Name, applications[1].ApplicationGroupName })).Returns(tableRow2);

            // Act
            var result = service.ExportApplicationDependenciesAsWikiText(applications);

            // Assert
            var expectedWikiText = $"{headerWikiText}{tableHeaderWikiText}{tableRow1}{tableRow2}";
            result.Should().Be(expectedWikiText);
        }

        #endregion

        #region export database dependencies as wiki text

        [Test]
        public void ExportDatabaseDependenciesAsWikiText_DependenciesNull_ReturnEmptyText()
        {
            // Arrange
            List<DatabaseDependency> databases = null;

            // Act
            var result = service.ExportDatabaseDependenciesAsWikiText(databases);

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public void ExportDatabaseDependenciesAsWikiText_HasRedmineProject_ReturnWikiTextWithRedmineLinks()
        {
            // Arrange
            var databases = CreateTwoDatabaseDependenciesOneWithRedmineProject();

            var headerText = ExportFileContentNames.DatabaseDependenciesHeaderText;
            var headerWikiText = "header-wiki-text";
            var tableHeader = new[] { ExportFileContentNames.DatabaseName, ExportFileContentNames.DatabaseGroup };
            var tableHeaderWikiText = "table-header-wiki-text";
            var tableRow1 = "table-row-1-wiki-text";
            var tableRow2 = "table-row-2-wiki-text";
            var database1NameLink = "database-1-name-link";

            wikiTextService.Setup(x => x.GenerateH2(headerText)).Returns(headerWikiText);
            wikiTextService.Setup(x => x.GenerateTableHeader(tableHeader)).Returns(tableHeaderWikiText);
            wikiTextService.Setup(x => x.GenerateWikiLink(databases[0].RedmineProjectName, databases[0].Name)).Returns(database1NameLink);
            wikiTextService.Setup(x => x.GenerateTableRow(new[] { database1NameLink, databases[0].DatabaseGroupName })).Returns(tableRow1);
            wikiTextService.Setup(x => x.GenerateTableRow(new[] { databases[1].Name, databases[1].DatabaseGroupName })).Returns(tableRow2);

            // Act
            var result = service.ExportDatabaseDependenciesAsWikiText(databases);

            // Assert
            var expectedWikiText = $"{headerWikiText}{tableHeaderWikiText}{tableRow1}{tableRow2}";
            result.Should().Be(expectedWikiText);
        }

        [Test]
        public void ExportDatabaseDependenciesAsWikiText_HasNoRedmineProject_ReturnWikiTextWithoutRedmineLinks()
        {
            // Arrange
            var databases = CreateTwoDatabaseDependenciesWithoutRedmineProject();

            var headerText = ExportFileContentNames.DatabaseDependenciesHeaderText;
            var headerWikiText = "header-wiki-text";
            var tableHeader = new[] { ExportFileContentNames.DatabaseName, ExportFileContentNames.DatabaseGroup };
            var tableHeaderWikiText = "table-header-wiki-text";
            var tableRow1 = "table-row-1-wiki-text";
            var tableRow2 = "table-row-2-wiki-text";

            wikiTextService.Setup(x => x.GenerateH2(headerText)).Returns(headerWikiText);
            wikiTextService.Setup(x => x.GenerateTableHeader(tableHeader)).Returns(tableHeaderWikiText);
            wikiTextService.Setup(x => x.GenerateTableRow(new[] { databases[0].Name, databases[0].DatabaseGroupName })).Returns(tableRow1);
            wikiTextService.Setup(x => x.GenerateTableRow(new[] { databases[1].Name, databases[1].DatabaseGroupName })).Returns(tableRow2);

            // Act
            var result = service.ExportDatabaseDependenciesAsWikiText(databases);

            // Assert
            var expectedWikiText = $"{headerWikiText}{tableHeaderWikiText}{tableRow1}{tableRow2}";
            result.Should().Be(expectedWikiText);
        }

        #endregion

        #region export external dependencies as wiki text

        [Test]
        public void ExportExternalDependenciesAsWikiText_DependenciesNull_ReturnEmptyText()
        {
            // Arrange
            List<ExternalDependency> externalDependencies = null;

            // Act
            var result = service.ExportExternalDependenciesAsWikiText(externalDependencies);

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public void ExportExternalDependenciesAsWikiText_NoCondition_ReturnWikiText()
        {
            // Arrange
            var externalDependencies = CreateTwoExternalDependencies();

            var headerText = ExportFileContentNames.ExternalDependenciesHeaderText;
            var headerWikiText = "header-wiki-text";
            var tableHeader = new[] { ExportFileContentNames.DependencyName, ExportFileContentNames.Description }; ;
            var tableHeaderWikiText = "table-header-wiki-text";
            var tableRow1 = "table-row-1-wiki-text";
            var tableRow2 = "table-row-2-wiki-text";

            wikiTextService.Setup(x => x.GenerateH2(headerText)).Returns(headerWikiText);
            wikiTextService.Setup(x => x.GenerateTableHeader(tableHeader)).Returns(tableHeaderWikiText);
            wikiTextService.Setup(x => x.GenerateTableRow(new[] { externalDependencies[0].Name, externalDependencies[0].Description })).Returns(tableRow1);
            wikiTextService.Setup(x => x.GenerateTableRow(new[] { externalDependencies[1].Name, externalDependencies[1].Description })).Returns(tableRow2);

            // Act
            var result = service.ExportExternalDependenciesAsWikiText(externalDependencies);

            // Assert
            var expectedWikiText = $"{headerWikiText}{tableHeaderWikiText}{tableRow1}{tableRow2}";
            result.Should().Be(expectedWikiText);
        }

        #endregion

        #region export nuget package dependencies as wiki text

        [Test]
        public void ExportNugetPackageDependenciesAsWikiText_DependenciesNull_ReturnEmptyText()
        {
            // Arrange
            List<NugetPackageDependency> nugetPackageDependencies = null;

            // Act
            var result = service.ExportNugetPackageDependenciesAsWikiText(nugetPackageDependencies);

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public void ExportNugetPackageDependenciesAsWikiText_NoCondition_ReturnWikiText()
        {
            // Arrange
            var nugetPackageDependencies = CreateTwoNugetPAckageDependencies();

            var headerText = ExportFileContentNames.NugetPackageDependenciesHeaderText;
            var headerWikiText = "header-wiki-text";
            var tableHeader = new[] { ExportFileContentNames.NugetPackageName, ExportFileContentNames.NugetPackageUrl }; ;
            var tableHeaderWikiText = "table-header-wiki-text";
            var tableRow1 = "table-row-1-wiki-text";
            var tableRow2 = "table-row-2-wiki-text";

            wikiTextService.Setup(x => x.GenerateH2(headerText)).Returns(headerWikiText);
            wikiTextService.Setup(x => x.GenerateTableHeader(tableHeader)).Returns(tableHeaderWikiText);
            wikiTextService.Setup(x => x.GenerateTableRow(new[] { nugetPackageDependencies[0].NugetPackageName, nugetPackageDependencies[0].PackageUrl })).Returns(tableRow1);
            wikiTextService.Setup(x => x.GenerateTableRow(new[] { nugetPackageDependencies[1].NugetPackageName, nugetPackageDependencies[1].PackageUrl })).Returns(tableRow2);

            // Act
            var result = service.ExportNugetPackageDependenciesAsWikiText(nugetPackageDependencies);

            // Assert
            var expectedWikiText = $"{headerWikiText}{tableHeaderWikiText}{tableRow1}{tableRow2}";
            result.Should().Be(expectedWikiText);
        }

        #endregion

        #region setup helpers

        static List<ApplicationDependency> CreateTwoApplicationDependenciesOneWithRedmineProject()
        {
            return new List<ApplicationDependency>
            {
                CreateApplicationDependencyWithRedmineProject("test-application-with-redmine-project-1"),
                CreateApplicationDependencyWithoutRedmineProject("test-application-without-redmine-project-1")
            };
        }

        static List<ApplicationDependency> CreateTwoApplicationDependenciesWithoutRedmineProject()
        {
            return new List<ApplicationDependency>
            {
                CreateApplicationDependencyWithoutRedmineProject("test-application-without-redmine-project-1"),
                CreateApplicationDependencyWithoutRedmineProject("test-application-without-redmine-project-2")
            };
        }

        static ApplicationDependency CreateApplicationDependencyWithRedmineProject(string name)
        {
            return new ApplicationDependency
            {
                Name = name,
                ApplicationGroupName = "test-application-group-1",
                RedmineProjectName = "project-name"
            };
        }

        static ApplicationDependency CreateApplicationDependencyWithoutRedmineProject(string name)
        {
            return new ApplicationDependency
            {
                Name = name,
                ApplicationGroupName = "test-application-group"
            };
        }

        static List<DatabaseDependency> CreateTwoDatabaseDependenciesOneWithRedmineProject()
        {
            return new List<DatabaseDependency>
            {
                CreateDatabaseDependencyWithRedmineProject(),
                CreateDatabaseDependencyWithoutRedmineProject("test-database-without-redmine-project-1")
            };
        }

        static List<DatabaseDependency> CreateTwoDatabaseDependenciesWithoutRedmineProject()
        {
            return new List<DatabaseDependency>
            {
                CreateDatabaseDependencyWithoutRedmineProject("test-database-without-redmine-project-1"),
                CreateDatabaseDependencyWithoutRedmineProject("test-database-without-redmine-project-2")
            };
        }

        static DatabaseDependency CreateDatabaseDependencyWithRedmineProject()
        {
            return new DatabaseDependency
            {
                Name = "test-database-with-redmine-project",
                DatabaseGroupName = "test-database-group-1",
                RedmineProjectName = "project-name"
            };
        }

        static DatabaseDependency CreateDatabaseDependencyWithoutRedmineProject(string name)
        {
            return new DatabaseDependency
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

        static List<NugetPackageDependency> CreateTwoNugetPAckageDependencies()
        {
            return new List<NugetPackageDependency> {
                new NugetPackageDependency{
                    NugetPackageName = "nuget-dependency-1",
                    PackageUrl = "nuget-dependency-1-url"
                },
                new NugetPackageDependency{
                    NugetPackageName = "nuget-dependency-2",
                    PackageUrl = "nuget-dependency-2-url"
                }
            };
        }

        #endregion
    }
}