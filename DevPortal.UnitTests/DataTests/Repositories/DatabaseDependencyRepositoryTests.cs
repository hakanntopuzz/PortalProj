using AB.Data.DapperClient.Abstract;
using AB.Data.DapperClient.Model.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Repositories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.DataTests.Repositories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class DatabaseDependencyRepositoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IDataClient> dataClient;

        StrictMock<IApplicationDataRequestFactory> dataRequestFactory;

        StrictMock<ISettings> settings;

        DatabaseDependencyRepository repository;

        [SetUp]
        public void Initialize()
        {
            dataClient = new StrictMock<IDataClient>();
            dataRequestFactory = new StrictMock<IApplicationDataRequestFactory>();
            settings = new StrictMock<ISettings>();
            SetupDataClient();
            repository = new DatabaseDependencyRepository(dataClient.Object,
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
        public void GetDatabaseDependencyById_NoCondition_ReturnDatabaseDependency()
        {
            //Arrange
            var id = 1;
            IDataRequest dataRequest = null;
            DatabaseDependency defaultReturnValue = null;
            var expectedValue = new DatabaseDependency();

            dataRequestFactory.Setup(x => x.GetDatabaseDependencyById(id)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetDatabaseDependencyById(id);

            //Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        [Test]
        public void GetDatabaseDependencyUpdateInfo_NoCondition_ReturnRecordUpdateInfo()
        {
            //Arrange
            var id = 1;
            IDataRequest dataRequest = null;
            RecordUpdateInfo defaultReturnValue = null;
            var expectedValue = new RecordUpdateInfo();

            dataRequestFactory.Setup(x => x.GetDatabaseDependencyUpdateInfo(id)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetDatabaseDependencyUpdateInfo(id);

            //Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        #region add database dependency

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void AddDatabaseDependency_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var databaseDependency = new DatabaseDependency();
            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.AddDatabaseDependency(databaseDependency)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.AddDatabaseDependency(databaseDependency);

            //Assert
            result.Should().Be(returnValue);
        }

        #endregion

        #region update database dependency

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void UpdateDatabaseDependency_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var databaseDependency = new DatabaseDependency();
            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.UpdateDatabaseDependency(databaseDependency)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.UpdateDatabaseDependency(databaseDependency);

            //Assert
            result.Should().Be(returnValue);
        }

        #endregion

        #region delete database dependency

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void DeleteDatabaseDependency_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var databaseDependencyId = 1;
            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.DeleteDatabaseDependency(databaseDependencyId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.DeleteDatabaseDependency(databaseDependencyId);

            //Assert
            result.Should().Be(returnValue);
        }

        #endregion

        [Test]
        public void GetDatabaseDependenciesByApplicationId_NoCondition_ReturDatabaseList()
        {
            //Arrange
            var applicationId = 3;
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<DatabaseDependency>();
            var expectedValue = new List<DatabaseDependency>();

            dataRequestFactory.Setup(x => x.GetDatabaseDependenciesByApplicationId(applicationId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetDatabaseDependenciesByApplicationId(applicationId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetFullDatabaseDependenciesByApplicationId_NoCondition_ReturDatabaseExportListItems()
        {
            //Arrange
            var applicationId = 3;
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<DatabaseDependenciesExportListItem>();
            var expectedValue = new List<DatabaseDependenciesExportListItem>();

            dataRequestFactory.Setup(x => x.GetFullDatabaseDependenciesByApplicationId(applicationId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetFullDatabaseDependenciesByApplicationId(applicationId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }
    }
}