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
    public class ApplicationSonarqubeProjectRepositoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IDataClient> dataClient;

        StrictMock<IApplicationDataRequestFactory> dataRequestFactory;

        StrictMock<ISettings> settings;

        ApplicationSonarqubeProjectRepository repository;

        [SetUp]
        public void Initialize()
        {
            dataClient = new StrictMock<IDataClient>();
            dataRequestFactory = new StrictMock<IApplicationDataRequestFactory>();
            settings = new StrictMock<ISettings>();
            SetupDataClient();

            repository = new ApplicationSonarqubeProjectRepository(
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

        #region GetSonarqubeProjects

        [Test]
        public void GetSonarqubeProjects_NoCondition_ReturnSonarqubeProjects()
        {
            //Arrange
            var applicationId = 6;
            var defaultReturnValue = new List<SonarqubeProject>();
            var expectedValue = new List<SonarqubeProject>();
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetSonarqubeProjects(applicationId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetSonarqubeProjects(applicationId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        #endregion

        #region GetApplicationSonarQubeProjectById

        [Test]
        public void GetApplicationSonarQubeProjectById_NoCondition_ReturnSonarQubeProject()
        {
            //Arrange
            IDataRequest dataRequest = null;
            var projectId = 1;
            SonarqubeProject defaultReturnValue = null;
            var expectedValue = new SonarqubeProject();

            dataRequestFactory.Setup(x => x.GetApplicationSonarQubeProjectById(projectId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationSonarQubeProjectById(projectId);

            //Assert
            result.Should().Be(expectedValue);
        }

        #endregion

        #region GetSonarQubeProjectTypes

        [Test]
        public void GetSonarQubeProjectTypes_NoCondition_ReturnSonarQubeProjectTypest()
        {
            //Arrange
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<SonarQubeProjectType>();
            var expectedValue = new List<SonarQubeProjectType>();

            dataRequestFactory.Setup(x => x.GetSonarQubeProjectTypes()).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetSonarQubeProjectTypes();

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        #endregion

        #region AddApplicationSonarQubeProject

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void AddApplicationSonarQubeProject_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var project = new SonarqubeProject();

            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.AddApplicationSonarQubeProject(project)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.AddApplicationSonarQubeProject(project);

            //Assert
            result.Should().Be(returnValue);
        }

        #endregion

        #region DeleteApplicationSonarQubeProject

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void DeleteApplicationSonarQubeProject_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            int projectId = 1;

            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.DeleteApplicationSonarQubeProject(projectId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.DeleteApplicationSonarQubeProject(projectId);

            //Assert
            result.Should().Be(returnValue);
        }

        #endregion

        #region UpdateApplicationSonarQubeProject

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void UpdateApplicationSonarQubeProject_NoCondition_ReturnExpectedResult(bool expectedResult)
        {
            //Arrange

            var project = new SonarqubeProject();
            var defaultReturnValue = false;
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.UpdateApplicationSonarQubeProject(project)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(expectedResult);

            //Act

            var result = repository.UpdateApplicationSonarQubeProject(project);

            //Assert
            result.Should().Be(expectedResult);
        }

        #endregion

        #region GetApplicationSonarQubeProjectUpdateInfo

        [Test]
        public void GetApplicationSonarQubeProjectUpdateInfo_NoCondition_ReturnRecordUpdateInfo()
        {
            //Arrange

            int projectId = 85;
            RecordUpdateInfo defaultReturnValue = null;
            var expectedValue = new RecordUpdateInfo();
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetApplicationSonarQubeProjectUpdateInfo(projectId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act

            var result = repository.GetApplicationSonarQubeProjectUpdateInfo(projectId);

            //Assert
            result.Should().Be(expectedValue);
        }

        #endregion
    }
}