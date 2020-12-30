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
    public class ApplicationDependencyViewModelFactoryTests : BaseTestFixture
    {
        #region members & setup

        ApplicationDependencyViewModelFactory factory;

        StrictMock<IBreadCrumbFactory> breadCrumbFactory;

        StrictMock<IAuthorizationServiceWrapper> authorizationServiceWrapper;

        [SetUp]
        public void Initialize()
        {
            breadCrumbFactory = new StrictMock<IBreadCrumbFactory>();
            authorizationServiceWrapper = new StrictMock<IAuthorizationServiceWrapper>();

            factory = new ApplicationDependencyViewModelFactory(
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
        public void CreatApplicationDependencyViewModelAddView_NoCondition_ReturnApplicationDependencyViewModel()
        {
            // Arrange
            int applicationId = 1;
            var applications = new List<ApplicationListItem>();
            var applicationGroups = new List<ApplicationGroup>();
            var applicationDependency = new ApplicationDependency
            {
                DependentApplicationId = applicationId
            };
            var breadCrumb = new BreadCrumbViewModel();
            var viewModel = new ApplicationDependencyViewModel()
            {
                ApplicationDependency = applicationDependency,
                BreadCrumbViewModel = breadCrumb,
                ApplicationGroups = applicationGroups,
                Applications = applications
            };

            breadCrumbFactory.Setup(x => x.CreateApplicationDependencyAddModel(applicationId)).Returns(breadCrumb);

            // Act
            var result = factory.CreatApplicationDependencyViewModelAddView(applicationId, applications, applicationGroups);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
            result.Applications.Should().BeSameAs(applications);
            result.ApplicationGroups.Should().BeSameAs(applicationGroups);
            result.BreadCrumbViewModel.Should().BeSameAs(breadCrumb);
        }

        #region CreateApplicationDependencyViewModel

        [Test]
        public void CreateApplicationDependencyViewModel_ApplicationDependencyNull_ReturnNull()
        {
            // Arrange
            ApplicationDependency applicationDependency = null;

            // Act
            var result = factory.CreateApplicationDependencyViewModel(applicationDependency);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void CreateApplicationDependencyViewModel_ApplicationDependencyValid_ReturnApplicationDependencyViewModel(bool returnValue)
        {
            // Arrange
            var applicationDependency = new ApplicationDependency();
            var breadCrumb = new BreadCrumbViewModel();
            var favouritePageName = $"{applicationDependency.ApplicationName} - Uygulama Bağımlılığı - {applicationDependency.Name}";

            var viewModel = new ApplicationDependencyViewModel()
            {
                ApplicationDependency = applicationDependency,
                BreadCrumbViewModel = breadCrumb,
                IsAuthorized = returnValue,
                FavouritePageName = favouritePageName
            };

            authorizationServiceWrapper.Setup(x => x.CheckUserHasAdminDeveloperPolicy()).Returns(returnValue);
            breadCrumbFactory.Setup(x => x.CreateApplicationDependencyDetailModel(applicationDependency.Id)).Returns(breadCrumb);

            // Act
            var result = factory.CreateApplicationDependencyViewModel(applicationDependency);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
            result.ApplicationDependency.Should().BeSameAs(applicationDependency);
            result.BreadCrumbViewModel.Should().BeSameAs(breadCrumb);
        }

        #endregion

        [Test]
        public void CreateApplicationDependencyEditViewModel_NoCondition_ReturnApplicationDependencyViewModel()
        {
            // Arrange
            var applicationDependency = new ApplicationDependency
            {
                Id = 1,
                DependentApplicationId = 44
            };
            var breadCrumb = new BreadCrumbViewModel();
            var viewModel = new ApplicationDependencyViewModel()
            {
                ApplicationDependency = applicationDependency,
                BreadCrumbViewModel = breadCrumb
            };

            breadCrumbFactory.Setup(x => x.CreateApplicationDependencyEditModel(applicationDependency.DependentApplicationId, applicationDependency.Id)).Returns(breadCrumb);

            // Act
            var result = factory.CreateApplicationDependencyEditViewModel(applicationDependency);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
            result.ApplicationDependency.Should().BeSameAs(applicationDependency);
            result.BreadCrumbViewModel.Should().BeSameAs(breadCrumb);
        }
    }
}