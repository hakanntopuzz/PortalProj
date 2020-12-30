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
    public class ApplicationSvnRepositoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IDataClient> dataClient;

        StrictMock<IApplicationDataRequestFactory> dataRequestFactory;

        StrictMock<ISettings> settings;

        ApplicationSvnRepository repository;

        [SetUp]
        public void Initialize()
        {
            dataClient = new StrictMock<IDataClient>();
            dataRequestFactory = new StrictMock<IApplicationDataRequestFactory>();
            settings = new StrictMock<ISettings>();
            SetupDataClient();
            repository = new ApplicationSvnRepository(
                dataClient.Object,
                dataRequestFactory.Object,
                settings.Object
            );
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
        [TestCase(true)]
        [TestCase(false)]
        public void AddApplicationSvnRepository_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var svnRepository = new SvnRepository();
            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.AddApplicationSvnRepository(svnRepository)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.AddApplicationSvnRepository(svnRepository);

            //Assert
            result.Should().Be(returnValue);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void UpdateApplicationSvnRepository_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var svnRepository = new SvnRepository();
            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.UpdateApplicationSvnRepository(svnRepository)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.UpdateApplicationSvnRepository(svnRepository);

            //Assert
            result.Should().Be(returnValue);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void DeleteApplicationSvnRepository_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var svnRepositoryId = 1;
            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.DeleteApplicationSvnRepository(svnRepositoryId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.DeleteApplicationSvnRepository(svnRepositoryId);

            //Assert
            result.Should().Be(returnValue);
        }

        [Test]
        public void DeleteApplicationSvnRepository_NoCondition_ReturnSvnRepository()
        {
            //Arrange
            var svnRepositoryId = 1;
            IDataRequest dataRequest = null;
            SvnRepository defaultReturnValue = null;
            var svnRepository = new SvnRepository();

            dataRequestFactory.Setup(x => x.GetApplicationSvnRepositoryById(svnRepositoryId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(svnRepository);

            //Act
            var result = repository.GetApplicationSvnRepositoryById(svnRepositoryId);

            //Assert
            result.Should().Be(svnRepository);
        }

        [Test]
        public void GetSvnRepositoryTypes_NoCondition_ReturnSvnRepositoryTypes()
        {
            //Arrange
            IDataRequest dataRequest = null;
            var defaultValue = new List<SvnRepositoryType>();
            var expectedValue = new List<SvnRepositoryType>();

            dataRequestFactory.Setup(x => x.GetSvnRepositoryTypes()).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultValue)).Returns(expectedValue);

            //Act
            var result = repository.GetSvnRepositoryTypes();

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetApplicationSvnRepositoryUpdateInfo_NoCondition_ReturnSvnRepositoryTypes()
        {
            //Arrange
            int repositoryId = 85;
            IDataRequest dataRequest = null;
            RecordUpdateInfo defaultValue = null;
            var expectedValue = new RecordUpdateInfo();

            dataRequestFactory.Setup(x => x.GetApplicationSvnRepositoryUpdateInfo(repositoryId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationSvnRepositoryUpdateInfo(repositoryId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }
    }
}