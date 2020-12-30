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

namespace DevPortal.UnitTests.DataTests.Repositories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class DatabaseGroupRepositoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IDataClient> dataClient;

        StrictMock<IApplicationDataRequestFactory> dataRequestFactory;

        StrictMock<ISettings> settings;

        DatabaseGroupRepository repository;

        [SetUp]
        public void Initialize()
        {
            dataClient = new StrictMock<IDataClient>();
            dataRequestFactory = new StrictMock<IApplicationDataRequestFactory>();
            settings = new StrictMock<ISettings>();
            SetupDataClient();
            repository = new DatabaseGroupRepository(dataClient.Object,
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

        #region get environments

        [Test]
        public void GetDatabaseGroups_NoCondition_ReturnDatabaseGroups()
        {
            //Arrange
            var defaultReturnValue = new List<DatabaseGroup>();
            var expectedValue = new List<DatabaseGroup>();
            Mock<IDataRequest> dataRequest = new Mock<IDataRequest>();

            dataRequestFactory.Setup(x => x.GetDatabaseGroups()).Returns(dataRequest.Object);
            dataClient.Setup(x => x.GetCollection(
               dataRequest.Object,
               DataClientMapFactory.DatabaseGroupsMap,
               defaultReturnValue,
               dataRequest.Object.SplitOnParameters))
               .Returns(expectedValue);

            //Act
            var result = repository.GetDatabaseGroups();

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        #endregion

        #region get database group by name

        [Test]
        public void GetDatabaseGroupByName_NoCondition_ReturnEnvironment()
        {
            //Arrange
            var databaseGroupName = "databaseGroupName";
            IDataRequest dataRequest = null;
            DatabaseGroup defaultReturnValue = null;
            var expectedValue = new DatabaseGroup();

            dataRequestFactory.Setup(x => x.GetDatabaseGroupByName(databaseGroupName)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetDatabaseGroupByName(databaseGroupName);

            //Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        #endregion

        #region add database group

        [Test]
        public void AddDatabaseGroup_NoCondition_ReturnDatabaseGroup()
        {
            //Arrange
            var databaseGroup = new DatabaseGroup();
            IDataRequest dataRequest = null;
            const int defaultReturnValue = 0;
            var expectedValue = 1;

            dataRequestFactory.Setup(x => x.AddDatabaseGroup(databaseGroup)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.AddDatabaseGroup(databaseGroup);

            //Assert
            result.Should().Be(expectedValue);
        }

        #endregion

        #region get database group by id

        [Test]
        public void GetDatabaseGroupById_NoCondition_ReturnDatabaseGroup()
        {
            //Arrange
            var databaseGroupId = 1;
            IDataRequest dataRequest = null;
            DatabaseGroup defaultReturnValue = null;
            var expectedValue = new DatabaseGroup();

            dataRequestFactory.Setup(x => x.GetDatabaseGroupById(databaseGroupId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetDatabaseGroupById(databaseGroupId);

            //Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        #endregion

        #region get database group update info

        [Test]
        public void GetDatabaseGroupUpdateInfo_NoCondition_ReturnRecordUpdateInfo()
        {
            //Arrange
            var databaseGroupId = 1;
            IDataRequest dataRequest = null;
            RecordUpdateInfo defaultReturnValue = null;
            var expectedValue = new RecordUpdateInfo();

            dataRequestFactory.Setup(x => x.GetDatabaseGroupUpdateInfo(databaseGroupId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetDatabaseGroupUpdateInfo(databaseGroupId);

            //Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        #endregion

        #region update database group

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void UpdateDatabaseGroup_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var databaseGroup = new DatabaseGroup();
            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.UpdateDatabaseGroup(databaseGroup)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.UpdateDatabaseGroup(databaseGroup);

            //Assert
            result.Should().Be(returnValue);
        }

        #endregion

        #region delete database group

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void DeleteDatabaseGroup_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var databaseGroupId = 1;
            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.DeleteDatabaseGroup(databaseGroupId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.DeleteDatabaseGroup(databaseGroupId);

            //Assert
            result.Should().Be(returnValue);
        }

        #endregion

        #region get database count by database group id

        [Test]
        public void GetDatabaseCountByDatabaseGroupId_NoCondition_ReturnDatabaseGroup()
        {
            //Arrange
            var databaseGroupId = 1;
            IDataRequest dataRequest = null;
            const int defaultReturnValue = 0;
            var expectedValue = 0;

            dataRequestFactory.Setup(x => x.GetDatabaseCountByDatabaseGroupId(databaseGroupId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetDatabaseCountByDatabaseGroupId(databaseGroupId);

            //Assert
            result.Should().Be(expectedValue);
        }

        #endregion
    }
}