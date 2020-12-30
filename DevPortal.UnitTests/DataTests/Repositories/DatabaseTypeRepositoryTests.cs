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
    public class DatabaseTypeRepositoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IDataClient> dataClient;

        StrictMock<IApplicationDataRequestFactory> dataRequestFactory;

        StrictMock<ISettings> settings;

        DatabaseTypeRepository repository;

        [SetUp]
        public void Initialize()
        {
            dataClient = new StrictMock<IDataClient>();
            dataRequestFactory = new StrictMock<IApplicationDataRequestFactory>();
            settings = new StrictMock<ISettings>();
            SetupDataClient();
            repository = new DatabaseTypeRepository(dataClient.Object,
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

        #region get database types

        [Test]
        public void GetDatabaseTypes_NoCondition_ReturnDatabaseTypes()
        {
            //Arrange
            var defaultReturnValue = new List<DatabaseType>();
            var expectedValue = new List<DatabaseType>();
            Mock<IDataRequest> dataRequest = new Mock<IDataRequest>();

            dataRequestFactory.Setup(x => x.GetDatabaseTypes()).Returns(dataRequest.Object);
            dataClient.Setup(x => x.GetCollection(
               dataRequest.Object,
               DataClientMapFactory.DatabaseTypesMap,
               defaultReturnValue,
               dataRequest.Object.SplitOnParameters))
               .Returns(expectedValue);

            //Act
            var result = repository.GetDatabaseTypes();

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        #endregion

        #region add database type

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void AddDatabaseType_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var databaseType = new DatabaseType();
            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.AddDatabaseType(databaseType)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.AddDatabaseType(databaseType);

            //Assert
            result.Should().Be(returnValue);
        }

        #endregion

        #region update database type

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void UpdateDatabaseType_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var databaseType = new DatabaseType();
            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.UpdateDatabaseType(databaseType)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.UpdateDatabaseType(databaseType);

            //Assert
            result.Should().Be(returnValue);
        }

        #endregion

        #region delete database type

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void DeleteDatabaseType_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var databaseTypeId = 1;
            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.DeleteDatabaseType(databaseTypeId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.DeleteDatabaseType(databaseTypeId);

            //Assert
            result.Should().Be(returnValue);
        }

        #endregion

        #region get database type by name

        [Test]
        public void GetDatabaseTypeByName_NoCondition_ReturnDatabaseType()
        {
            //Arrange
            var databaseTypeName = "databaseTypeName";
            IDataRequest dataRequest = null;
            DatabaseType defaultReturnValue = null;
            var expectedValue = new DatabaseType();

            dataRequestFactory.Setup(x => x.GetDatabaseTypeByName(databaseTypeName)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetDatabaseTypeByName(databaseTypeName);

            //Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        #endregion

        #region get database type by id

        [Test]
        public void GetDatabaseTypeById_NoCondition_ReturnDatabaseType()
        {
            //Arrange
            var databasetypeId = 1;
            IDataRequest dataRequest = null;
            DatabaseType defaultReturnValue = null;
            var expectedValue = new DatabaseType();

            dataRequestFactory.Setup(x => x.GetDatabaseTypeById(databasetypeId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetDatabaseTypeById(databasetypeId);

            //Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        #endregion

        #region get database type update info

        [Test]
        public void GetDatabaseTypeUpdateInfo_NoCondition_ReturnRecordUpdateInfo()
        {
            //Arrange
            var databasetypeId = 1;
            IDataRequest dataRequest = null;
            RecordUpdateInfo defaultReturnValue = null;
            var expectedValue = new RecordUpdateInfo();

            dataRequestFactory.Setup(x => x.GetDatabaseTypeUpdateInfo(databasetypeId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetDatabaseTypeUpdateInfo(databasetypeId);

            //Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        #endregion

        #region get database count by database type id

        [Test]
        public void GetDatabaseCountByDatabaseTypeId_NoCondition_ReturnDatabaseType()
        {
            //Arrange
            var databasetypeId = 1;
            IDataRequest dataRequest = null;
            const int defaultReturnValue = 0;
            var expectedValue = 0;

            dataRequestFactory.Setup(x => x.GetDatabaseCountByDatabaseTypeId(databasetypeId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetDatabaseCountByDatabaseTypeId(databasetypeId);

            //Assert
            result.Should().Be(expectedValue);
        }

        #endregion
    }
}