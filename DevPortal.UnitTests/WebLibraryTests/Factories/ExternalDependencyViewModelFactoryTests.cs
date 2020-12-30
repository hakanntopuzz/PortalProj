using AB.Framework.UnitTests;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using NUnit.Framework;

namespace DevPortal.UnitTests.WebLibraryTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ExternalDependencyViewModelFactoryTests : BaseTestFixture
    {
        #region members & setup

        ExternalDependencyViewModelFactory factory;

        StrictMock<IBreadCrumbFactory> breadCrumbFactory;

        StrictMock<IAuthorizationServiceWrapper> authorizationServiceWrapper;

        [SetUp]
        public void Initialize()
        {
            breadCrumbFactory = new StrictMock<IBreadCrumbFactory>();
            authorizationServiceWrapper = new StrictMock<IAuthorizationServiceWrapper>();

            factory = new ExternalDependencyViewModelFactory(
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

        #region CreateExternalDependencyViewModel

        [Test]
        public void CreateExternalDependencyViewModel_ExternalDependencyNull_ReturnNull()
        {
            // Arrange
            ExternalDependency externalDependency = null;

            // Act
            var result = factory.CreateExternalDependencyViewModel(externalDependency);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void CreateExternalDependencyViewModel_ParametersValidAndConditionsInTestCases_ReturnExternalDependencyViewModel(bool returnValue)
        {
            // Arrange
            var externalDependency = new ExternalDependency
            {
                ApplicationId = 2
            };
            var breadCrumb = new BreadCrumbViewModel();
            var favouritePageName = $"{externalDependency.ApplicationName} - Harici Bağımlılık - {externalDependency.Name}";

            var viewModel = new ExternalDependencyViewModel
            {
                ExternalDependency = externalDependency,
                BreadCrumbViewModel = breadCrumb,
                IsAuthorized = returnValue,
                FavouritePageName = favouritePageName
            };

            authorizationServiceWrapper.Setup(x => x.CheckUserHasAdminDeveloperPolicy()).Returns(returnValue);
            breadCrumbFactory.Setup(x => x.CreateExternalDependencyDetailModel(externalDependency.ApplicationId)).Returns(breadCrumb);

            // Act
            var result = factory.CreateExternalDependencyViewModel(externalDependency);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
            result.ExternalDependency.Should().BeSameAs(externalDependency);
            result.IsAuthorized.Should().Be(returnValue);
            result.BreadCrumbViewModel.Should().BeSameAs(breadCrumb);
        }

        #endregion

        [Test]
        public void CreateAddExternalDependencyViewModel_NoCondition_ReturnExternalDependencyViewModel()
        {
            // Arrange
            var applicationId = 5;
            var applicationName = "applicationName";
            var externalDependency = new ExternalDependency
            {
                ApplicationId = applicationId,
                ApplicationName = applicationName
            };
            var breadCrumb = new BreadCrumbViewModel();
            var viewModel = new ExternalDependencyViewModel
            {
                ExternalDependency = externalDependency,
                BreadCrumbViewModel = breadCrumb
            };

            breadCrumbFactory.Setup(x => x.CreateExternalDependencyAddModel(applicationId)).Returns(breadCrumb);

            // Act
            var result = factory.CreateAddExternalDependencyViewModel(applicationName, applicationId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
            result.ExternalDependency.Should().BeEquivalentTo(externalDependency);
            result.BreadCrumbViewModel.Should().BeSameAs(breadCrumb);
        }

        [Test]
        public void CreateEditExternalDependencyViewModel_NoCondition_ReturnExternalDependencyViewModel()
        {
            // Arrange
            var externalDependency = new ExternalDependency
            {
                ApplicationId = 5,
                Id = 3
            };
            var breadCrumb = new BreadCrumbViewModel();
            var viewModel = new ExternalDependencyViewModel
            {
                ExternalDependency = externalDependency,
                BreadCrumbViewModel = breadCrumb
            };

            breadCrumbFactory.Setup(x => x.CreateExternalDependencyEditModel(externalDependency.ApplicationId, externalDependency.Id)).Returns(breadCrumb);

            // Act
            var result = factory.CreateEditExternalDependencyViewModel(externalDependency);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
            result.ExternalDependency.Should().BeSameAs(externalDependency);
            result.BreadCrumbViewModel.Should().BeSameAs(breadCrumb);
        }
    }
}