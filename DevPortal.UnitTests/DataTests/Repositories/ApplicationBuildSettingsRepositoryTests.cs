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
    public class ApplicationBuildSettingsRepositoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IDataClient> dataClient;

        StrictMock<IApplicationDataRequestFactory> dataRequestFactory;

        StrictMock<ISettings> settings;

        ApplicationBuildSettingsRepository repository;

        [SetUp]
        public void Initialize()
        {
            dataClient = new StrictMock<IDataClient>();
            dataRequestFactory = new StrictMock<IApplicationDataRequestFactory>();
            settings = new StrictMock<ISettings>();
            SetupDataClient();
            repository = new ApplicationBuildSettingsRepository(
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
        public void GetApplicationBuildSettings_NoCondition_ReturnApplicationBuildSettings()
        {
            //Arrange
            var id = 1;
            IDataRequest dataRequest = null;
            ApplicationBuildSettings defaultReturnValue = null;
            var expectedValue = new ApplicationBuildSettings();

            dataRequestFactory.Setup(x => x.GetApplicationBuildSettings(id)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationBuildSettings(id);

            //Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        [Test]
        public void GetApplicationBuildSettingsUpdateInfo_NoCondition_ReturnRecordUpdateInfo()
        {
            //Arrange
            var id = 7;
            RecordUpdateInfo defaultReturnValue = null;
            var expectedValue = new RecordUpdateInfo();
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetApplicationBuildSettingsUpdateInfo(id)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationBuildSettingsUpdateInfo(id);

            //Assert
            result.Should().Be(expectedValue);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void AddApplicationBuildSettings_ExpectedResultsInTestCases_ReturnExpectedResult(bool returnValue)
        {
            //Arrange
            var buildSettings = new ApplicationBuildSettings();
            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.AddApplicationBuildSettings(buildSettings)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.AddApplicationBuildSettings(buildSettings);

            //Assert
            result.Should().Be(returnValue);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void UpdateApplicationBuildSettings_ExpectedResultsInTestCases_ReturnExpectedResult(bool returnValue)
        {
            //Arrange
            var buildSettings = new ApplicationBuildSettings();
            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.UpdateApplicationBuildSettings(buildSettings)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.UpdateApplicationBuildSettings(buildSettings);

            //Assert
            result.Should().Be(returnValue);
        }
    }
}