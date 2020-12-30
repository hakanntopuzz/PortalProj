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
    public class ApplicationSonarQubeProjectViewModelFactoryTests : BaseTestFixture
    {
        #region members & setup

        ApplicationSonarQubeProjectViewModelFactory factory;

        StrictMock<IBreadCrumbFactory> breadCrumbFactory;

        StrictMock<IAuthorizationServiceWrapper> authorizationServiceWrapper;

        [SetUp]
        public void Initialize()
        {
            breadCrumbFactory = new StrictMock<IBreadCrumbFactory>();
            authorizationServiceWrapper = new StrictMock<IAuthorizationServiceWrapper>();

            factory = new ApplicationSonarQubeProjectViewModelFactory(breadCrumbFactory.Object,
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

        #region application sonarQube project

        [Test]
        public void CreateApplicationSonarQubeProjectViewModel_NoCondition_ReturnApplicationSonarQubeProjectViewModel()
        {
            // Arrange
            var projectTypeList = new List<SonarQubeProjectType>();
            var application = new Application();
            var projectUrl = "";
            BreadCrumbViewModel breadcrumbViewModel = null;
            var viewModel = new ApplicationSonarQubeProjectViewModel
            {
                ApplicationSonarQubeProject = new SonarqubeProject
                {
                    ApplicationId = application.Id,
                    ApplicationName = application.Name
                },
                SonarQubeProjectTypeList = projectTypeList,
                SonarQubeProjectUrl = projectUrl,
                BreadCrumbViewModel = breadcrumbViewModel
            };

            breadCrumbFactory.Setup(x => x.CreateNewSonarQubeProjectModel(application.Id)).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateApplicationSonarQubeProjectViewModel(application, projectTypeList, projectUrl);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
        }

        #region CreateApplicationSonarQubeProjectViewModel

        [Test]
        public void CreateApplicationSonarQubeProjectViewModel_ApplicationNull_ReturnNull()
        {
            // Arrange
            var projectTypeList = new List<SonarQubeProjectType>();
            Application application = null;
            var project = new SonarqubeProject();
            var projectUrl = "";

            // Act
            var result = factory.CreateApplicationSonarQubeProjectViewModel(application, project, projectTypeList, projectUrl);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void CreateApplicationSonarQubeProjectViewModel_SonarqubeProjectNull_ReturnNull()
        {
            // Arrange
            var projectTypeList = new List<SonarQubeProjectType>();
            var application = new Application();
            SonarqubeProject project = null;
            var projectUrl = "";

            // Act
            var result = factory.CreateApplicationSonarQubeProjectViewModel(application, project, projectTypeList, projectUrl);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void CreateApplicationSonarQubeProjectViewModel_ParametersValid_ReturnApplicationSonarQubeProjectViewModelWithProject()
        {
            // Arrange
            var projectTypeList = new List<SonarQubeProjectType>();
            var application = new Application();
            var project = new SonarqubeProject();
            var projectUrl = "";
            BreadCrumbViewModel breadcrumbViewModel = null;
            var favouritePageName = $"{application.Name} - SonarQube Projesi - {project.SonarqubeProjectName}";

            var viewModel = new ApplicationSonarQubeProjectViewModel
            {
                ApplicationSonarQubeProject = project,
                SonarQubeProjectTypeList = projectTypeList,
                SonarQubeProjectUrl = projectUrl,
                BreadCrumbViewModel = breadcrumbViewModel,
                IsAuthorized = true,
                FavouritePageName = favouritePageName
            };
            viewModel.ApplicationSonarQubeProject.ApplicationName = application.Name;

            authorizationServiceWrapper.Setup(s => s.CheckUserHasAdminDeveloperPolicy()).Returns(true);
            breadCrumbFactory.Setup(x => x.CreateDetailSonarQubeProjectModel(application.Id)).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateApplicationSonarQubeProjectViewModel(application, project, projectTypeList, projectUrl);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
        }

        #endregion

        [Test]
        public void CreateEditApplicationSonarQubeProjectViewModel_NoCondition_ReturnApplicationSonarQubeProjectViewModel()
        {
            // Arrange
            var projectTypeList = new List<SonarQubeProjectType>();
            var application = new Application();
            var project = new SonarqubeProject();
            var projectUrl = "";
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var applicationSonarQubeProjectViewModel = new ApplicationSonarQubeProjectViewModel
            {
                ApplicationSonarQubeProject = project,
                SonarQubeProjectTypeList = projectTypeList,
                SonarQubeProjectUrl = projectUrl,
                BreadCrumbViewModel = breadcrumbViewModel
            };
            applicationSonarQubeProjectViewModel.ApplicationSonarQubeProject.ApplicationName = application.Name;

            breadCrumbFactory.Setup(x => x.CreateEditSonarQubeProjectModel(project.SonarqubeProjectId, application.Id)).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateEditApplicationSonarQubeProjectViewModel(application, project, projectTypeList, projectUrl);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(applicationSonarQubeProjectViewModel);
            result.ApplicationSonarQubeProject.Should().BeSameAs(applicationSonarQubeProjectViewModel.ApplicationSonarQubeProject);
            result.SonarQubeProjectTypeList.Should().BeSameAs(applicationSonarQubeProjectViewModel.SonarQubeProjectTypeList);
            result.SonarQubeProjectUrl.Should().BeSameAs(applicationSonarQubeProjectViewModel.SonarQubeProjectUrl);
            result.BreadCrumbViewModel.Should().BeSameAs(applicationSonarQubeProjectViewModel.BreadCrumbViewModel);
        }

        #endregion
    }
}