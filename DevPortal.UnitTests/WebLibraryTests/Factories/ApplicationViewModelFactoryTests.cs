using AB.Framework.UnitTests;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.WebLibraryTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationViewModelFactoryTests : BaseTestFixture
    {
        #region members & setup

        ApplicationViewModelFactory factory;

        StrictMock<IBreadCrumbFactory> breadCrumbFactory;

        StrictMock<IAuthorizationServiceWrapper> authorizationServiceWrapper;

        [SetUp]
        public void Initialize()
        {
            breadCrumbFactory = new StrictMock<IBreadCrumbFactory>();
            authorizationServiceWrapper = new StrictMock<IAuthorizationServiceWrapper>();

            factory = new ApplicationViewModelFactory(breadCrumbFactory.Object,
                authorizationServiceWrapper.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            breadCrumbFactory.VerifyAll();
            authorizationServiceWrapper.VerifyAll();
        }

        #endregion

        #region  application

        [Test]
        public void CreateApplication_NoCondition_ReturnViewModel()
        {
            // Arrange

            var application = new Application
            {
                Id = 45,
                Name = "app-name",
                Status = "Aktif",
                RedmineProjectUrl = "redmine/project/url",
                ApplicationGroupName = "group-name",
                ApplicationGroupId = 74,
                ApplicationType = "app-type"
            };
            var databases = new List<Database>();
            var breadcrumbViewModel = new BreadCrumbViewModel();

            authorizationServiceWrapper.Setup(s => s.CheckUserHasAdminDeveloperPolicy()).Returns(true);
            breadCrumbFactory.Setup(x => x.CreateApplicationDetailModel(application.Id)).Returns(breadcrumbViewModel);

            var viewModel = new ApplicationViewModel
            {
                Application = application,
                BreadCrumbViewModel = breadcrumbViewModel,
                IsAuthorized = true,
                FavouritePageName = $"{PageNames.ApplicationInformation} - {application.Name}"
            };

            // Act
            var result = factory.CreateApplication(application);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
        }

        [Test]
        public void CreateApplicationsViewModel_NoCondition_ReturnModel()
        {
            // Arrange
            var applicationGroups = new List<ApplicationGroup>();
            var applications = new List<ApplicationListItem>();
            var breadcrumbViewModel = new BreadCrumbViewModel();

            authorizationServiceWrapper.Setup(s => s.CheckUserHasAdminDeveloperPolicy()).Returns(true);
            breadCrumbFactory.Setup(x => x.CreateApplicationsModel()).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateApplicationsViewModel(applicationGroups, applications);

            // Assert
            result.ApplicationGroups.Should().BeSameAs(applicationGroups);
            result.Applications.Should().BeSameAs(applications);
        }

        [Test]
        public void CreateApplicationsViewModelWithFilter_NoCondition_ReturnViewModel()
        {
            // Arrange

            var applicationGroups = new List<ApplicationGroup>();
            var applications = new List<ApplicationListItem>();
            var applicationGroupId = 2;
            var applicationName = "app-name";
            var breadcrumbViewModel = new BreadCrumbViewModel();

            breadCrumbFactory.Setup(x => x.CreateApplicationsModel()).Returns(breadcrumbViewModel);

            var viewModel = new ApplicationsViewModel
            {
                ApplicationGroups = applicationGroups,
                Applications = applications,
                ApplicationGroupId = applicationGroupId,
                ApplicationName = applicationName,
                BreadCrumbViewModel = breadcrumbViewModel
            };

            // Act
            var result = factory.CreateApplicationsViewModelWithFilter(applicationGroups, applications, applicationGroupId, applicationName);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
        }

        [Test]
        public void CreateApplicationAddViewModel_NoCondition_ReturnModel()
        {
            // Arrange
            var applicationGroups = new List<ApplicationGroup>();
            var applicationTypes = new List<ApplicationType>();
            var applicationStatus = new List<ApplicationStatus>();
            var breadcrumbViewModel = new BreadCrumbViewModel();

            breadCrumbFactory.Setup(x => x.CreateApplicationAddModel()).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateApplicationAddViewModel(applicationGroups, applicationTypes, applicationStatus);

            // Assert
            result.ApplicationGroups.Should().BeSameAs(applicationGroups);
            result.ApplicationTypes.Should().BeSameAs(applicationTypes);
        }

        [Test]
        public void CreateEditApplication_NoCondition_ReturnModel()
        {
            // Arrange
            var application = new Application();
            var applicationGroups = new List<ApplicationGroup>();
            var applicationTypes = new List<ApplicationType>();
            var applicationStatusList = new List<ApplicationStatus>();
            var breadcrumbViewModel = new BreadCrumbViewModel();

            var viewModel = new EditApplicationViewModel
            {
                ApplicationGroups = applicationGroups,
                ApplicationTypes = applicationTypes,
                ApplicationStatusList = applicationStatusList,
                Application = application,
                BreadCrumbViewModel = breadcrumbViewModel
            };

            breadCrumbFactory.Setup(x => x.CreateApplicationEditModel(application.Id)).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateEditApplication(application, applicationGroups, applicationTypes, applicationStatusList);

            // Assert
            result.Should().BeEquivalentTo(viewModel);
        }

        [Test]
        public void CreateApplicationEnvironment_NoCondition_ReturnModel()
        {
            // Arrange
            var applicationEnvironment = new ApplicationEnvironment();

            var model = new ApplicationEnvironment
            {
                ApplicationId = applicationEnvironment.ApplicationId,
                EnvironmentId = applicationEnvironment.EnvironmentId,
                HasLog = applicationEnvironment.HasLog,
                LogFilePath = applicationEnvironment.LogFilePath,
                PhysicalPath = applicationEnvironment.PhysicalPath,
                Url = applicationEnvironment.Url
            };

            // Act
            var result = factory.CreateApplicationEnvironment(applicationEnvironment);

            // Assert
            result.Should().BeEquivalentTo(model);
        }

        [Test]
        public void CreateApplicationEnvironment_NoCondition_ReturnModelWithApplicationName()
        {
            // Arrange
            var applicationId = 1;
            var applicationName = "appName";

            var model = new ApplicationEnvironment
            {
                ApplicationId = applicationId,
                ApplicationName = applicationName
            };

            // Act
            var result = factory.CreateApplicationEnvironment(applicationId, applicationName);

            // Assert
            result.Should().BeEquivalentTo(model);
        }

        [Test]
        public void CreateApplicationListModel_NoCondition_ReturnApplicationListModel()
        {
            // Arrange
            var applicationList = new List<Application>();
            var applicationListModel = new ApplicationListModel
            {
                data = applicationList
            };

            // Act
            var result = factory.CreateApplicationListModel(applicationList);

            // Assert
            result.Should().BeEquivalentTo(applicationListModel);
            result.data.Should().BeSameAs(applicationListModel.data);
        }

        [Test]
        public void CreateApplicationListModel_NoCondition_ReturnApplicationFullModel()
        {
            // Arrange
            var application = new Application();
            var applicationEnvironments = new List<ApplicationEnvironment>();
            var svnRepositories = new List<SvnRepository>();
            var jenkinsJobs = new List<JenkinsJob>();
            var sonarqubeProjects = new List<SonarqubeProject>();
            var applicationNugetPackages = new List<ApplicationNugetPackage>();
            var expectedResult = new ApplicationFullModel
            {
                Application = application,
                ApplicationEnvironments = applicationEnvironments,
                ApplicationNugetPackages = applicationNugetPackages,
                JenkinsJobs = jenkinsJobs,
                SonarqubeProjects = sonarqubeProjects,
                SvnRepositories = svnRepositories
            };

            // Act
            var result = factory.CreateApplicationFullModel(application, applicationEnvironments, svnRepositories, jenkinsJobs, sonarqubeProjects, applicationNugetPackages);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion
    }
}