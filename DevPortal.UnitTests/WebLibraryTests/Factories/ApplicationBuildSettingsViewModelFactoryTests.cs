using AB.Framework.UnitTests;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Factories;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using NUnit.Framework;

namespace DevPortal.UnitTests.WebLibraryTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationBuildSettingsViewModelFactoryTests : BaseTestFixture
    {
        #region members & setup

        ApplicationBuildSettingsViewModelFactory factory;

        StrictMock<IBreadCrumbFactory> breadCrumbFactory;

        StrictMock<IAuthorizationServiceWrapper> authorizationServiceWrapper;

        [SetUp]
        public void Initialize()
        {
            breadCrumbFactory = new StrictMock<IBreadCrumbFactory>();
            authorizationServiceWrapper = new StrictMock<IAuthorizationServiceWrapper>();

            factory = new ApplicationBuildSettingsViewModelFactory(
                breadCrumbFactory.Object,
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

        [Test]
        public void CreateApplicationBuildSettingsViewModel_ApplicationNameIsNull_ReturnNull()
        {
            // Arrange
            var buildSettings = new ApplicationBuildSettings();
            var application = new Application();

            // Act
            var result = factory.CreateApplicationBuildSettingsViewModel(buildSettings, application);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void CreateApplicationBuildSettingsViewModel_BuildSettingsIsNull_ReturnNull(bool isAuthorized)
        {
            // Arrange
            ApplicationBuildSettings buildSettings = null;
            var application = new Application { Id = 1, Name = "application-name" };
            var breadCrumb = new BreadCrumbViewModel();
            var viewModel = new ApplicationBuildSettingsViewModel
            {
                BuildSettings = new ApplicationBuildSettings { ApplicationId = application.Id },
                ApplicationName = application.Name,
                BreadCrumbViewModel = breadCrumb,
                IsAuthorized = isAuthorized
            };

            breadCrumbFactory.Setup(x => x.CreateApplicationBuildSettingsModel(application.Id)).Returns(breadCrumb);
            authorizationServiceWrapper.Setup(x => x.CheckUserHasAdminDeveloperPolicy()).Returns(isAuthorized);

            // Act
            var result = factory.CreateApplicationBuildSettingsViewModel(buildSettings, application);

            // Assert
            result.Should().BeEquivalentTo(viewModel);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void CreateApplicationBuildSettingsViewModel_ApplicationIsNotNull_ReturnApplicationBuildSettingsViewModel(bool isAuthorized)
        {
            // Arrange
            var applicationdId = 1;
            var buildSettings = new ApplicationBuildSettings { ApplicationId = applicationdId };
            var application = new Application { Id = 1, Name = "application-name" };
            var breadCrumb = new BreadCrumbViewModel();
            var viewModel = new ApplicationBuildSettingsViewModel
            {
                BuildSettings = buildSettings,
                ApplicationName = application.Name,
                BreadCrumbViewModel = breadCrumb,
                IsAuthorized = isAuthorized
            };

            breadCrumbFactory.Setup(x => x.CreateApplicationBuildSettingsModel(application.Id)).Returns(breadCrumb);
            authorizationServiceWrapper.Setup(x => x.CheckUserHasAdminDeveloperPolicy()).Returns(isAuthorized);

            // Act
            var result = factory.CreateApplicationBuildSettingsViewModel(buildSettings, application);

            // Assert
            result.Should().BeEquivalentTo(viewModel);
            result.BuildSettings.Should().Be(buildSettings);
            result.ApplicationName.Should().Be(application.Name);
            result.BreadCrumbViewModel.Should().BeSameAs(breadCrumb);
        }
    }
}