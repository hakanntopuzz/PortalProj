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
    public class ApplicationJenkinsJobViewModelFactoryTests : BaseTestFixture
    {
        #region members & setup

        ApplicationJenkinsJobViewModelFactory factory;

        StrictMock<IBreadCrumbFactory> breadCrumbFactory;

        StrictMock<IAuthorizationServiceWrapper> authorizationServiceWrapper;

        [SetUp]
        public void Initialize()
        {
            breadCrumbFactory = new StrictMock<IBreadCrumbFactory>();
            authorizationServiceWrapper = new StrictMock<IAuthorizationServiceWrapper>();

            factory = new ApplicationJenkinsJobViewModelFactory(breadCrumbFactory.Object,
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

        #region application jenkins jobs

        [Test]
        public void CreateApplicationJenkinsJobViewModel_NoCondition_ReturnApplicationJenkinsJobViewModel()
        {
            // Arrange
            var application = new Application();
            var jenkinsJobTypeList = new List<JenkinsJobType>();
            var jenkinsJobUrl = "jenkinsJobUrl";
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var viewModel = new ApplicationJenkinsJobViewModel
            {
                ApplicationJenkinsJob = new JenkinsJob
                {
                    ApplicationId = application.Id,
                    ApplicationName = application.Name
                },
                JenkinsJobTypeList = jenkinsJobTypeList,
                JenkinsJobUrl = jenkinsJobUrl,
                BreadCrumbViewModel = breadcrumbViewModel
            };

            breadCrumbFactory.Setup(x => x.CreateNewJenkinsJobModel(application.Id)).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateApplicationJenkinsJobViewModel(application, jenkinsJobTypeList, jenkinsJobUrl);

            // Assert
            result.Should().BeEquivalentTo(viewModel);
            result.ApplicationJenkinsJob.Should().BeEquivalentTo(viewModel.ApplicationJenkinsJob);
            result.ApplicationJenkinsJob.ApplicationId.Should().Be(viewModel.ApplicationJenkinsJob.ApplicationId);
            result.JenkinsJobTypeList.Should().BeEquivalentTo(viewModel.JenkinsJobTypeList);
            result.JenkinsJobUrl.Should().BeSameAs(viewModel.JenkinsJobUrl);
            result.BreadCrumbViewModel.Should().BeSameAs(viewModel.BreadCrumbViewModel);
        }

        [Test]
        public void CreateApplicationJenkinsJobViewModel2_NoCondition_ReturnApplicationJenkinsJobViewModel()
        {
            // Arrange
            var application = new Application();
            var jenkinsJob = new JenkinsJob();
            var jenkinsJobTypeList = new List<JenkinsJobType>();
            var jenkinsJobUrl = "jenkinsJobUrl";
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var viewModel = new ApplicationJenkinsJobViewModel
            {
                ApplicationJenkinsJob = jenkinsJob,
                JenkinsJobTypeList = jenkinsJobTypeList,
                JenkinsJobUrl = jenkinsJobUrl,
                BreadCrumbViewModel = breadcrumbViewModel
            };

            breadCrumbFactory.Setup(x => x.CreateNewJenkinsJobModel(application.Id)).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateApplicationJenkinsJobViewModel(application, jenkinsJob, jenkinsJobTypeList, jenkinsJobUrl);

            // Assert
            result.Should().BeEquivalentTo(viewModel);
            result.ApplicationJenkinsJob.Should().BeSameAs(viewModel.ApplicationJenkinsJob);
            result.JenkinsJobTypeList.Should().BeEquivalentTo(viewModel.JenkinsJobTypeList);
            result.JenkinsJobUrl.Should().BeSameAs(viewModel.JenkinsJobUrl);
            result.BreadCrumbViewModel.Should().BeSameAs(viewModel.BreadCrumbViewModel);
        }

        [Test]
        public void CreateDetailApplicationJenkinsJobViewModel_ApplicationNull_ReturnNull()
        {
            // Arrange
            Application application = null;
            var jenkinsJob = new JenkinsJob();
            var jenkinsJobTypeList = new List<JenkinsJobType>();
            var jenkinsJobUrl = "jenkinsJobUrl";

            // Act
            var result = factory.CreateDetailApplicationJenkinsJobViewModel(application, jenkinsJob, jenkinsJobTypeList, jenkinsJobUrl);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void CreateDetailApplicationJenkinsJobViewModel_JenkinsJobNull_ReturnNull()
        {
            // Arrange
            var application = new Application();
            JenkinsJob jenkinsJob = null;
            var jenkinsJobTypeList = new List<JenkinsJobType>();
            var jenkinsJobUrl = "jenkinsJobUrl";

            // Act
            var result = factory.CreateDetailApplicationJenkinsJobViewModel(application, jenkinsJob, jenkinsJobTypeList, jenkinsJobUrl);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void CreateDetailApplicationJenkinsJobViewModel_ParametersValid_ReturnApplicationJenkinsJobViewModel()
        {
            // Arrange
            var application = new Application();
            var jenkinsJob = new JenkinsJob();
            var jenkinsJobTypeList = new List<JenkinsJobType>();
            var jenkinsJobUrl = "jenkinsJobUrl";
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var favouritePageName = $"{application.Name} - Jenkins Görevi - {jenkinsJob.JenkinsJobName}";

            var viewModel = new ApplicationJenkinsJobViewModel
            {
                ApplicationJenkinsJob = jenkinsJob,
                JenkinsJobTypeList = jenkinsJobTypeList,
                JenkinsJobUrl = jenkinsJobUrl,
                BreadCrumbViewModel = breadcrumbViewModel,
                IsAuthorized = true,
                FavouritePageName = favouritePageName
            };

            authorizationServiceWrapper.Setup(s => s.CheckUserHasAdminDeveloperPolicy()).Returns(true);
            breadCrumbFactory.Setup(x => x.CreateDetailJenkinsJobModel(application.Id)).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateDetailApplicationJenkinsJobViewModel(application, jenkinsJob, jenkinsJobTypeList, jenkinsJobUrl);

            // Assert
            result.Should().BeEquivalentTo(viewModel);
            result.ApplicationJenkinsJob.Should().BeSameAs(viewModel.ApplicationJenkinsJob);
            result.JenkinsJobTypeList.Should().BeEquivalentTo(viewModel.JenkinsJobTypeList);
            result.JenkinsJobUrl.Should().BeSameAs(viewModel.JenkinsJobUrl);
            result.IsAuthorized.Should().BeTrue();
            result.BreadCrumbViewModel.Should().BeSameAs(viewModel.BreadCrumbViewModel);
        }

        [Test]
        public void CreateEditApplicationJenkinsJobViewModel_NoCondition_ReturnApplicationJenkinsJobViewModel()
        {
            // Arrange
            var application = new Application();
            var jenkinsJob = new JenkinsJob();
            var jenkinsJobTypeList = new List<JenkinsJobType>();
            var jenkinsJobUrl = "jenkinsJobUrl";
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var viewModel = new ApplicationJenkinsJobViewModel
            {
                ApplicationJenkinsJob = jenkinsJob,
                JenkinsJobTypeList = jenkinsJobTypeList,
                JenkinsJobUrl = jenkinsJobUrl,
                BreadCrumbViewModel = breadcrumbViewModel
            };

            breadCrumbFactory.Setup(x => x.CreateEditJenkinsJobModel(jenkinsJob.JenkinsJobId, application.Id)).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateEditApplicationJenkinsJobViewModel(application, jenkinsJob, jenkinsJobTypeList, jenkinsJobUrl);

            // Assert
            result.Should().BeEquivalentTo(viewModel);
            result.ApplicationJenkinsJob.Should().BeSameAs(viewModel.ApplicationJenkinsJob);
            result.JenkinsJobTypeList.Should().BeEquivalentTo(viewModel.JenkinsJobTypeList);
            result.JenkinsJobUrl.Should().BeSameAs(viewModel.JenkinsJobUrl);
            result.BreadCrumbViewModel.Should().BeSameAs(viewModel.BreadCrumbViewModel);
        }

        #endregion
    }
}