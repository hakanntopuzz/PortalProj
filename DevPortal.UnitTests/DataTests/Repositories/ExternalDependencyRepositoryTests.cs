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
    public class ExternalDependencyRepositoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IDataClient> dataClient;

        StrictMock<IApplicationDataRequestFactory> dataRequestFactory;

        StrictMock<ISettings> settings;

        ExternalDependencyRepository repository;

        [SetUp]
        public void Initialize()
        {
            dataClient = new StrictMock<IDataClient>();
            dataRequestFactory = new StrictMock<IApplicationDataRequestFactory>();
            settings = new StrictMock<ISettings>();
            SetupDataClient();
            repository = new ExternalDependencyRepository(dataClient.Object,
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

        #region get external dependency by id

        [Test]
        public void GetExternalDependencyById_NoCondition_ReturnExternalDependency()
        {
            //Arrange
            var id = 1;
            IDataRequest dataRequest = null;
            ExternalDependency defaultReturnValue = null;
            var expectedValue = new ExternalDependency();

            dataRequestFactory.Setup(x => x.GetExternalDependencyById(id)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetExternalDependencyById(id);

            //Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        #endregion

        #region get external dependency update info

        [Test]
        public void GetExternalDependencyUpdateInfo_NoCondition_ReturnRecordUpdateInfo()
        {
            //Arrange
            var id = 1;
            IDataRequest dataRequest = null;
            RecordUpdateInfo defaultReturnValue = null;
            var expectedValue = new RecordUpdateInfo();

            dataRequestFactory.Setup(x => x.GetExternalDependencyUpdateInfo(id)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetExternalDependencyUpdateInfo(id);

            //Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        #endregion

        #region add external dependency

        [Test]
        public void AddExternalDependency_NoCondition_ReturnExternalDependencyId()
        {
            //Arrange
            var externalDependency = new ExternalDependency();
            IDataRequest dataRequest = null;
            const int defaultReturnValue = 0;
            var expectedValue = 1;

            dataRequestFactory.Setup(x => x.AddExternalDependency(externalDependency)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.AddExternalDependency(externalDependency);

            //Assert
            result.Should().Be(expectedValue);
        }

        #endregion

        #region update external dependency

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void UpdateExternalDependency_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var externalDependency = new ExternalDependency();
            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.UpdateExternalDependency(externalDependency)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.UpdateExternalDependency(externalDependency);

            //Assert
            result.Should().Be(returnValue);
        }

        #endregion

        #region delete external dependency

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void DeleteExternalDependency_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var externalDependency = new ExternalDependency();
            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.DeleteExternalDependency(externalDependency)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.DeleteExternalDependency(externalDependency);

            //Assert
            result.Should().Be(returnValue);
        }

        #endregion

        #region get external dependencies

        [Test]
        public void GetExternalDependencies_NoCondition_ReturnExternalExportListItems()
        {
            //Arrange
            var applicationId = 1;
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<ExternalDependenciesExportListItem>();
            var expectedValue = new List<ExternalDependenciesExportListItem>();

            dataRequestFactory.Setup(x => x.GetExternalDependencies(applicationId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetExternalDependencies(applicationId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        #endregion
    }
}