using AB.Data.DapperClient.Abstract;
using AB.Data.DapperClient.Model.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Repositories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;

namespace DevPortal.UnitTests.DataTests.Repositories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationDependencyRepositoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IDataClient> dataClient;

        StrictMock<IApplicationDataRequestFactory> dataRequestFactory;

        StrictMock<ISettings> settings;

        ApplicationDependencyRepository repository;

        [SetUp]
        public void Initialize()
        {
            dataClient = new StrictMock<IDataClient>();
            dataRequestFactory = new StrictMock<IApplicationDataRequestFactory>();
            settings = new StrictMock<ISettings>();
            SetupDataClient();
            repository = new ApplicationDependencyRepository(dataClient.Object,
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

        #region add database dependency

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void AddDatabaseDependency_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var applicationDependency = new ApplicationDependency();
            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.AddApplicationDependency(applicationDependency)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.AddApplicationDependency(applicationDependency);

            //Assert
            result.Should().Be(returnValue);
        }

        #endregion

        #region get application dependency by id

        [Test]
        public void GetApplicationDependencyById_NoCondition_ReturnDatabaseDependency()
        {
            //Arrange
            var id = 1;
            IDataRequest dataRequest = null;
            ApplicationDependency defaultReturnValue = null;
            var expectedValue = new ApplicationDependency();

            dataRequestFactory.Setup(x => x.GetApplicationDependencyById(id)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationDependencyById(id);

            //Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        #endregion

        #region get application dependency update info

        [Test]
        public void GetApplicationDependencyUpdateInfo_NoCondition_ReturnTrueAndFalse()
        {
            //Arrange
            var id = 13;
            IDataRequest dataRequest = null;
            RecordUpdateInfo defaultReturnValue = null;
            var expectedValue = new RecordUpdateInfo();

            dataRequestFactory.Setup(x => x.GetApplicationDependencyUpdateInfo(id)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationDependencyUpdateInfo(id);

            //Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        #endregion

        #region update application dependency

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void UpdateApplicationDependency_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var applicationDependency = new ApplicationDependency();
            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.UpdateApplicationDependency(applicationDependency)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.UpdateApplicationDependency(applicationDependency);

            //Assert
            result.Should().Be(returnValue);
        }

        #endregion

        #region delete application dependency

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void DeleteApplicationDependency_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var applicationDependencyId = 15;
            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.DeleteApplicationDependency(applicationDependencyId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.DeleteApplicationDependency(applicationDependencyId);

            //Assert
            result.Should().Be(returnValue);
        }

        #endregion
    }
}