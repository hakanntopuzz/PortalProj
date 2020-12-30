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
    public class GeneralSettingsRepositoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IDataClient> dataClient;

        StrictMock<IApplicationDataRequestFactory> dataRequestFactory;

        StrictMock<ISettings> settings;

        GeneralSettingsRepository repository;

        [SetUp]
        public void Initialize()
        {
            dataClient = new StrictMock<IDataClient>();
            dataRequestFactory = new StrictMock<IApplicationDataRequestFactory>();
            settings = new StrictMock<ISettings>();
            SetupDataClient();
            repository = new GeneralSettingsRepository(dataClient.Object, dataRequestFactory.Object, settings.Object);
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

        #region get general settings

        [Test]
        public void GetGeneralSettings_NoCondition_ReturnGeneralSettings()
        {
            //Arrange
            IDataRequest dataRequest = null;
            var defaultValue = new GeneralSettings();
            var expectedValue = new GeneralSettings();

            dataRequestFactory.Setup(x => x.GetGeneralSettings()).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, SetupAny<GeneralSettings>())).Returns(expectedValue);

            //Act
            var result = repository.GetGeneralSettings();

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        #endregion

        #region update general settings

        [Test]
        public void UpdateGeneralSettings_NoCondition_ReturnBool()
        {
            //Arrange
            IDataRequest dataRequest = null;
            var generalSettings = new GeneralSettings();
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.UpdateGeneralSettings(generalSettings)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(true);

            //Act
            var result = repository.UpdateGeneralSettings(generalSettings);

            //Assert
            result.Should().BeTrue();
        }

        #endregion
    }
}