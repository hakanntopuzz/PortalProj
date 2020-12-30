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
    public class ApplicationEnvironmentRepositoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IDataClient> dataClient;

        StrictMock<IApplicationDataRequestFactory> dataRequestFactory;

        StrictMock<ISettings> settings;

        ApplicationEnvironmentRepository repository;

        [SetUp]
        public void Initialize()
        {
            dataClient = new StrictMock<IDataClient>();
            dataRequestFactory = new StrictMock<IApplicationDataRequestFactory>();
            settings = new StrictMock<ISettings>();
            SetupDataClient();
            repository = new ApplicationEnvironmentRepository(dataClient.Object, dataRequestFactory.Object, settings.Object);
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
        public void GetApplicationEnvironments_NoCondition_ReturnApplicationEnvironments()
        {
            //Arrange
            var applicationId = 6;
            var defaultReturnValue = new List<Model.ApplicationEnvironment>();
            var expectedValue = new List<Model.ApplicationEnvironment>();
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetApplicationEnvironments(applicationId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationEnvironments(applicationId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetEnvironmentsDoesNotExistByApplicationId_NoCondition_ReturnEnvironments()
        {
            //Arrange
            var applicationId = 6;
            var defaultReturnValue = new List<Environment>();
            var expectedValue = new List<Environment>();
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetEnvironmentsDoesNotExistByApplicationId(applicationId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetEnvironmentsDoesNotExistByApplicationId(applicationId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void AddApplicationEnvironment_NoCondition_ReturnTrue()
        {
            //Arrange
            var applicationEnvironment = new ApplicationEnvironment();
            var defaultReturnValue = false;
            var expectedValue = true;
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.AddApplicationEnvironment(applicationEnvironment)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.AddApplicationEnvironment(applicationEnvironment);

            //Assert
            result.Should().BeTrue();
        }

        [Test]
        public void AddApplicationEnvironment_NoCondition_ReturnFalse()
        {
            //Arrange
            var applicationEnvironment = new Model.ApplicationEnvironment();
            var defaultReturnValue = false;
            var expectedValue = false;
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.AddApplicationEnvironment(applicationEnvironment)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.AddApplicationEnvironment(applicationEnvironment);

            //Assert
            result.Should().BeFalse();
        }

        [Test]
        public void GetApplicationEnvironmentById_NoCondition_ReturnNull()
        {
            //Arrange
            var id = 123;
            const ApplicationEnvironment defaultReturnValue = null;

            IDataRequest dataRequest = null;
            dataRequestFactory.Setup(x => x.GetApplicationEnvironmentById(id)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(defaultReturnValue);

            //Act
            var result = repository.GetApplicationEnvironmentById(id);

            //Assert
            result.Should().BeNull();
        }

        [Test]
        public void GetApplicationEnvironmentByEnvironmentId_NoCondition_ReturnNull()
        {
            //Arrange
            var applicationId = 123;
            var environmentId = 123;
            const ApplicationEnvironment defaultReturnValue = null;

            IDataRequest dataRequest = null;
            dataRequestFactory.Setup(x => x.GetApplicationEnvironmentByEnvironmentId(applicationId, environmentId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(defaultReturnValue);

            //Act
            var result = repository.GetApplicationEnvironmentByEnvironmentId(applicationId, environmentId);

            //Assert
            result.Should().BeNull();
        }

        [Test]
        public void UpdateApplicationEnvironment_NoCondition_ReturnFalse()
        {
            //Arrange
            var formModel = new Model.ApplicationEnvironment();
            var defaultReturnValue = false;
            var expectedValue = false;
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.UpdateApplicationEnvironment(formModel)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.UpdateApplicationEnvironment(formModel);

            //Assert
            result.Should().BeFalse();
        }

        [Test]
        public void UpdateApplicationEnvironment_NoCondition_ReturnTrue()
        {
            //Arrange
            var formModel = new Model.ApplicationEnvironment();
            var defaultReturnValue = false;
            var expectedValue = true;
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.UpdateApplicationEnvironment(formModel)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.UpdateApplicationEnvironment(formModel);

            //Assert
            result.Should().BeTrue();
        }

        [Test]
        public void DeleteApplicationEnvironment_NoCondition_ReturnFalse()
        {
            //Arrange
            var id = 8;
            var defaultReturnValue = false;
            var expectedValue = false;
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.DeleteApplicationEnvironment(id)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.DeleteApplicationEnvironment(id);

            //Assert
            result.Should().BeFalse();
        }

        [Test]
        public void DeleteApplicationEnvironment_NoCondition_ReturnTrue()
        {
            //Arrange
            var id = 8;
            var defaultReturnValue = false;
            var expectedValue = true;
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.DeleteApplicationEnvironment(id)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.DeleteApplicationEnvironment(id);

            //Assert
            result.Should().BeTrue();
        }

        #region GetApplicationEnvironmentUpdateInfo

        [Test]
        public void GetApplicationEnvironmentUpdateInfo_NoCondition_ReturnRecordUpdateInfo()
        {
            //Arrange
            var id = 8;
            RecordUpdateInfo defaultReturnValue = null;
            var expectedValue = new RecordUpdateInfo();
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetApplicationEnvironmentUpdateInfo(id)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationEnvironmentUpdateInfo(id);

            //Assert
            result.Should().Be(expectedValue);
        }

        #endregion
    }
}