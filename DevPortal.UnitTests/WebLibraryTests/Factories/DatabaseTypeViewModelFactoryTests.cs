using AB.Framework.UnitTests;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Factories;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.WebLibraryTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class DatabaseTypeViewModelFactoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IBreadCrumbFactory> breadCrumbFactory;

        StrictMock<IAuthorizationServiceWrapper> authorizationServiceWrapper;

        DatabaseTypeViewModelFactory factory;

        [SetUp]
        public void Initialize()
        {
            breadCrumbFactory = new StrictMock<IBreadCrumbFactory>();
            authorizationServiceWrapper = new StrictMock<IAuthorizationServiceWrapper>();

            factory = new DatabaseTypeViewModelFactory(
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
        public void CreateEnvironmentsViewModel_NoCondition_ReturnDatabaseTypeViewModel()
        {
            // Arrange
            var databaseTypes = new List<DatabaseType>();
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var environmentListViewModel = new DatabaseTypeViewModel
            {
                DatabaseTypes = databaseTypes,
                BreadCrumbViewModel = breadcrumbViewModel,
                IsAuthorized = true
            };

            authorizationServiceWrapper.Setup(s => s.CheckUserHasAdminDeveloperPolicy()).Returns(true);
            breadCrumbFactory.Setup(x => x.CreateDatabaseTypesModel()).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateDatabaseTypeViewModel(databaseTypes);

            // Assert
            result.Should().BeEquivalentTo(environmentListViewModel);
        }

        [Test]
        public void CreateDatabaseTypeAddView_NoCondition_ReturnDatabaseTypeViewModel()
        {
            // Arrange
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var environmentListViewModel = new DatabaseTypeViewModel
            {
                BreadCrumbViewModel = breadcrumbViewModel,
            };

            breadCrumbFactory.Setup(x => x.CreateDatabaseTypeAddModel()).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateDatabaseTypeAddView();

            // Assert
            result.Should().BeEquivalentTo(environmentListViewModel);
        }

        [Test]
        public void CreateDatabaseTypeDetailViewModel_NoCondition_ReturnDatabaseTypeViewModel()
        {
            // Arrange
            var databaseType = new DatabaseType();
            var databases = new List<Database>();
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var environmentListViewModel = new DatabaseTypeViewModel
            {
                DatabaseType = databaseType,
                Databases = databases,
                BreadCrumbViewModel = breadcrumbViewModel,
                IsAuthorized = true
            };

            authorizationServiceWrapper.Setup(s => s.CheckUserHasAdminDeveloperPolicy()).Returns(true);
            breadCrumbFactory.Setup(x => x.CreateDatabaseTypeDetailModel(databaseType.Id)).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateDatabaseTypeDetailViewModel(databaseType, databases);

            // Assert
            result.Should().BeEquivalentTo(environmentListViewModel);
        }

        [Test]
        public void CreateDatabaseTypeEditViewModel_NoCondition_ReturnDatabaseTypeViewModel()
        {
            // Arrange
            var databaseType = new DatabaseType();
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var environmentListViewModel = new DatabaseTypeViewModel
            {
                DatabaseType = databaseType,
                BreadCrumbViewModel = breadcrumbViewModel
            };

            breadCrumbFactory.Setup(x => x.CreateDatabaseTypeEditModel(databaseType.Id)).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateDatabaseTypeEditViewModel(databaseType);

            // Assert
            result.Should().BeEquivalentTo(environmentListViewModel);
        }
    }
}