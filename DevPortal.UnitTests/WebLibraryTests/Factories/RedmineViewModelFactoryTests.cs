using AB.Framework.UnitTests;
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
    public class RedmineViewModelFactoryTests : BaseTestFixture
    {
        #region members & setup

        RedmineViewModelFactory factory;

        StrictMock<IBreadCrumbFactory> breadCrumbFactory;

        [SetUp]
        public void Initialize()
        {
            breadCrumbFactory = new StrictMock<IBreadCrumbFactory>();

            factory = new RedmineViewModelFactory(breadCrumbFactory.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            breadCrumbFactory.VerifyAll();
        }

        #endregion

        [Test]
        public void CreateRedmineProjectsViewModel_NoCondition_ReturnViewModel()
        {
            // Arrange

            var applicationGroups = new List<ApplicationGroup>();
            var breadcrumbViewModel = new BreadCrumbViewModel();

            breadCrumbFactory.Setup(x => x.CreateApplicationRedmineProjectsModel()).Returns(breadcrumbViewModel);

            var viewModel = new ApplicationRedmineProjectsViewModel
            {
                ApplicationGroups = applicationGroups,
                BreadCrumbViewModel = breadcrumbViewModel
            };

            // Act
            var result = factory.CreateApplicationRedmineProjectsViewModel(applicationGroups);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
        }

        [Test]
        public void CreateRedmineProjectListModel_NoCondition_ReturnViewModel()
        {
            // Arrange

            var projects = new List<RedmineProject>();

            var redmineProjectListModel = new RedmineProjectListModel
            {
                data = projects
            };

            // Act
            var result = factory.CreateRedmineProjectListModel(projects);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(redmineProjectListModel);
        }

        [Test]
        public void CreateDatabaseRedmineProjectsViewModel_NoCondition_ReturnViewModel()
        {
            // Arrange

            var databaseGroups = new List<DatabaseGroup>();
            var breadcrumbViewModel = new BreadCrumbViewModel();

            breadCrumbFactory.Setup(x => x.CreateDatabaseRedmineProjectsModel()).Returns(breadcrumbViewModel);

            var viewModel = new DatabaseRedmineProjectsViewModel
            {
                DatabaseGroups = databaseGroups,
                BreadCrumbViewModel = breadcrumbViewModel
            };

            // Act
            var result = factory.CreateDatabaseRedmineProjectsViewModel(databaseGroups);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
        }
    }
}