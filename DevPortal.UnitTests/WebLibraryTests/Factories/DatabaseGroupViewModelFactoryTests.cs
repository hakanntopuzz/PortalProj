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
    public class DatabaseGroupViewModelFactoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IBreadCrumbFactory> breadCrumbFactory;

        StrictMock<IAuthorizationServiceWrapper> authorizationServiceWrapper;

        DatabaseGroupViewModelFactory factory;

        [SetUp]
        public void Initialize()
        {
            breadCrumbFactory = new StrictMock<IBreadCrumbFactory>();
            authorizationServiceWrapper = new StrictMock<IAuthorizationServiceWrapper>();

            factory = new DatabaseGroupViewModelFactory(
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

        #region create database group view model

        [Test]
        public void CreateDatabaseGroupViewModel_NoCondition_ReturnDatabaseGroupViewModel()
        {
            // Arrange
            var databaseGroups = new List<DatabaseGroup>();
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var groupListViewModel = new DatabaseGroupViewModel
            {
                DatabaseGroups = databaseGroups,
                BreadCrumbViewModel = breadcrumbViewModel,
                IsAuthorized = true
            };

            authorizationServiceWrapper.Setup(s => s.CheckUserHasAdminDeveloperPolicy()).Returns(true);
            breadCrumbFactory.Setup(x => x.CreateDatabaseGroupsModel()).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateDatabaseGroupViewModel(databaseGroups);

            // Assert
            result.Should().BeEquivalentTo(groupListViewModel);
        }

        #endregion

        #region create database group add view

        [Test]
        public void CreateDatabaseGroupAddView_NoCondition_ReturnDatabaseGroupViewModel()
        {
            // Arrange
            var databaseGroups = new List<DatabaseGroup>();
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var expectedResult = new DatabaseGroupViewModel
            {
                BreadCrumbViewModel = breadcrumbViewModel,
            };

            breadCrumbFactory.Setup(x => x.CreateDatabaseGroupAddModel()).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateDatabaseGroupAddView();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region create database group detail view model

        [Test]
        public void CreateDatabaseGroupDetailViewModel_NoCondition_ReturnDatabaseGroupViewModel()
        {
            // Arrange
            var databaseGroup = new DatabaseGroup();
            var databases = new List<Database>();
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var expectedResult = new DatabaseGroupViewModel
            {
                DatabaseGroup = databaseGroup,
                Databases = databases,
                IsAuthorized = true,
                BreadCrumbViewModel = breadcrumbViewModel
            };

            authorizationServiceWrapper.Setup(s => s.CheckUserHasAdminDeveloperPolicy()).Returns(true);
            breadCrumbFactory.Setup(x => x.CreateDatabaseGroupDetailModel(databaseGroup.Id)).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateDatabaseGroupDetailViewModel(databaseGroup, databases);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region create database group edit view model

        [Test]
        public void CreateDatabaseGroupEditViewModel_NoCondition_ReturnDatabaseGroupViewModel()
        {
            // Arrange
            var databaseGroup = new DatabaseGroup();
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var expectedResult = new DatabaseGroupViewModel
            {
                DatabaseGroup = databaseGroup,
                BreadCrumbViewModel = breadcrumbViewModel
            };

            breadCrumbFactory.Setup(x => x.CreateDatabaseGroupEditModel(databaseGroup.Id)).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateDatabaseGroupEditViewModel(databaseGroup);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion
    }
}