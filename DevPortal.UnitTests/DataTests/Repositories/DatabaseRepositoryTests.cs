using AB.Data.DapperClient.Abstract;
using AB.Data.DapperClient.Model.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Factories;
using DevPortal.Data.Repositories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.UnitTests.DataTests.Repositories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class DatabaseRepositoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IDataClient> dataClient;

        StrictMock<IApplicationDataRequestFactory> dataRequestFactory;

        StrictMock<ISettings> settings;

        DatabaseRepository repository;

        [SetUp]
        public void Initialize()
        {
            dataClient = new StrictMock<IDataClient>();
            dataRequestFactory = new StrictMock<IApplicationDataRequestFactory>();
            settings = new StrictMock<ISettings>();
            SetupDataClient();
            repository = new DatabaseRepository(
                dataClient.Object,
                dataRequestFactory.Object,
                settings.Object);
        }

        void SetupDataClient()
        {
            const string devPortalDbConnectionString = "devPortalDbConnectionString";
            settings.SetupGet(x => x.DevPortalDbConnectionString).Returns(devPortalDbConnectionString);
            dataClient.Setup(x => x.SetConnectionString(devPortalDbConnectionString));
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            dataClient.VerifyAll();
            dataRequestFactory.VerifyAll();
            settings.VerifyAll();
        }

        #endregion

        [Test]
        public void GetDatabaseGroups_NoCondition_ReturnDatabaseGroups()
        {
            //Arrange
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<DatabaseGroup>();
            var expectedValue = new List<DatabaseGroup>();

            dataRequestFactory.Setup(x => x.GetDatabaseGroups()).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetDatabaseGroups();

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetDatabaseTypes_NoCondition_ReturnDatabaseTypes()
        {
            //Arrange
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<DatabaseType>();
            var expectedValue = new List<DatabaseType>();

            dataRequestFactory.Setup(x => x.GetDatabaseTypes()).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetDatabaseTypes();

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetDatabases_NoCondition_ReturnDatabases()
        {
            //Arrange
            var defaultReturnValue = new List<Database>();
            var expectedValue = new List<Database>();
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetDatabases()).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetDatabases();

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public async Task GetFilteredDatabaseListAsync_NoCondition_ReturnDatabases()
        {
            //Arrange
            int skip = 1;
            int take = 1;
            string orderBy = "orderBy";
            string orderDir = "orderDir";
            string searchText = "searchText";
            int databaseGroupId = 1;
            var defaultReturnValue = new List<Database>();
            var expectedValue = new List<Database>();

            Mock<IDataRequest> dataRequest = new Mock<IDataRequest>();

            dataRequestFactory.Setup(x => x.GetFilteredDatabaseList(skip, take, orderBy, orderDir, searchText, databaseGroupId)).Returns(dataRequest.Object);

            dataClient.Setup(x => x.GetCollectionAsync<Database, RecordUpdateInfo, Database>(
                dataRequest.Object,
                DataClientMapFactory.DatabasesMap,
                defaultReturnValue,
                dataRequest.Object.SplitOnParameters)).ReturnsAsync(expectedValue);

            //Act
            var result = await repository.GetFilteredDatabaseListAsync(skip, take, orderBy, orderDir, searchText, databaseGroupId);

            //Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        [Test]
        public void GetDatabase_NoCondition_ReturnDatabase()
        {
            //Arrange
            var databaseId = 13;
            IDataRequest dataRequest = null;
            var defaultReturnValue = new Database();
            var expectedValue = new Database();

            dataRequestFactory.Setup(x => x.GetDatabase(databaseId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, SetupAny<Database>())).Returns(expectedValue);

            //Act
            var result = repository.GetDatabase(databaseId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetDatabaseUpdateInfo_NoCondition_ReturnRecordUpdateInfo()
        {
            //Arrange
            var databaseId = 3;
            IDataRequest dataRequest = null;
            var defaultReturnValue = new RecordUpdateInfo();
            var expectedValue = new RecordUpdateInfo();

            dataRequestFactory.Setup(x => x.GetDatabaseUpdateInfo(databaseId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, SetupAny<RecordUpdateInfo>())).Returns(expectedValue);

            //Act
            var result = repository.GetDatabaseUpdateInfo(databaseId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void UpdateDatabase_NoCondition_ReturnTrue()
        {
            //Arrange
            var database = new Database();
            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.UpdateDatabase(database)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(true);

            //Act
            var result = repository.UpdateDatabase(database);

            //Assert
            result.Should().BeTrue();
        }

        [Test]
        public void UpdateDatabase_NoCondition_ReturnFalse()
        {
            //Arrange
            var database = new Database();
            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.UpdateDatabase(database)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(false);

            //Act
            var result = repository.UpdateDatabase(database);

            //Assert
            result.Should().BeFalse();
        }

        [Test]
        public void GetDatabaseByDatabaseName_NoCondition_ReturDatabaseList()
        {
            //Arrange
            string name = "name";
            IDataRequest dataRequest = null;
            var defaultReturnValue = new Database();
            var expectedValue = new Database();

            dataRequestFactory.Setup(x => x.GetDatabaseByDatabaseName(name)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, SetupAny<Database>())).Returns(expectedValue);

            //Act
            var result = repository.GetDatabaseByDatabaseName(name);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetDatabaseByDatabaseTypeId_NoCondition_ReturDatabaseList()
        {
            //Arrange
            var databaseTypeId = 3;
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<Database>();
            var expectedValue = new List<Database>();

            dataRequestFactory.Setup(x => x.GetDatabaseByDatabaseTypeId(databaseTypeId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetDatabaseByDatabaseTypeId(databaseTypeId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void AddDatabase_NoCondition_ReturnDatabase()
        {
            //Arrange
            var database = new Database();
            IDataRequest dataRequest = null;
            var expectedValue = new int();

            dataRequestFactory.Setup(x => x.AddDatabase(database)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, SetupAny<int>())).Returns(expectedValue);

            //Act
            var result = repository.AddDatabase(database);

            //Assert
            result.Should().Be(expectedValue);
        }

        [Test]
        public void DeleteDatabase_NoCondition_ReturnFalse()
        {
            //Arrange
            var databaseId = 2;
            var defaultReturnValue = false;
            var expectedValue = false;
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.DeleteDatabase(databaseId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.DeleteDatabase(databaseId);

            //Assert
            result.Should().BeFalse();
        }

        [Test]
        public void DeleteDatabase_NoCondition_ReturnTrue()
        {
            //Arrange
            var databaseId = 2;
            var defaultReturnValue = false;
            var expectedValue = true;
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.DeleteDatabase(databaseId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.DeleteDatabase(databaseId);

            //Assert
            result.Should().BeTrue();
        }

        [Test]
        public void GetDatabaseByDatabaseGroupId_NoCondition_ReturDatabaseList()
        {
            //Arrange
            var databaseGroupId = 3;
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<Database>();
            var expectedValue = new List<Database>();

            dataRequestFactory.Setup(x => x.GetDatabasesByDatabaseGroupId(databaseGroupId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetDatabasesByDatabaseGroupId(databaseGroupId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetDatabasesByDatabaseName_NoCondition_ReturDatabaseList()
        {
            //Arrange
            var databaseName = "databaseName";
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<Database>();
            var expectedValue = new List<Database>();

            dataRequestFactory.Setup(x => x.GetDatabasesByDatabaseName(databaseName)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetDatabasesByDatabaseName(databaseName);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetDatabasesByGroupIdAndName_NoCondition_ReturDatabaseList()
        {
            //Arrange
            var databaseGroupId = 3;
            var databaseName = "databaseName";
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<Database>();
            var expectedValue = new List<Database>();

            dataRequestFactory.Setup(x => x.GetDatabasesByGroupIdAndName(databaseGroupId, databaseName)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetDatabasesByGroupIdAndName(databaseGroupId, databaseName);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public async Task GetFilteredDatabaseRedmineProjectListAsync_NoCondition_ReturnRedmineProjects()
        {
            //Arrange
            int skip = 1;
            int take = 1;
            string orderBy = "orderBy";
            string orderDir = "orderDir";
            string searchText = "searchText";
            int databaseGroupId = 45;

            var defaultReturnValue = new List<RedmineProject>();
            var expectedValue = new List<RedmineProject>();
            var dataRequest = new Mock<IDataRequest>();

            dataRequestFactory.Setup(x => x.GetFilteredDatabaseRedmineProjectList(skip, take, orderBy, orderDir, searchText, databaseGroupId)).Returns(dataRequest.Object);
            dataClient.Setup(x => x.GetCollectionAsync(dataRequest.Object, SetupAny<List<RedmineProject>>())).ReturnsAsync(expectedValue);

            //Act
            var result = await repository.GetFilteredDatabaseRedmineProjectListAsync(skip, take, orderBy, orderDir, searchText, databaseGroupId);

            //Assert
            result.Should().BeEquivalentTo(expectedValue);
        }
    }
}