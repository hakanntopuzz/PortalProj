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
    public class NugetPackageDependencyViewModelFactoryTests : BaseTestFixture
    {
        #region members & setup

        NugetPackageDependencyViewModelFactory factory;

        StrictMock<IBreadCrumbFactory> breadCrumbFactory;

        StrictMock<IAuthorizationServiceWrapper> authorizationServiceWrapper;

        [SetUp]
        public void Initialize()
        {
            breadCrumbFactory = new StrictMock<IBreadCrumbFactory>();
            authorizationServiceWrapper = new StrictMock<IAuthorizationServiceWrapper>();

            factory = new NugetPackageDependencyViewModelFactory(
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

        #region CreateApplicationDependencyViewModel

        [Test]
        public void CreateNugetPackageDependencyViewModel_NugetPackageDependencyNull_ReturnNull()
        {
            // Arrange
            NugetPackageDependency nugetPackageDependency = null;

            // Act
            var result = factory.CreateNugetPackageDependencyViewModel(nugetPackageDependency);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void CreateNugetPackageDependencyViewModel_NugetPackageDependencyValid_ReturnNugetPackageDependencyViewModel(bool returnValue)
        {
            // Arrange
            var nugetPackageDependency = new NugetPackageDependency();
            var breadCrumb = new BreadCrumbViewModel();
            var favouritePageName = $"{nugetPackageDependency.ApplicationName} - Nuget Paketi Bağımlılığı - {nugetPackageDependency.NugetPackageName}";

            var viewModel = new NugetPackageDependencyViewModel()
            {
                NugetPackageDependency = nugetPackageDependency,
                BreadCrumbViewModel = breadCrumb,
                IsAuthorized = returnValue,
                FavouritePageName = favouritePageName
            };

            authorizationServiceWrapper.Setup(x => x.CheckUserHasAdminDeveloperPolicy()).Returns(returnValue);
            breadCrumbFactory.Setup(x => x.CreateNugetPackageDependencyDetailModel(nugetPackageDependency.DependentApplicationId)).Returns(breadCrumb);

            // Act
            var result = factory.CreateNugetPackageDependencyViewModel(nugetPackageDependency);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
            result.NugetPackageDependency.Should().BeSameAs(nugetPackageDependency);
            result.BreadCrumbViewModel.Should().BeSameAs(breadCrumb);
        }

        #endregion

        #region CreateNugetPackageDependencyViewModelAddView

        [Test]
        public void CreateNugetPackageDependencyViewModelAddView_NoCondition_ReturnNugetPackageDependencyViewModel()
        {
            // Arrange
            var application = new Application
            {
                Name = "Name",
                Id = 5
            };
            var nugetPackageDependency = new NugetPackageDependency
            {
                ApplicationName = application.Name,
                DependentApplicationId = application.Id
            };
            var breadCrumb = new BreadCrumbViewModel();
            var viewModel = new NugetPackageDependencyViewModel()
            {
                NugetPackageDependency = nugetPackageDependency,
                BreadCrumbViewModel = breadCrumb
            };

            breadCrumbFactory.Setup(x => x.CreateNugetPackageDependencyAddModel(application.Id)).Returns(breadCrumb);

            // Act
            var result = factory.CreateNugetPackageDependencyViewModelAddView(application);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
            result.NugetPackageDependency.ApplicationName.Should().BeSameAs(application.Name);
            result.NugetPackageDependency.DependentApplicationId.Should().Be(application.Id);
            result.BreadCrumbViewModel.Should().BeSameAs(breadCrumb);
        }

        #endregion
    }
}