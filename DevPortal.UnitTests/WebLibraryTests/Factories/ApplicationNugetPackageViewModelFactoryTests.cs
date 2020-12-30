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
    public class ApplicationNugetPackageViewModelFactoryTests : BaseTestFixture
    {
        #region members & setup

        ApplicationNugetPackageViewModelFactory factory;

        StrictMock<IBreadCrumbFactory> breadCrumbFactory;

        StrictMock<IAuthorizationServiceWrapper> authorizationServiceWrapper;

        [SetUp]
        public void Initialize()
        {
            breadCrumbFactory = new StrictMock<IBreadCrumbFactory>();
            authorizationServiceWrapper = new StrictMock<IAuthorizationServiceWrapper>();

            factory = new ApplicationNugetPackageViewModelFactory(breadCrumbFactory.Object,
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

        #region application nuget package

        [Test]
        public void CreateDetailApplicationNugetPackageViewModel_ApplicationNull_ReturnNull()
        {
            // Arrange
            Application application = null;
            var package = new ApplicationNugetPackage();
            var packageUrl = "";

            // Act
            var result = factory.CreateDetailApplicationNugetPackageViewModel(application, package, packageUrl);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void CreateDetailApplicationNugetPackageViewModel_ApplicationNugetPackageNull_ReturnNull()
        {
            // Arrange
            var application = new Application();
            ApplicationNugetPackage package = null;
            var packageUrl = "";

            // Act
            var result = factory.CreateDetailApplicationNugetPackageViewModel(application, package, packageUrl);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void CreateDetailApplicationNugetPackageViewModel_ParametersValid_ReturnApplicationNugetPackageViewModelWithPackage()
        {
            // Arrange
            var application = new Application();
            var package = new ApplicationNugetPackage();
            var packageUrl = "";
            BreadCrumbViewModel breadcrumbViewModel = null;
            var favouritePageName = $"{application.Name} - NuGet Paketi - {package.NugetPackageName}";

            var viewModel = new ApplicationNugetPackageViewModel
            {
                ApplicationNugetPackage = package,
                NugetPackageUrl = packageUrl,
                BreadCrumbViewModel = breadcrumbViewModel,
                IsAuthorized = true,
                FavouritePageName = favouritePageName
            };
            viewModel.ApplicationNugetPackage.ApplicationName = application.Name;

            authorizationServiceWrapper.Setup(s => s.CheckUserHasAdminDeveloperPolicy()).Returns(true);
            breadCrumbFactory.Setup(x => x.CreateDetailNugetPackageModel(application.Id)).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateDetailApplicationNugetPackageViewModel(application, package, packageUrl);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
        }

        [Test]
        public void CreateApplicationNugetPackageViewModel_NoCondition_ReturnApplicationNugetPackageViewModelWithPackage()
        {
            // Arrange
            var application = new Application();
            var package = new ApplicationNugetPackage();
            BreadCrumbViewModel breadcrumbViewModel = null;
            var viewModel = new ApplicationNugetPackageViewModel
            {
                ApplicationNugetPackage = package,
                BreadCrumbViewModel = breadcrumbViewModel
            };
            viewModel.ApplicationNugetPackage.ApplicationName = application.Name;

            breadCrumbFactory.Setup(x => x.CreateNewNugetPackageModel(application.Id)).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateApplicationNugetPackageViewModel(application);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
        }

        [Test]
        public void CreateEditApplicationNugetPackageViewModel_NoCondition_ReturnApplicationNugetPackageViewModel()
        {
            // Arrange
            var application = new Application();
            var applicationNugetPackage = new ApplicationNugetPackage();
            var nugetPackageUrl = "nugetPackageUrl";
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var applicationNugetPackageViewModel = new ApplicationNugetPackageViewModel
            {
                ApplicationNugetPackage = applicationNugetPackage,
                NugetPackageUrl = nugetPackageUrl,
                BreadCrumbViewModel = breadcrumbViewModel
            };
            applicationNugetPackageViewModel.ApplicationNugetPackage.ApplicationName = application.Name;

            breadCrumbFactory.Setup(x => x.CreateEditNugetPackageModel(application.Id, applicationNugetPackage.NugetPackageId)).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateEditApplicationNugetPackageViewModel(application, applicationNugetPackage, nugetPackageUrl);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(applicationNugetPackageViewModel);
            result.ApplicationNugetPackage.Should().BeSameAs(applicationNugetPackageViewModel.ApplicationNugetPackage);
            result.NugetPackageUrl.Should().BeSameAs(applicationNugetPackageViewModel.NugetPackageUrl);
            result.BreadCrumbViewModel.Should().BeSameAs(applicationNugetPackageViewModel.BreadCrumbViewModel);
        }

        #endregion
    }
}