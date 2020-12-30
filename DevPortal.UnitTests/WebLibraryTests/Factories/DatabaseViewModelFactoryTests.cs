using AB.Framework.UnitTests;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Factories;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DevPortal.UnitTests.WebLibraryTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class DatabaseViewModelFactoryTests : BaseTestFixture
    {
        #region members & setup

        DatabaseViewModelFactory factory;

        StrictMock<IBreadCrumbFactory> breadCrumbFactory;

        StrictMock<IAuthorizationServiceWrapper> authorizationServiceWrapper;

        [SetUp]
        public void Initialize()
        {
            breadCrumbFactory = new StrictMock<IBreadCrumbFactory>();
            authorizationServiceWrapper = new StrictMock<IAuthorizationServiceWrapper>();

            factory = new DatabaseViewModelFactory(
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
        public void CreateDatabaseListModel_NoCondition_ReturndatabaseListModel()
        {
            // Arrange
            var databaseList = new List<Database>();
            var databaseListModel = new DatabaseListModel
            {
                data = databaseList
            };

            // Act
            var result = factory.CreateDatabaseListModel(databaseList);

            // Assert
            result.Should().BeEquivalentTo(databaseListModel);
            result.data.Should().BeSameAs(databaseListModel.data);
        }

        [Test]
        public void CreateDatabaseViewModel_NoCondition_ReturnModel()
        {
            // Arrange
            var databaseGroups = new List<DatabaseGroup>();
            var databases = new List<Database>();
            var breadcrumbViewModel = new BreadCrumbViewModel();

            authorizationServiceWrapper.Setup(s => s.CheckUserHasAdminDeveloperPolicy()).Returns(true);
            breadCrumbFactory.Setup(x => x.CreateDatabasesModel()).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateDatabasesViewModel(databaseGroups, databases);

            // Assert
            result.DatabaseGroups.Should().BeSameAs(databaseGroups);
            result.Databases.Should().BeSameAs(databases);
        }

        [Test]
        public void Createdatabase_NoCondition_ReturnViewModel()
        {
            // Arrange

            var database = new Database
            {
                Id = 45,
                Name = "app-name",
                RedmineProjectUrl = "redmine/project/url",
                DatabaseGroupName = "group-name",
                DatabaseGroupId = 74,
                DatabaseTypeName = "app-type",
                ModifiedDate = DateTime.Today,
                TotalCount = 1,
                RedmineProjectName = "redmine-project",
                DatabaseTypeId = 1,
                Description = "açıklama"
            };

            var breadcrumbViewModel = new BreadCrumbViewModel();

            authorizationServiceWrapper.Setup(s => s.CheckUserHasAdminDeveloperPolicy()).Returns(true);
            breadCrumbFactory.Setup(x => x.CreateDatabaseDetailModel(database.Id)).Returns(breadcrumbViewModel);

            var viewModel = new DatabaseViewModel
            {
                Database = database,
                BreadCrumbViewModel = breadcrumbViewModel,
                IsAuthorized = true
            };

            // Act
            var result = factory.CreateDatabase(database);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
        }

        [Test]
        public void CreateAddViewModel_NoCondition_ReturnViewModel()
        {
            // Arrang

            var databaseTypes = new List<DatabaseType>();
            var databaseGroups = new List<DatabaseGroup>();
            var breadcrumbViewModel = new BreadCrumbViewModel();

            authorizationServiceWrapper.Setup(s => s.CheckUserHasAdminDeveloperPolicy()).Returns(true);
            breadCrumbFactory.Setup(x => x.CreateDatabaseAddModel()).Returns(breadcrumbViewModel);

            var viewModel = new DatabaseViewModel
            {
                DatabaseTypes = databaseTypes,
                DatabaseGroups = databaseGroups,
                BreadCrumbViewModel = breadcrumbViewModel,
                IsAuthorized = true
            };

            // Act
            var result = factory.CreateAddViewModel(databaseTypes, databaseGroups);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
        }

        [Test]
        public void CreateEditDatabase_NoCondition_ReturnViewModel()
        {
            // Arrang

            var database = new Database { Id = 3 };
            var databaseTypes = new List<DatabaseType>();
            var databaseGroups = new List<DatabaseGroup>();
            var breadcrumbViewModel = new BreadCrumbViewModel();

            breadCrumbFactory.Setup(x => x.CreateDatabaseEditModel(database.Id)).Returns(breadcrumbViewModel);

            var viewModel = new EditDatabaseViewModel
            {
                DatabaseGroups = databaseGroups,
                DatabaseTypes = databaseTypes,
                Database = database,
                BreadCrumbViewModel = breadcrumbViewModel
            };

            // Act
            var result = factory.CreateEditDatabase(database, databaseGroups, databaseTypes);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
        }
    }
}