using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Business.Services;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Model;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class DatabaseReaderServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IDatabaseRepository> databaseRepository;

        StrictMock<IGeneralSettingsService> generalSettingsService;

        DatabaseReaderService service;

        [SetUp]
        public void Initialize()
        {
            databaseRepository = new StrictMock<IDatabaseRepository>();
            generalSettingsService = new StrictMock<IGeneralSettingsService>();

            service = new DatabaseReaderService(
                databaseRepository.Object,
                generalSettingsService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            databaseRepository.VerifyAll();
            generalSettingsService.VerifyAll();
        }

        #endregion

        #region get filtered database redmine projects

        [Test]
        public void GetFilteredDatabaseRedmineProjectListAsync_TableParamNull_ReturnEmptyList()
        {
            // Arrange
            DatabaseRedmineProjectTableParam tableParam = null;

            // Act
            var result = service.GetFilteredDatabaseRedmineProjectListAsync(tableParam);

            // Assert
            result.Result.Should().NotBeNull().And.HaveCount(0);
        }

        [Test]
        public void GetFilteredDatabaseRedmineProjectListAsync_TableParamValid_SetProjectUrlsAndReturnProjects()
        {
            // Arrange
            var tableParam = new DatabaseRedmineProjectTableParam
            {
                start = 1,
                length = 10,
                order = new List<TableOrder>
                {
                    new TableOrder { column = 1, dir = "" }
                },
                columns = new List<TableColumn>
                {
                    new TableColumn { data = "", name = "" }
                },
                SearchText = "text",
                DatabaseGroupId = 45
            };

            var projects = new List<RedmineProject> {
                new RedmineProject{
                    ProjectName = "project"
                }
            };
            var projectUrl = "http://wwww.example.com/project-url";
            var projectUri = new Uri(projectUrl);

            var expectedProjects = new List<RedmineProject> {
                new RedmineProject{
                    ProjectName = "project",
                    ProjectUrl = projectUrl,
                    RepositoryUrl = $"{projectUrl}/repository"
                }
            };

            databaseRepository.Setup(x => x.GetFilteredDatabaseRedmineProjectListAsync(tableParam.start, tableParam.length, tableParam.SortColumn, tableParam.order[0].dir, tableParam.SearchText, tableParam.DatabaseGroupId)).ReturnsAsync(projects);
            generalSettingsService.Setup(x => x.GetRedmineProjectUrl(projects[0].ProjectName)).Returns(projectUri);

            // Act
            var result = service.GetFilteredDatabaseRedmineProjectListAsync(tableParam);

            // Assert
            result.Result.Should().BeEquivalentTo(projects);
        }

        #endregion

        #region get database by database group id

        [Test]
        public void GetDatabaseByDatabaseGroupId_NoCondition_ReturnDatabase()
        {
            // Arrange
            var databaseGroupId = 1;
            var databases = new List<Database>();

            // Act
            databaseRepository.Setup(x => x.GetDatabasesByDatabaseGroupId(databaseGroupId)).Returns(databases);

            var result = service.GetDatabasesByDatabaseGroupId(databaseGroupId);

            // Assert
            result.Should().BeEquivalentTo(databases);
        }

        #endregion

        #region filter databases

        [Test]
        public void FilterDatabases_DatabaseGroupIdIsZeroAndDatabaseNameIsEmpty_ReturnDatabase()
        {
            // Arrange
            var databaseGroupId = 0;
            var databaseName = "";
            ICollection<Database> databases = new List<Database>();

            // Act
            databaseRepository.Setup(x => x.GetDatabases()).Returns(databases);

            var result = service.FilterDatabases(databaseGroupId, databaseName);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void FilterDatabases_DatabaseGroupIdIsZeroAndDatabaseNameIsEmpty_ReturnDatabaseIsNull()
        {
            // Arrange
            var databaseGroupId = 0;
            var databaseName = "";
            ICollection<Database> databases = null;

            // Act
            databaseRepository.Setup(x => x.GetDatabases()).Returns(databases);

            var result = service.FilterDatabases(databaseGroupId, databaseName);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void FilterDatabases_DatabaseGroupIdIsNotZeroAndDatabaseNameIsEmpty_ReturnDatabase()
        {
            // Arrange
            var databaseGroupId = 5;
            var databaseName = "";
            ICollection<Database> databases = new List<Database>();

            // Act
            databaseRepository.Setup(x => x.GetDatabasesByDatabaseGroupId(databaseGroupId)).Returns(databases);

            var result = service.FilterDatabases(databaseGroupId, databaseName);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void FilterDatabases_DatabaseGroupIdIsNotZeroAndDatabaseIsEmpty_ReturnDatabaseIsNull()
        {
            // Arrange
            var databaseGroupId = 5;
            var databaseName = "";
            ICollection<Database> databases = null;

            // Act
            databaseRepository.Setup(x => x.GetDatabasesByDatabaseGroupId(databaseGroupId)).Returns(databases);

            var result = service.FilterDatabases(databaseGroupId, databaseName);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void FilterDatabases_DatabaseGroupIdIsZeroAndDatabaseNameIsNotEmpty_ReturnDatabase()
        {
            // Arrange
            var databaseGroupId = 0;
            var databaseName = "databaseName";
            ICollection<Database> databases = new List<Database>();

            // Act
            databaseRepository.Setup(x => x.GetDatabasesByDatabaseName(databaseName)).Returns(databases);

            var result = service.FilterDatabases(databaseGroupId, databaseName);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void FilterDatabases_DatabaseGroupIdIsZeroAndDatabaseNameIsNotEmpty_ReturnDatabaseIsNull()
        {
            // Arrange
            var databaseGroupId = 0;
            var databaseName = "databaseName";
            ICollection<Database> databases = null;

            // Act
            databaseRepository.Setup(x => x.GetDatabasesByDatabaseName(databaseName)).Returns(databases);

            var result = service.FilterDatabases(databaseGroupId, databaseName);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void FilterDatabases_DatabaseGroupIdIsNotZeroAndDatabaseNameIsNotEmpty_ReturnDatabase()
        {
            // Arrange
            var databaseGroupId = 5;
            var databaseName = "databaseName";
            ICollection<Database> databases = new List<Database>();

            // Act
            databaseRepository.Setup(x => x.GetDatabasesByGroupIdAndName(databaseGroupId, databaseName)).Returns(databases);

            var result = service.FilterDatabases(databaseGroupId, databaseName);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void FilterDatabases_DatabaseGroupIdIsNotZeroAndDatabaseNameIsNotEmpty_ReturnDatabaseIsNull()
        {
            // Arrange
            var databaseGroupId = 5;
            var databaseName = "databaseName";
            ICollection<Database> databases = null;

            // Act
            databaseRepository.Setup(x => x.GetDatabasesByGroupIdAndName(databaseGroupId, databaseName)).Returns(databases);

            var result = service.FilterDatabases(databaseGroupId, databaseName);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region get database by database type id

        [Test]
        public void GetDatabaseByDatabaseTypeId_NoCondition_ReturnDatabase()
        {
            // Arrange
            var databaseTypeId = 1;
            var databases = new List<Database>();

            // Act
            databaseRepository.Setup(x => x.GetDatabaseByDatabaseTypeId(databaseTypeId)).Returns(databases);

            var result = service.GetDatabaseByDatabaseTypeId(databaseTypeId);

            // Assert
            result.Should().BeEquivalentTo(databases);
        }

        #endregion

        #region get filtered databaseList

        [Test]
        public void GetFilteredDatabaseList_TableParamNull_ReturnEmptyList()
        {
            // Arrange
            DatabaseTableParam tableParam = null;

            // Act
            var result = service.GetFilteredDatabaseListAsync(tableParam);

            // Assert
            result.Result.Should().NotBeNull().And.HaveCount(0);
        }

        [Test]
        public void GetFilteredDatabaseList_NoCondition_ReturnFilteredDatabaseList()
        {
            // Arrange
            var tableParam = new DatabaseTableParam
            {
                start = 1,
                length = 10,
                order = new List<TableOrder>
                {
                    new TableOrder { column = 1, dir = "" }
                },
                columns = new List<TableColumn>
                {
                    new TableColumn { data = "", name = "" }
                }
            };
            var databases = new List<Database>();

            databaseRepository.Setup(x => x.GetFilteredDatabaseListAsync(tableParam.start, tableParam.length, tableParam.SortColumn, tableParam.order[0].dir, tableParam.SearchText, tableParam.DatabaseGroupId)).ReturnsAsync(databases);

            // Act
            var result = service.GetFilteredDatabaseListAsync(tableParam);

            // Assert
            result.Result.Should().BeEquivalentTo(databases);
        }

        #endregion

        #region get database

        [Test]
        public void GetDatabase_DatabaseExists_ReturnDatabase()
        {
            // Arrange
            var databaseId = 1;
            var redmineProjectName = "redmine-project-name";
            var redmineProjectUrl = new Uri("http://wwww.example.com/redmine-project-url");
            var database = new Database
            {
                RedmineProjectName = redmineProjectName
            };
            var updateInfo = new RecordUpdateInfo();

            generalSettingsService.Setup(x => x.GetRedmineProjectUrl(redmineProjectName)).Returns(redmineProjectUrl);
            databaseRepository.Setup(x => x.GetDatabase(databaseId)).Returns(database);
            databaseRepository.Setup(x => x.GetDatabaseUpdateInfo(databaseId)).Returns(updateInfo);

            // Act
            var result = service.GetDatabase(databaseId);

            // Assert
            result.Should().Be(database);
            database.RedmineProjectUrl.Should().Be(redmineProjectUrl.ToString());
        }

        [Test]
        public void GetDatabase_DatabaseDoesNotExist_ReturnNull()
        {
            // Arrange
            var databaseId = 1;
            Database database = null;

            databaseRepository.Setup(x => x.GetDatabase(databaseId)).Returns(database);

            // Act

            var result = service.GetDatabase(databaseId);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region get databaseTypes

        [Test]
        public void GetDatabaseTypes_NoCondition_ReturnDatabaseTypes()
        {
            // Arrange
            ICollection<DatabaseType> databaseTypes = new List<DatabaseType>();

            // Act
            databaseRepository.Setup(x => x.GetDatabaseTypes()).Returns(databaseTypes);

            var result = service.GetDatabaseTypes();

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void GetDatabaseTypes_NoCondition_ReturnDatabaseTypesIsNull()
        {
            // Arrange
            ICollection<DatabaseType> databaseTypes = null;

            // Act
            databaseRepository.Setup(x => x.GetDatabaseTypes()).Returns(databaseTypes);

            var result = service.GetDatabaseTypes();

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region get databases

        [Test]
        public void GetDatabases_NoCondition_ReturnDatabases()
        {
            // Arrange
            ICollection<Database> databases = new List<Database>();

            // Act
            databaseRepository.Setup(x => x.GetDatabases()).Returns(databases);

            var result = service.GetDatabases();

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void GetDatabases_NoCondition_ReturnDatabaseIsNull()
        {
            // Arrange
            ICollection<Database> databases = null;

            // Act
            databaseRepository.Setup(x => x.GetDatabases()).Returns(databases);

            var result = service.GetDatabases();

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region get databaseGroups

        [Test]
        public void GetDatabaseGroups_NoCondition_ReturnDatabaseGroups()
        {
            // Arrange
            ICollection<DatabaseGroup> databaseGroups = new List<DatabaseGroup>();

            // Act
            databaseRepository.Setup(x => x.GetDatabaseGroups()).Returns(databaseGroups);

            var result = service.GetDatabaseGroups();

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void GetApplicationGroups_NoCondition_ReturnDatabaseGroupsIsNull()
        {
            // Arrange
            ICollection<DatabaseGroup> databaseGroups = null;

            // Act
            databaseRepository.Setup(x => x.GetDatabaseGroups()).Returns(databaseGroups);

            var result = service.GetDatabaseGroups();

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region get databaseTypes

        [Test]
        public void GetDatabaseTypes_NoCondition_ReturnDatabaseGroups()
        {
            // Arrange
            ICollection<DatabaseType> databaseType = new List<DatabaseType>();

            // Act
            databaseRepository.Setup(x => x.GetDatabaseTypes()).Returns(databaseType);

            var result = service.GetDatabaseTypes();

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void GetDatabaseTypes_NoCondition_ReturnDatabaseGroupsIsNull()
        {
            // Arrange
            ICollection<DatabaseType> databaseType = null;

            // Act
            databaseRepository.Setup(x => x.GetDatabaseTypes()).Returns(databaseType);

            var result = service.GetDatabaseTypes();

            // Assert
            result.Should().BeNull();
        }

        #endregion
    }
}