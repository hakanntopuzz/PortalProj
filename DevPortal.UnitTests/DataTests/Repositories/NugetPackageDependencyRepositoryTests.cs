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
    public class NugetPackageDependencyRepositoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IDataClient> dataClient;

        StrictMock<IApplicationDataRequestFactory> dataRequestFactory;

        StrictMock<ISettings> settings;

        NugetPackageDependencyRepository repository;

        [SetUp]
        public void Initialize()
        {
            dataClient = new StrictMock<IDataClient>();
            dataRequestFactory = new StrictMock<IApplicationDataRequestFactory>();
            settings = new StrictMock<ISettings>();
            SetupDataClient();
            repository = new NugetPackageDependencyRepository(dataClient.Object,
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

        #region GetNugetPackageDependencyById

        [Test]
        public void GetNugetPackageDependencyById_NoCondition_ReturnNugetPackageDependency()
        {
            //Arrange
            var id = 37;
            IDataRequest dataRequest = null;
            NugetPackageDependency defaultReturnValue = null;
            var expectedValue = new NugetPackageDependency();

            dataRequestFactory.Setup(x => x.GetNugetPackageDependenciesById(id)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetNugetPackageDependencyById(id);

            //Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        #endregion

        #region GetNugetPackageDependencyUpdateInfo

        [Test]
        public void GetNugetPackageDependencyUpdateInfo_NoCondition_ReturnNugetPackageDependency()
        {
            //Arrange
            var id = 37;
            IDataRequest dataRequest = null;
            RecordUpdateInfo defaultReturnValue = null;
            var expectedValue = new RecordUpdateInfo();

            dataRequestFactory.Setup(x => x.GetNugetPackageDependencyUpdateInfo(id)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetNugetPackageDependencyUpdateInfo(id);

            //Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        #endregion

        #region AddNugetPackageDependency

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void AddNugetPackageDependency_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var nugetPackageDependency = new NugetPackageDependency();
            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.AddNugetPackageDependency(nugetPackageDependency)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.AddNugetPackageDependency(nugetPackageDependency);

            //Assert
            result.Should().Be(returnValue);
        }

        #endregion

        #region DeleteNugetPackageDependency

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void DeleteApplicationDependency_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var nugetPackageDependencyId = 15;
            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.DeleteNugetPackageDependency(nugetPackageDependencyId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.DeleteNugetPackageDependency(nugetPackageDependencyId);

            //Assert
            result.Should().Be(returnValue);
        }

        #endregion
    }
}