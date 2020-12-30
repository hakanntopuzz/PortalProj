using AB.Data.DapperClient.Abstract;
using AB.Data.DapperClient.Model.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Factories;
using DevPortal.Data.Repositories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.UnitTests.DataTests.Repositories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationRepositoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IDataClient> dataClient;

        StrictMock<IApplicationDataRequestFactory> dataRequestFactory;

        StrictMock<ISettings> settings;

        ApplicationRepository repository;

        [SetUp]
        public void Initialize()
        {
            dataClient = new StrictMock<IDataClient>();
            dataRequestFactory = new StrictMock<IApplicationDataRequestFactory>();
            settings = new StrictMock<ISettings>();
            SetupDataClient();
            repository = new ApplicationRepository(dataClient.Object, dataRequestFactory.Object, settings.Object);
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
        public void GetApplicationGroups_NoCondition_ReturnApplicationGroups()
        {
            //Arrange
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<ApplicationGroup>();
            var expectedValue = new List<ApplicationGroup>();

            dataRequestFactory.Setup(x => x.GetApplicationGroups()).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationGroups();

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetApplicationsWithLogByApplicationGroup_NoCondition_ReturnApplicationGroups()
        {
            //Arrange
            var applicationGroupId = 1;
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<Application>();
            var expectedValue = new List<Application>();

            dataRequestFactory.Setup(x => x.GetApplicationsWithLogByApplicationGroup(applicationGroupId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationsWithLogByApplicationGroup(applicationGroupId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetApplicationTypes_NoCondition_ReturnGetApplicationTypes()
        {
            //Arrange
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<ApplicationType>();
            var expectedValue = new List<ApplicationType>();

            dataRequestFactory.Setup(x => x.GetApplicationTypes()).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationTypes();

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetApplicationsByApplicationGroupId_NoCondition_ReturnApplications()
        {
            //Arrange
            var applicationGroupId = 1;
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<Application>();
            var expectedValue = new List<Application>();

            dataRequestFactory.Setup(x => x.GetApplicationsByApplicationGroupId(applicationGroupId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationsByApplicationGroupId(applicationGroupId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void AddApplication_AddingFails_ReturnZero()
        {
            //Arrange
            var application = new Application();
            var defaultReturnValue = 0;
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.AddApplication(application)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(0);

            //Act
            var result = repository.AddApplication(application);

            //Assert
            result.Should().Be(0);
        }

        [Test]
        public void AddApplication_AddingSuccess_ReturnApplicationId()
        {
            //Arrange
            var application = new Application
            {
                Id = 1054
            };
            var defaultReturnValue = 0;
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.AddApplication(application)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(application.Id);

            //Act
            var result = repository.AddApplication(application);

            //Assert
            result.Should().Be(application.Id);
        }

        [Test]
        public void GetApplications_NoCondition_ReturnApplications()
        {
            //Arrange
            var defaultReturnValue = new List<ApplicationListItem>();
            var expectedValue = new List<ApplicationListItem>();
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetApplications()).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplications();

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetApplicationsByGroupIdAndName_NoCondition_ReturnApplications()
        {
            //Arrange
            var applicationGroupId = 13;
            var applicationName = "applicationName";
            var defaultReturnValue = new List<ApplicationListItem>();
            var expectedValue = new List<ApplicationListItem>();
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetApplicationsByGroupIdAndName(applicationGroupId, applicationName)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationsByGroupIdAndName(applicationGroupId, applicationName);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetApplicationsByGroupId_NoCondition_ReturnApplications()
        {
            //Arrange
            var applicationGroupId = 13;
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<ApplicationListItem>();
            var expectedValue = new List<ApplicationListItem>();

            dataRequestFactory.Setup(x => x.GetApplicationsByGroupId(applicationGroupId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationsByGroupId(applicationGroupId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetApplicationsByApplicationName_NoCondition_ReturnApplications()
        {
            //Arrange
            var applicationName = "applicationName";
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<ApplicationListItem>();
            var expectedValue = new List<ApplicationListItem>();

            dataRequestFactory.Setup(x => x.GetApplicationsByApplicationName(applicationName)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationsByApplicationName(applicationName);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetApplication_NoCondition_ReturnApplication()
        {
            //Arrange
            var applicationId = 13;
            IDataRequest dataRequest = null;
            var defaultReturnValue = new Application();
            var expectedValue = new Application();

            dataRequestFactory.Setup(x => x.GetApplication(applicationId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, SetupAny<Application>())).Returns(expectedValue);

            //Act
            var result = repository.GetApplication(applicationId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetSvnRepositories_NoCondition_ReturnSvnRepositoryList()
        {
            //Arrange
            var applicationId = 1;
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<SvnRepository>();
            var expectedValue = new List<SvnRepository>();

            dataRequestFactory.Setup(x => x.GetSvnRepositories(applicationId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetSvnRepositories(applicationId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetJenkinsJobs_NoCondition_ReturnJenkinsJobList()
        {
            //Arrange
            var applicationGroupId = 1;
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<JenkinsJob>();
            var expectedValue = new List<JenkinsJob>();

            dataRequestFactory.Setup(x => x.GetJenkinsJobs(applicationGroupId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetJenkinsJobs(applicationGroupId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        //[Test]
        //public void GetEnvironments_NoCondition_ReturnJenkinsJobList()
        //{
        //    //Arrange
        //    IDataRequest dataRequest = null;
        //    var defaultReturnValue = new List<Environment>();
        //    var expectedValue = new List<Environment>();

        //    dataRequestFactory.Setup(x => x.GetEnvironments()).Returns(dataRequest);
        //    dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

        //    //Act
        //    var result = repository.GetEnvironments();

        //    //Assert
        //    result.Should().BeSameAs(expectedValue);
        //}

        [Test]
        public void EditApplication_NoCondition_ReturnJenkinsJobList()
        {
            //Arrange
            var application = new Application();
            IDataRequest dataRequest = null;
            var defaultReturnValue = false;
            var expectedValue = true;

            dataRequestFactory.Setup(x => x.UpdateApplication(application)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.UpdateApplication(application);

            //Assert
            result.Should().Be(expectedValue);
        }

        [Test]
        public void GetApplicationStatusList_NoCondition_ReturnApplicationStatusList()
        {
            //Arrange
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<ApplicationStatus>();
            var expectedValue = new List<ApplicationStatus>();

            dataRequestFactory.Setup(x => x.GetApplicationStatusList()).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationStatusList();

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void DeleteApplication_NoCondition_ReturnFalse()
        {
            //Arrange
            var applicationId = 2;
            var defaultReturnValue = false;
            var expectedValue = false;
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.DeleteApplication(applicationId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.DeleteApplication(applicationId);

            //Assert
            result.Should().BeFalse();
        }

        [Test]
        public void DeleteApplication_NoCondition_ReturnTrue()
        {
            //Arrange
            var applicationId = 2;
            var defaultReturnValue = false;
            var expectedValue = true;
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.DeleteApplication(applicationId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.DeleteApplication(applicationId);

            //Assert
            result.Should().BeTrue();
        }

        [Test]
        public void GetApplicationByApplicationName_NoCondition_ReturnFalse()
        {
            //Arrange
            var name = "name";
            Application defaultReturnValue = null;
            var expectedValue = new Application();
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetApplicationByApplicationName(name)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationByApplicationName(name);

            //Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        [Test]
        public void GetApplicationByApplicationName_NoCondition_ReturnTrue()
        {
            //Arrange
            var name = "name";
            Application defaultReturnValue = null;
            var expectedValue = new Application();
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetApplicationByApplicationName(name)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationByApplicationName(name);

            //Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        [Test]
        public void GetApplicationCount_NoCondition_ReturnCount()
        {
            //Arrange
            var defaultReturnValue = 0;
            var expectedValue = 56;
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetApplicationCount()).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationCount();

            //Assert
            result.Should().Be(expectedValue);
        }

        [Test]
        public void GetApplicationGroupCount_NoCondition_ReturnCount()
        {
            //Arrange
            var defaultReturnValue = 0;
            var expectedValue = 8;
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetApplicationGroupCount()).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationGroupCount();

            //Assert
            result.Should().Be(expectedValue);
        }

        [Test]
        public void GetJenkinsJobCount_NoCondition_ReturnCount()
        {
            //Arrange
            var defaultReturnValue = 0;
            var expectedValue = 120;
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetJenkinsJobCount()).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetJenkinsJobCount();

            //Assert
            result.Should().Be(expectedValue);
        }

        [Test]
        public void GetSvnRepositoryCount_NoCondition_ReturnCount()
        {
            //Arrange
            var defaultReturnValue = 0;
            var expectedValue = 86;
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetSvnRepositoryCount()).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetSvnRepositoryCount();

            //Assert
            result.Should().Be(expectedValue);
        }

        [Test]
        public void GetSonarQubeProjectCount_NoCondition_ReturnCount()
        {
            //Arrange
            var defaultReturnValue = 0;
            var expectedValue = 75;
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetSonarQubeProjectCount()).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetSonarQubeProjectCount();

            //Assert
            result.Should().Be(expectedValue);
        }

        [Test]
        public void GetSonarQubeProjectCountByType_NoCondition_ReturnCounts()
        {
            //Arrange
            var defaultReturnValue = new List<SonarQubeProjectCountByTypeModel>();
            var expectedValue = new List<SonarQubeProjectCountByTypeModel>();
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetSonarQubeProjectCountByType()).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetSonarQubeProjectCountByType();

            //Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        [Test]
        public void GetApplicationRedmineProjectCount_NoCondition_ReturnCount()
        {
            //Arrange
            var defaultReturnValue = 0;
            var expectedValue = 75;
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetApplicationRedmineProjectCount()).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationRedmineProjectCount();

            //Assert
            result.Should().Be(expectedValue);
        }

        [Test]
        public void GetDatabaseRedmineProjectCount_NoCondition_ReturnCount()
        {
            //Arrange
            var defaultReturnValue = 0;
            var expectedValue = 75;
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetDatabaseRedmineProjectCount()).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetDatabaseRedmineProjectCount();

            //Assert
            result.Should().Be(expectedValue);
        }

        [Test]
        public void GetNugetPackageCount_NoCondition_ReturnCount()
        {
            //Arrange
            var defaultReturnValue = 0;
            var expectedValue = 75;
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetNugetPackageCount()).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetNugetPackageCount();

            //Assert
            result.Should().Be(expectedValue);
        }

        [Test]
        public void GetApplicationCountByType_NoCondition_ReturnCounts()
        {
            //Arrange
            var defaultReturnValue = new List<ApplicationCountByTypeModel>();
            var expectedValue = new List<ApplicationCountByTypeModel>();
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetApplicationCountByType()).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationCountByType();

            //Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        [Test]
        public void GetJenkinsJobCountByType_NoCondition_ReturnCounts()
        {
            //Arrange
            var defaultReturnValue = new List<JenkinsJobCountByTypeModel>();
            var expectedValue = new List<JenkinsJobCountByTypeModel>();
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetJenkinsJobCountByType()).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetJenkinsJobCountByType();

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetLastUpdatedApplications_NoCondition_ReturnApplications()
        {
            //Arrange
            var defaultReturnValue = new List<ApplicationListItem>();
            var expectedValue = new List<ApplicationListItem>();
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetLastUpdatedApplications()).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetLastUpdatedApplications();

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public async Task GetFilteredApplicationListAsync_NoCondition_ReturnApplications()
        {
            //Arrange
            int skip = 1;
            int take = 1;
            string orderBy = "orderBy";
            string orderDir = "orderDir";
            string searchText = "searchText";
            int applicationGroupId = 1;

            var defaultReturnValue = new List<Application>();
            var expectedValue = new List<Application>();
            Mock<IDataRequest> dataRequest = new Mock<IDataRequest>();

            dataRequestFactory.Setup(x => x.GetFilteredApplicationList(skip, take, orderBy, orderDir, searchText, applicationGroupId)).Returns(dataRequest.Object);
            dataClient.Setup(x => x.GetCollectionAsync<Application, RecordUpdateInfo, Application>(
                dataRequest.Object,
                DataClientMapFactory.ApplicationsMap,
                defaultReturnValue,
                dataRequest.Object.SplitOnParameters)).ReturnsAsync(expectedValue);

            //Act
            var result = await repository.GetFilteredApplicationListAsync(skip, take, orderBy, orderDir, searchText, applicationGroupId);

            //Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        [Test]
        public void GetApplicationByJenkinsJobName_NoCondition_ReturnApplication()
        {
            //Arrange
            var jenkinsJobName = "job-name";
            IDataRequest dataRequest = null;
            var defaultReturnValue = new Application();
            var expectedValue = new Application();

            dataRequestFactory.Setup(x => x.GetApplicationByJenkinsJobName(jenkinsJobName)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, SetupAny<Application>())).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationByJenkinsJobName(jenkinsJobName);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetApplicationUpdateInfo_NoCondition_ReturnRecordUpdateInfo()
        {
            //Arrange
            var applicationId = 3;
            IDataRequest dataRequest = null;
            var defaultReturnValue = new RecordUpdateInfo();
            var expectedValue = new RecordUpdateInfo();

            dataRequestFactory.Setup(x => x.GetApplicationUpdateInfo(applicationId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, SetupAny<RecordUpdateInfo>())).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationUpdateInfo(applicationId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetExternalDependenciesByApplicationId_NoCondition_ReturnExternalDependencies()
        {
            //Arrange
            var applicationId = 1;
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<ExternalDependency>();
            var expectedValue = new List<ExternalDependency>();

            dataRequestFactory.Setup(x => x.GetExternalDependenciesByApplicationId(applicationId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetExternalDependenciesByApplicationId(applicationId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetNugetPackageDependenciesByApplicationId_NoCondition_ReturnApplicationDependencies()
        {
            //Arrange
            var applicationId = 1;
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<NugetPackageDependency>();
            var expectedValue = new List<NugetPackageDependency>();

            dataRequestFactory.Setup(x => x.GetNugetPackageDependenciesByApplicationId(applicationId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetNugetPackageDependenciesByApplicationId(applicationId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetApplicationDependenciesByApplicationId_NoCondition_ReturnApplicationDependencies()
        {
            //Arrange
            var applicationId = 1;
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<ApplicationDependency>();
            var expectedValue = new List<ApplicationDependency>();

            dataRequestFactory.Setup(x => x.GetApplicationDependenciesByApplicationId(applicationId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationDependenciesByApplicationId(applicationId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetApplicationDependencies_NoCondition_ReturnApplicationExportListItems()
        {
            //Arrange
            var applicationId = 1;
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<ApplicationDependenciesExportListItem>();
            var expectedValue = new List<ApplicationDependenciesExportListItem>();

            dataRequestFactory.Setup(x => x.GetApplicationDependencies(applicationId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationDependencies(applicationId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetFullExternalDependenciesByApplicationId_NoCondition_ReturnExternalExportListItems()
        {
            //Arrange
            var applicationId = 1;
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<ExternalDependenciesExportListItem>();
            var expectedValue = new List<ExternalDependenciesExportListItem>();

            dataRequestFactory.Setup(x => x.GetFullExternalDependenciesByApplicationId(applicationId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetFullExternalDependenciesByApplicationId(applicationId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetApplicationTypeByApplicationId_NoCondition_ReturnApplicationType()
        {
            //Arrange
            var applicationId = 7;
            IDataRequest dataRequest = null;
            var defaultReturnValue = new ApplicationType();
            var expectedValue = new ApplicationType();

            dataRequestFactory.Setup(x => x.GetApplicationTypeByApplicationId(applicationId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, SetupAny<ApplicationType>())).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationTypeByApplicationId(applicationId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public async Task GetFilteredApplicationRedmineProjectListAsync_NoCondition_ReturnRedmineProjects()
        {
            //Arrange
            int skip = 1;
            int take = 1;
            string orderBy = "orderBy";
            string orderDir = "orderDir";
            string searchText = "searchText";
            int applicationGroupId = 45;

            var defaultReturnValue = new List<RedmineProject>();
            var expectedValue = new List<RedmineProject>();
            var dataRequest = new Mock<IDataRequest>();

            dataRequestFactory.Setup(x => x.GetFilteredApplicationRedmineProjectList(skip, take, orderBy, orderDir, searchText, applicationGroupId)).Returns(dataRequest.Object);
            dataClient.Setup(x => x.GetCollectionAsync(dataRequest.Object, SetupAny<List<RedmineProject>>())).ReturnsAsync(expectedValue);

            //Act
            var result = await repository.GetFilteredApplicationRedmineProjectListAsync(skip, take, orderBy, orderDir, searchText, applicationGroupId);

            //Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        [Test]
        public void GetDatabaseDependencies_NoCondition_ReturnDatabaseDependenciesExportListItems()
        {
            //Arrange
            var applicationId = 1;
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<DatabaseDependenciesExportListItem>();
            var expectedValue = new List<DatabaseDependenciesExportListItem>();

            dataRequestFactory.Setup(x => x.GetDatabaseDependencies(applicationId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetDatabaseDependencies(applicationId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetNugetPackageDependencies_NoCondition_ReturnNugetPackageDependenciesExportListItems()
        {
            //Arrange
            var applicationId = 1;
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<NugetPackageDependenciesExportListItem>();
            var expectedValue = new List<NugetPackageDependenciesExportListItem>();

            dataRequestFactory.Setup(x => x.GetNugetPackageDependencies(applicationId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetNugetPackageDependencies(applicationId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }
    }
}