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
    public class EnvironmentRepositoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IDataClient> dataClient;

        StrictMock<IApplicationDataRequestFactory> dataRequestFactory;

        StrictMock<ISettings> settings;

        EnvironmentRepository repository;

        [SetUp]
        public void Initialize()
        {
            dataClient = new StrictMock<IDataClient>();
            dataRequestFactory = new StrictMock<IApplicationDataRequestFactory>();
            settings = new StrictMock<ISettings>();
            SetupDataClient();
            repository = new EnvironmentRepository(dataClient.Object,
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
        public void GetEnvironments_NoCondition_ReturnEnvironments()
        {
            //Arrange
            var defaultReturnValue = new List<Model.Environment>();
            var expectedValue = new List<Model.Environment>();
            Mock<IDataRequest> dataRequest = new Mock<IDataRequest>();

            dataRequestFactory.Setup(x => x.GetEnvironments()).Returns(dataRequest.Object);
            dataClient.Setup(x => x.GetCollection(
               dataRequest.Object,
               DataClientMapFactory.EnvironmentsMap,
               defaultReturnValue,
               dataRequest.Object.SplitOnParameters))
               .Returns(expectedValue);

            //Act
            var result = repository.GetEnvironments();

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        #endregion

        #region get environment by id

        [Test]
        public void GetEnvironmentById_NoCondition_ReturnEnvironment()
        {
            //Arrange
            var environmentId = 1;
            IDataRequest dataRequest = null;
            Environment defaultReturnValue = null;
            var expectedValue = new Environment();

            dataRequestFactory.Setup(x => x.GetEnvironmentById(environmentId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetEnvironmentById(environmentId);

            //Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        #endregion

        #region get environment update info

        [Test]
        public void GetEnvironmentUpdateInfo_NoCondition_ReturnRecordUpdateInfo()
        {
            //Arrange
            var environmentId = 1;
            IDataRequest dataRequest = null;
            RecordUpdateInfo defaultReturnValue = null;
            var expectedValue = new RecordUpdateInfo();

            dataRequestFactory.Setup(x => x.GetEnvironmentUpdateInfo(environmentId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetEnvironmentUpdateInfo(environmentId);

            //Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        #endregion

        #region get environment by name

        [Test]
        public void GetEnvironmentByName_NoCondition_ReturnEnvironment()
        {
            //Arrange
            var environmentName = "environmentName";
            IDataRequest dataRequest = null;
            Environment defaultReturnValue = null;
            var expectedValue = new Environment();

            dataRequestFactory.Setup(x => x.GetEnvironmentByName(environmentName)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetEnvironmentByName(environmentName);

            //Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        #endregion

        #region add environment

        [Test]
        public void AddEnvironment_NoCondition_ReturnEnvironment()
        {
            //Arrange
            var environment = new Environment();
            IDataRequest dataRequest = null;
            const int defaultReturnValue = 0;
            var expectedValue = 1;

            dataRequestFactory.Setup(x => x.AddEnvironment(environment)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.AddEnvironment(environment);

            //Assert
            result.Should().Be(expectedValue);
        }

        #endregion

        #region update environment

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void UpdateEnvironment_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var environment = new Environment();
            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.UpdateEnvironment(environment)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.UpdateEnvironment(environment);

            //Assert
            result.Should().Be(returnValue);
        }

        #endregion

        #region delete environment

        [Test]
        public void DeleteEnvironment_NoCondition_ReturnBoolean()
        {
            //Arrange
            var environmentId = 13;
            IDataRequest dataRequest = null;
            const bool defaultReturnValue = false;
            var expectedValue = false;

            dataRequestFactory.Setup(x => x.DeleteEnvironment(environmentId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.DeleteEnvironment(environmentId);

            //Assert
            result.Should().Be(expectedValue);
        }

        #endregion

        #region get application environment count by environmentId

        [Test]
        public void GetApplicationEnvironmentCountByEnvironmentId_NoCondition_ReturnDatabaseType()
        {
            //Arrange
            var environmentId = 13;
            IDataRequest dataRequest = null;
            const int defaultReturnValue = 0;
            var expectedValue = 0;

            dataRequestFactory.Setup(x => x.GetApplicationEnvironmentCountByEnvironmentId(environmentId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationEnvironmentCountByEnvironmentId(environmentId);

            //Assert
            result.Should().Be(expectedValue);
        }

        #endregion
    }
}