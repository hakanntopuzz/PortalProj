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
    public class ApplicationNugetPackageRepositoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IDataClient> dataClient;

        StrictMock<IApplicationDataRequestFactory> dataRequestFactory;

        StrictMock<ISettings> settings;

        ApplicationNugetPackageRepository repository;

        [SetUp]
        public void Initialize()
        {
            dataClient = new StrictMock<IDataClient>();
            dataRequestFactory = new StrictMock<IApplicationDataRequestFactory>();
            settings = new StrictMock<ISettings>();
            SetupDataClient();

            repository = new ApplicationNugetPackageRepository(
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

        #region GetNugetPackages

        [Test]
        public void GetNugetPackages_NoCondition_ReturnNugetPackages()
        {
            //Arrange
            var applicationId = 6;
            var defaultReturnValue = new List<ApplicationNugetPackage>();
            var expectedValue = new List<ApplicationNugetPackage>();
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetNugetPackages(applicationId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetNugetPackages(applicationId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        #endregion

        #region GetApplicationNugetPackageById

        [Test]
        public void GetApplicationNugetPackageById_NoCondition_ReturnNugetPackage()
        {
            //Arrange
            IDataRequest dataRequest = null;
            var packageId = 1;
            ApplicationNugetPackage defaultReturnValue = null;
            var expectedValue = new ApplicationNugetPackage();

            dataRequestFactory.Setup(x => x.GetApplicationNugetPackageById(packageId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationNugetPackageById(packageId);

            //Assert
            result.Should().Be(expectedValue);
        }

        #endregion

        #region GetPackageUpdateInfo

        [Test]
        public void GetPackageUpdateInfo_NoCondition_ReturnNugetPackage()
        {
            //Arrange
            IDataRequest dataRequest = null;
            var packageId = 1;
            RecordUpdateInfo defaultReturnValue = null;
            var expectedValue = new RecordUpdateInfo();

            dataRequestFactory.Setup(x => x.GetPackageUpdateInfo(packageId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetPackageUpdateInfo(packageId);

            //Assert
            result.Should().Be(expectedValue);
        }

        #endregion

        #region AddApplicationNugetPackage

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void AddApplicationNugetPackage_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var package = new ApplicationNugetPackage();

            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.AddApplicationNugetPackage(package)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.AddApplicationNugetPackage(package);

            //Assert
            result.Should().Be(returnValue);
        }

        #endregion

        #region UpdateApplicationNugetPackage

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void UpdateApplicationNugetPackage_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var package = new ApplicationNugetPackage();

            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.UpdateApplicationNugetPackage(package)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.UpdateApplicationNugetPackage(package);

            //Assert
            result.Should().Be(returnValue);
        }

        #endregion

        #region GetApplicationNugetPackageByName

        [Test]
        public void GetApplicationNugetPackageByName_NoCondition_ReturnApplicationNugetPackage()
        {
            //Arrange
            var packageName = "name";
            ApplicationNugetPackage defaultReturnValue = null;
            var expectedValue = new ApplicationNugetPackage();
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetApplicationNugetPackageByName(packageName)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationNugetPackageByName(packageName);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        #endregion

        #region DeleteApplicationNugetPackage

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void DeleteApplicationNugetPackage_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var id = 8;
            var defaultReturnValue = false;
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.DeleteApplicationNugetPackage(id)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.DeleteApplicationNugetPackage(id);

            //Assert
            result.Should().Be(returnValue);
        }

        #endregion
    }
}