using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Business.Services;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Model;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationReaderServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IApplicationRepository> applicationRepository;

        StrictMock<IGeneralSettingsService> generalSettingsService;

        ApplicationReaderService service;

        [SetUp]
        public void Initialize()
        {
            applicationRepository = new StrictMock<IApplicationRepository>();
            generalSettingsService = new StrictMock<IGeneralSettingsService>();

            service = new ApplicationReaderService(
                applicationRepository.Object,
                generalSettingsService.Object
                );
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            applicationRepository.VerifyAll();
            generalSettingsService.VerifyAll();
        }

        #endregion

        #region get filtered application redmine projects

        [Test]
        public void GetFilteredApplicationRedmineProjectListAsync_TableParamNull_ReturnEmptyList()
        {
            // Arrange
            RedmineTableParam tableParam = null;

            // Act
            var result = service.GetFilteredApplicationRedmineProjectListAsync(tableParam);

            // Assert
            result.Result.Should().NotBeNull().And.HaveCount(0);
        }

        [Test]
        public void GetFilteredApplicationRedmineProjectListAsync_ValidTableParam_SetProjectUrlsAndReturnProjects()
        {
            // Arrange
            var tableParam = new RedmineTableParam
            {
                start = 1,
                length = 10,
                order = new List<TableOrder>
                {
                    new TableOrder { column = 1, dir = "" }
                },
                columns = new List<TableColumn>
                {
                    new TableColumn { data = "", name = "" }
                },
                SearchText = "text",
                ApplicationGroupId = 45
            };

            var projects = new List<RedmineProject> {
                new RedmineProject{
                    ProjectName = "project",
                    TotalCount = 14
                }
            };
            var projectUrl = new Uri("http://wwww.example.com/project-url");

            var expectedProjects = new List<RedmineProject> {
                new RedmineProject{
                    ProjectName = "project",
                    ProjectUrl = projectUrl.ToString(),
                    RepositoryUrl = $"{projectUrl}/repository"
                }
            };

            applicationRepository.Setup(x => x.GetFilteredApplicationRedmineProjectListAsync(tableParam.start, tableParam.length, tableParam.SortColumn, tableParam.order[0].dir, tableParam.SearchText, tableParam.ApplicationGroupId)).ReturnsAsync(projects);
            generalSettingsService.Setup(x => x.GetRedmineProjectUrl(projects[0].ProjectName)).Returns(projectUrl);

            // Act
            var result = service.GetFilteredApplicationRedmineProjectListAsync(tableParam);

            // Assert
            result.Result.Should().BeEquivalentTo(projects);
        }

        #endregion

        #region GetApplicationGroups

        [Test]
        public void GetApplicationGroups_NoCondition_ReturnApplicationGroup()
        {
            // Arrange
            ICollection<ApplicationGroup> applicationGroups = new List<ApplicationGroup>();

            // Act
            applicationRepository.Setup(x => x.GetApplicationGroups()).Returns(applicationGroups);

            var result = service.GetApplicationGroups();

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void GetApplicationGroups_NoCondition_ReturnApplicationGroupIsNull()
        {
            // Arrange
            ICollection<ApplicationGroup> applicationGroups = null;

            // Act
            applicationRepository.Setup(x => x.GetApplicationGroups()).Returns(applicationGroups);

            var result = service.GetApplicationGroups();

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region GetApplicationsWithLogByApplicationGroup

        [Test]
        public void GetApplicationsWithLogByApplicationGroup_NoCondition_ReturnApplicationGroup()
        {
            // Arrange
            var applicationGroupId = 1;
            ICollection<Application> applications = new List<Application>();

            // Act
            applicationRepository.Setup(x => x.GetApplicationsWithLogByApplicationGroup(applicationGroupId)).Returns(applications);

            var result = service.GetApplicationsWithLogByApplicationGroup(applicationGroupId);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void GetApplicationsWithLogByApplicationGroup_NoCondition_ReturnApplicationGroupIsNull()
        {
            // Arrange
            var applicationGroupId = 1;
            ICollection<Application> applications = null;

            // Act
            applicationRepository.Setup(x => x.GetApplicationsWithLogByApplicationGroup(applicationGroupId)).Returns(applications);

            var result = service.GetApplicationsWithLogByApplicationGroup(applicationGroupId);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region GetApplicationsByApplicationGroupId

        [Test]
        public void GetApplicationsByApplicationGroupId_NoCondition_ReturnApplications()
        {
            // Arrange
            int applicationGroupId = 1;
            ICollection<Application> applications = new List<Application>();

            // Act
            applicationRepository.Setup(x => x.GetApplicationsByApplicationGroupId(applicationGroupId)).Returns(applications);

            var result = service.GetApplicationsByApplicationGroupId(applicationGroupId);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void GetApplicationsByApplicationGroupId_NoCondition_ReturnApplicationsIsNull()
        {
            // Arrange
            int applicationGroupId = 1;
            ICollection<Application> applications = null;

            // Act
            applicationRepository.Setup(x => x.GetApplicationsByApplicationGroupId(applicationGroupId)).Returns(applications);

            var result = service.GetApplicationsByApplicationGroupId(applicationGroupId);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region GetApplications

        [Test]
        public void GetApplications_NoCondition_ReturnApplication()
        {
            // Arrange
            ICollection<ApplicationListItem> applications = new List<ApplicationListItem>();

            // Act
            applicationRepository.Setup(x => x.GetApplications()).Returns(applications);

            var result = service.GetApplications();

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void GetApplications_NoCondition_ReturnApplicationIsNull()
        {
            // Arrange
            ICollection<ApplicationListItem> applications = null;

            // Act
            applicationRepository.Setup(x => x.GetApplications()).Returns(applications);

            var result = service.GetApplications();

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region GetApplicationsByGroupId

        [Test]
        public void GetApplicationsByGroupId_NoCondition_ReturnApplication()
        {
            // Arrange
            ICollection<ApplicationListItem> applications = new List<ApplicationListItem>();
            var applicationGroupId = 6;

            // Act
            applicationRepository.Setup(x => x.GetApplicationsByGroupId(applicationGroupId)).Returns(applications);

            var result = service.GetApplicationsByGroupId(applicationGroupId);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void GetApplicationsByGroupId_NoCondition_ReturnApplicationIsNull()
        {
            // Arrange
            ICollection<ApplicationListItem> applications = null;
            var applicationGroupId = 6;

            // Act
            applicationRepository.Setup(x => x.GetApplicationsByGroupId(applicationGroupId)).Returns(applications);

            var result = service.GetApplicationsByGroupId(applicationGroupId);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region GetApplicationsByApplicationName

        [Test]
        public void GetApplicationsByApplicationName_NoCondition_ReturnApplication()
        {
            // Arrange
            ICollection<ApplicationListItem> applications = new List<ApplicationListItem>();
            var applicationName = "applicationName";

            // Act
            applicationRepository.Setup(x => x.GetApplicationsByApplicationName(applicationName)).Returns(applications);

            var result = service.GetApplicationsByApplicationName(applicationName);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void GetApplicationsByApplicationName_NoCondition_ReturnApplicationIsNull()
        {
            // Arrange
            ICollection<ApplicationListItem> applications = null;
            var applicationName = "applicationName";

            // Act
            applicationRepository.Setup(x => x.GetApplicationsByApplicationName(applicationName)).Returns(applications);

            var result = service.GetApplicationsByApplicationName(applicationName);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region GetApplicationsByGroupIdAndName

        [Test]
        public void GetApplicationsByGroupIdAndName_NoCondition_ReturnApplication()
        {
            // Arrange
            var applicationGroupId = 1;
            var applicationName = "applicationName";
            ICollection<ApplicationListItem> applications = new List<ApplicationListItem>();

            // Act
            applicationRepository.Setup(x => x.GetApplicationsByGroupIdAndName(applicationGroupId, applicationName)).Returns(applications);

            var result = service.GetApplicationsByGroupIdAndName(applicationGroupId, applicationName);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void GetApplicationsByGroupIdAndName_NoCondition_ReturnApplicationIsNull()
        {
            // Arrange
            var applicationGroupId = 1;
            var applicationName = "applicationName";
            ICollection<ApplicationListItem> applications = null;

            // Act
            applicationRepository.Setup(x => x.GetApplicationsByGroupIdAndName(applicationGroupId, applicationName)).Returns(applications);

            var result = service.GetApplicationsByGroupIdAndName(applicationGroupId, applicationName);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region GetApplication

        [Test]
        public void GetApplication_ApplicationExists_ReturnApplication()
        {
            // Arrange
            var applicationId = 1;
            var redmineProjectName = "redmine-project-name";
            var redmineProjectUrl = "http://wwww.example.com/redmine-project-url";
            var redmineProjectUri = new Uri(redmineProjectUrl);
            var application = new Application
            {
                RedmineProjectName = redmineProjectName
            };
            var updateInfo = new RecordUpdateInfo();

            generalSettingsService.Setup(x => x.GetRedmineProjectUrl(redmineProjectName)).Returns(redmineProjectUri);
            applicationRepository.Setup(x => x.GetApplication(applicationId)).Returns(application);
            applicationRepository.Setup(x => x.GetApplicationUpdateInfo(applicationId)).Returns(updateInfo);

            // Act
            var result = service.GetApplication(applicationId);

            // Assert
            result.Should().Be(application);
            application.RedmineProjectUrl.Should().Be(redmineProjectUrl);
        }

        [Test]
        public void GetApplication_ApplicationDoesNotExist_ReturnNull()
        {
            // Arrange
            var applicationId = 1;
            Application application = null;

            applicationRepository.Setup(x => x.GetApplication(applicationId)).Returns(application);

            // Act

            var result = service.GetApplication(applicationId);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region Get Svn Repositories

        [Test]
        public void GetSvnRepositories_NoCondition_ReturnGetSvnRepositoryList()
        {
            // Arrange
            var applicationId = 1;
            var SvnRepositoryList = new List<SvnRepository>();
            var svnUrl = new Uri("http://wwww.example.com/svn-url");

            generalSettingsService.Setup(x => x.GetSvnUrl()).Returns(svnUrl);
            applicationRepository.Setup(x => x.GetSvnRepositories(applicationId)).Returns(SvnRepositoryList);

            // Act
            var result = service.GetSvnRepositories(applicationId);

            // Assert
            result.Should().BeSameAs(SvnRepositoryList);
        }

        #endregion

        #region Get Jenkins Jobs

        [Test]
        public void GetJenkinsJobs_NoCondition_ReturnJenkinsJobList()
        {
            // Arrange
            var applicationId = 1;
            var jenkinsJob = new JenkinsJob { JenkinsJobName = "name" };
            var jenkinsJobList = new List<JenkinsJob> { jenkinsJob };
            var jenkinsJobUrl = new Uri("http://wwww.example.com/jenkins-job-url");

            generalSettingsService.Setup(x => x.GetJenkinsJobUrl(jenkinsJob.JenkinsJobName)).Returns(jenkinsJobUrl);
            applicationRepository.Setup(x => x.GetJenkinsJobs(applicationId)).Returns(jenkinsJobList);

            // Act
            var result = service.GetJenkinsJobs(applicationId);

            // Assert
            result.Should().BeSameAs(jenkinsJobList);
        }

        #endregion

        #region filter applications

        [Test]
        public void FilterApplications_ApplicationGroupIdIsZeroAndApplicationNameIsEmpty_ReturnApplication()
        {
            // Arrange
            var applicationGroupId = 0;
            var applicationName = "";
            ICollection<ApplicationListItem> applications = new List<ApplicationListItem>();

            // Act
            applicationRepository.Setup(x => x.GetApplications()).Returns(applications);

            var result = service.FilterApplications(applicationGroupId, applicationName);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void FilterApplications_ApplicationGroupIdIsZeroAndApplicationNameIsEmpty_ReturnApplicationIsNull()
        {
            // Arrange
            var applicationGroupId = 0;
            var applicationName = "";
            ICollection<ApplicationListItem> applications = null;

            // Act
            applicationRepository.Setup(x => x.GetApplications()).Returns(applications);

            var result = service.FilterApplications(applicationGroupId, applicationName);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void FilterApplications_ApplicationGroupIdIsNotZeroAndApplicationNameIsEmpty_ReturnApplication()
        {
            // Arrange
            var applicationGroupId = 5;
            var applicationName = "";
            ICollection<ApplicationListItem> applications = new List<ApplicationListItem>();

            // Act
            applicationRepository.Setup(x => x.GetApplicationsByGroupId(applicationGroupId)).Returns(applications);

            var result = service.FilterApplications(applicationGroupId, applicationName);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void FilterApplications_ApplicationGroupIdIsNotZeroAndApplicationNameIsEmpty_ReturnApplicationIsNull()
        {
            // Arrange
            var applicationGroupId = 5;
            var applicationName = "";
            ICollection<ApplicationListItem> applications = null;

            // Act
            applicationRepository.Setup(x => x.GetApplicationsByGroupId(applicationGroupId)).Returns(applications);

            var result = service.FilterApplications(applicationGroupId, applicationName);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void FilterApplications_ApplicationGroupIdIsZeroAndApplicationNameIsNotEmpty_ReturnApplication()
        {
            // Arrange
            var applicationGroupId = 0;
            var applicationName = "applicationName";
            ICollection<ApplicationListItem> applications = new List<ApplicationListItem>();

            // Act
            applicationRepository.Setup(x => x.GetApplicationsByApplicationName(applicationName)).Returns(applications);

            var result = service.FilterApplications(applicationGroupId, applicationName);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void FilterApplications_ApplicationGroupIdIsZeroAndApplicationNameIsNotEmpty_ReturnApplicationIsNull()
        {
            // Arrange
            var applicationGroupId = 0;
            var applicationName = "applicationName";
            ICollection<ApplicationListItem> applications = null;

            // Act
            applicationRepository.Setup(x => x.GetApplicationsByApplicationName(applicationName)).Returns(applications);

            var result = service.FilterApplications(applicationGroupId, applicationName);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void FilterApplications_ApplicationGroupIdIsNotZeroAndApplicationNameIsNotEmpty_ReturnApplication()
        {
            // Arrange
            var applicationGroupId = 5;
            var applicationName = "applicationName";
            ICollection<ApplicationListItem> applications = new List<ApplicationListItem>();

            // Act
            applicationRepository.Setup(x => x.GetApplicationsByGroupIdAndName(applicationGroupId, applicationName)).Returns(applications);

            var result = service.FilterApplications(applicationGroupId, applicationName);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void FilterApplications_ApplicationGroupIdIsNotZeroAndApplicationNameIsNotEmpty_ReturnApplicationIsNull()
        {
            // Arrange
            var applicationGroupId = 5;
            var applicationName = "applicationName";
            ICollection<ApplicationListItem> applications = null;

            // Act
            applicationRepository.Setup(x => x.GetApplicationsByGroupIdAndName(applicationGroupId, applicationName)).Returns(applications);

            var result = service.FilterApplications(applicationGroupId, applicationName);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region GetApplicationTypeByApplicationId

        [Test]
        public void GetApplicationTypeByApplicationId_NoCondition_ReturnApplicationType()
        {
            // Arrange
            var applicationId = 7;
            var applicationType = new ApplicationType();

            applicationRepository.Setup(x => x.GetApplicationTypeByApplicationId(applicationId)).Returns(applicationType);

            // Act
            var result = service.GetApplicationTypeByApplicationId(applicationId);

            // Assert
            result.Should().Be(applicationType);
        }

        #endregion

        #region GetApplicationTypes

        [Test]
        public void GetApplicationTypes_NoCondition_ReturnApplicationTypes()
        {
            // Arrange
            ICollection<ApplicationType> applicationTypes = new List<ApplicationType>();

            // Act
            applicationRepository.Setup(x => x.GetApplicationTypes()).Returns(applicationTypes);

            var result = service.GetApplicationTypes();

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void GetApplicationTypes_NoCondition_ReturnApplicationTypesIsNull()
        {
            // Arrange
            ICollection<ApplicationType> applicationTypes = null;

            // Act
            applicationRepository.Setup(x => x.GetApplicationTypes()).Returns(applicationTypes);

            var result = service.GetApplicationTypes();

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region GetApplicationStatusList

        [Test]
        public void GetApplicationStatusList_NoCondition_ReturnApplicationStatusCollection()
        {
            // Arrange

            applicationRepository.Setup(x => x.GetApplicationStatusList()).Returns(SetupAny<ICollection<ApplicationStatus>>());

            // Act
            var result = service.GetApplicationStatusList();

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region GetLastUpdatedApplications

        [Test]
        public void GetLastUpdatedApplications_NoCondition_ReturnApplications()
        {
            // Arrange
            var applications = new List<ApplicationListItem>();

            // Act
            applicationRepository.Setup(x => x.GetLastUpdatedApplications()).Returns(applications);

            var result = service.GetLastUpdatedApplications();

            // Assert
            result.Should().NotBeNull();
        }

        #endregion

        #region GetApplicationByJenkinsJobName

        [Test]
        public void GetApplicationByJenkinsName_NoCondition_ReturnApplication()
        {
            // Arrange
            var name = "mx-social";
            var application = new Application();

            applicationRepository.Setup(x => x.GetApplicationByJenkinsJobName(name)).Returns(application);

            // Act
            var result = service.GetApplicationByJenkinsJobName(name);

            // Assert
            result.Should().Be(application);
        }

        #endregion

        #region GetFilteredApplicationListAsync

        [Test]
        public void GetFilteredApplications_ParamNull_ReturnEmptyApplicationList()
        {
            // Arrange
            ApplicationTableParam tableParam = null;

            var applications = new List<Application>();

            // Act
            var result = service.GetFilteredApplicationListAsync(tableParam);

            // Assert
            result.Result.Should().BeEquivalentTo(applications);
        }

        [Test]
        public void GetFilteredApplications_ParamValid_ReturnFilteredApplicationList()
        {
            // Arrange
            var tableParam = new ApplicationTableParam
            {
                start = 1,
                length = 10,
                order = new List<TableOrder>
                {
                    new TableOrder { column = 1, dir = "" }
                },
                columns = new List<TableColumn>
                {
                    new TableColumn { data = "", name = "" }
                }
            };

            var applications = new List<Application>();

            applicationRepository.Setup(x => x.GetFilteredApplicationListAsync(tableParam.start, tableParam.length, tableParam.SortColumn, tableParam.order[0].dir, tableParam.SearchText, tableParam.ApplicationGroupId)).ReturnsAsync(applications);

            // Act
            var result = service.GetFilteredApplicationListAsync(tableParam);

            // Assert
            result.Result.Should().BeEquivalentTo(applications);
        }

        #endregion
    }
}