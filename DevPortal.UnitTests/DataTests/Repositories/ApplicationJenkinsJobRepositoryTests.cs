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
    public class ApplicationJenkinsJobRepositoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IDataClient> dataClient;

        StrictMock<IApplicationDataRequestFactory> dataRequestFactory;

        StrictMock<ISettings> settings;

        ApplicationJenkinsJobRepository repository;

        [SetUp]
        public void Initialize()
        {
            dataClient = new StrictMock<IDataClient>();
            dataRequestFactory = new StrictMock<IApplicationDataRequestFactory>();
            settings = new StrictMock<ISettings>();
            SetupDataClient();
            repository = new ApplicationJenkinsJobRepository(
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

        #region GetJenkinsJobTypes

        [Test]
        public void GetJenkinsJobTypes_NoCondition_ReturnJenkinsJobTypeList()
        {
            //Arrange
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<JenkinsJobType>();
            var expectedValue = new List<JenkinsJobType>();

            dataRequestFactory.Setup(x => x.GetJenkinsJobTypes()).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetJenkinsJobTypes();

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        #endregion

        #region AddApplicationJenkinsJob

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void AddApplicationJenkinsJob_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var jenkinsJob = new JenkinsJob();

            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.AddApplicationJenkinsJob(jenkinsJob)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.AddApplicationJenkinsJob(jenkinsJob);

            //Assert
            result.Should().Be(returnValue);
        }

        #endregion

        #region GetApplicationJenkinsJobById

        [Test]
        public void GetApplicationJenkinsJobById_NoCondition_ReturnJenkinsJob()
        {
            //Arrange
            IDataRequest dataRequest = null;
            var jenkinsJobId = 1;
            JenkinsJob defaultReturnValue = null;
            var expectedValue = new JenkinsJob();

            dataRequestFactory.Setup(x => x.GetApplicationJenkinsJobById(jenkinsJobId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationJenkinsJobById(jenkinsJobId);

            //Assert
            result.Should().Be(expectedValue);
        }

        #endregion

        #region UpdateApplicationJenkinsJob

        [Test]
        public void UpdateApplicationJenkinsJob_NoCondition_ReturnJenkinsJob()
        {
            //Arrange
            IDataRequest dataRequest = null;
            var jenkinsJob = new JenkinsJob();
            bool defaultReturnValue = false;
            var expectedValue = true;

            dataRequestFactory.Setup(x => x.UpdateApplicationJenkinsJob(jenkinsJob)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.UpdateApplicationJenkinsJob(jenkinsJob);

            //Assert
            result.Should().BeTrue();
        }

        #endregion

        #region DeleteApplicationJenkinsJob

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void DeleteApplicationJenkinsJob_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var id = 8;
            var defaultReturnValue = false;
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.DeleteApplicationJenkinsJob(id)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.DeleteApplicationJenkinsJob(id);

            //Assert
            result.Should().Be(returnValue);
        }

        #endregion

        #region GetApplicationJenkinsJobUpdateInfo

        [Test]
        public void GetApplicationJenkinsJobUpdateInfo_NoCondition_ReturnRecordUpdateInfo()
        {
            //Arrange
            var id = 8;
            RecordUpdateInfo defaultReturnValue = null;
            var expectedValue = new RecordUpdateInfo();
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetApplicationJenkinsJobUpdateInfo(id)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationJenkinsJobUpdateInfo(id);

            //Assert
            result.Should().Be(expectedValue);
        }

        #endregion
    }
}