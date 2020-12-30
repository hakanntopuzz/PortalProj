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
    public class DatabaseDependencyViewModelFactoryTests : BaseTestFixture
    {
        #region members & setup

        DatabaseDependencyViewModelFactory factory;

        StrictMock<IBreadCrumbFactory> breadCrumbFactory;

        StrictMock<IAuthorizationServiceWrapper> authorizationServiceWrapper;

        [SetUp]
        public void Initialize()
        {
            breadCrumbFactory = new StrictMock<IBreadCrumbFactory>();
            authorizationServiceWrapper = new StrictMock<IAuthorizationServiceWrapper>();

            factory = new DatabaseDependencyViewModelFactory(
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

        #region CreateDatabaseDependencyViewModel

        [Test]
        public void CreateDatabaseDependencyViewModel_DatabaseDependencyNull_ReturnNull()
        {
            // Arrange
            DatabaseDependency databaseDependency = null;

            // Act
            var result = factory.CreateDatabaseDependencyViewModel(databaseDependency);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void CreateDatabaseDependencyViewModel_ParametersValidAndConditionsInTestCases_ReturnDatabaseDependencyViewModel(bool returnValue)
        {
            // Arrange
            var databaseDependencyViewModel = new DatabaseDependencyViewModel();
            var databaseDependency = new DatabaseDependency
            {
                ApplicationId = 2,
                ApplicationName = "ApplicationName"
            };
            var breadCrumb = new BreadCrumbViewModel();
            var favouritePageName = $"{databaseDependency.ApplicationName} - Veritabanı Bağımlılığı - {databaseDependency.Name}";

            var viewModel = new DatabaseDependencyViewModel
            {
                DatabaseDependency = databaseDependency,
                BreadCrumbViewModel = breadCrumb,
                IsAuthorized = returnValue,
                FavouritePageName = favouritePageName
            };

            authorizationServiceWrapper.Setup(x => x.CheckUserHasAdminDeveloperPolicy()).Returns(returnValue);
            breadCrumbFactory.Setup(x => x.CreateDatabaseDependencyDetailModel(databaseDependency.DependentApplicationId)).Returns(breadCrumb);

            // Act
            var result = factory.CreateDatabaseDependencyViewModel(databaseDependency);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
            result.DatabaseDependency.Should().BeSameAs(databaseDependency);
            result.IsAuthorized.Should().Be(returnValue);
            result.BreadCrumbViewModel.Should().BeSameAs(breadCrumb);
        }

        #endregion

        [Test]
        public void CreatDatabaseDependencyViewModelAddView_NoCondition_ReturnDatabaseDependencyViewModel()
        {
            // Arrange
            var applicationId = 5;
            var databaseDependency = new DatabaseDependency
            {
                ApplicationId = applicationId
            };
            var databaseGroups = new List<DatabaseGroup>();
            var databases = new List<Database>();
            var breadCrumb = new BreadCrumbViewModel();
            var viewModel = new DatabaseDependencyViewModel
            {
                DatabaseDependency = databaseDependency,
                Databases = databases,
                DatabaseGroups = databaseGroups,
                BreadCrumbViewModel = breadCrumb
            };

            breadCrumbFactory.Setup(x => x.CreateDatabaseDependencyAddModel(applicationId)).Returns(breadCrumb);

            // Act
            var result = factory.CreatDatabaseDependencyViewModelAddView(applicationId, databaseGroups, databases);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
            result.DatabaseGroups.Should().BeSameAs(databaseGroups);
            result.Databases.Should().BeSameAs(databases);
            result.DatabaseDependency.Should().BeEquivalentTo(databaseDependency);
            result.BreadCrumbViewModel.Should().BeSameAs(breadCrumb);
        }

        [Test]
        public void CreateDatabaseDependencyEditViewModel_NoCondition_ReturnDatabaseDependencyViewModel()
        {
            // Arrange
            var databaseDependency = new DatabaseDependency
            {
                ApplicationId = 3,
                Id = 5,
                ApplicationName = "ApplicationName"
            };
            var breadCrumb = new BreadCrumbViewModel();
            var viewModel = new DatabaseDependencyViewModel()
            {
                DatabaseDependency = databaseDependency,
                BreadCrumbViewModel = breadCrumb
            };

            breadCrumbFactory.Setup(x => x.CreateDatabaseDependencyEditModel(databaseDependency.DependentApplicationId, databaseDependency.Id)).Returns(breadCrumb);

            // Act
            var result = factory.CreateDatabaseDependencyEditViewModel(databaseDependency);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
            result.DatabaseDependency.Should().BeSameAs(databaseDependency);
            result.BreadCrumbViewModel.Should().BeSameAs(breadCrumb);
        }
    }
}