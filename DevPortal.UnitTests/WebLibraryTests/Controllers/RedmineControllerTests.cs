using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Controllers;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.UnitTests.WebLibraryTests.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class RedmineControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IApplicationReaderService> applicationReaderService;

        StrictMock<IRedmineViewModelFactory> redmineViewModelFactory;

        StrictMock<IDatabaseReaderService> databaseReaderService;

        StrictMock<IDatabaseGroupService> databaseGroupService;

        RedmineController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            applicationReaderService = new StrictMock<IApplicationReaderService>();
            redmineViewModelFactory = new StrictMock<IRedmineViewModelFactory>();
            databaseReaderService = new StrictMock<IDatabaseReaderService>();
            databaseGroupService = new StrictMock<IDatabaseGroupService>();

            controller = new RedmineController(
                userSessionService.Object,
                applicationReaderService.Object,
                redmineViewModelFactory.Object,
                databaseReaderService.Object,
                databaseGroupService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            applicationReaderService.VerifyAll();
        }

        #endregion

        #region index

        [Test]
        public void Index_NoCondition_ReturnView()
        {
            var applicationGroups = new List<ApplicationGroup>();
            var model = new ApplicationRedmineProjectsViewModel();

            // Arrange
            applicationReaderService.Setup(x => x.GetApplicationGroups()).Returns(applicationGroups);
            redmineViewModelFactory.Setup(x => x.CreateApplicationRedmineProjectsViewModel(applicationGroups)).Returns(model);

            // Act
            var result = controller.Index();

            // Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Index).ModelAs<ApplicationRedmineProjectsViewModel>().Should().BeEquivalentTo(model);
        }

        [Test]
        public async Task Index_NoCondition_ReturnRedmineProjectsListModel()
        {
            // Arrange
            var redmineProjectListModel = new RedmineProjectListModel();
            var projects = new List<RedmineProject>();
            var tableParam = new RedmineTableParam();

            applicationReaderService.Setup(x => x.GetFilteredApplicationRedmineProjectListAsync(tableParam)).ReturnsAsync(projects);
            redmineViewModelFactory.Setup(x => x.CreateRedmineProjectListModel(projects)).Returns(redmineProjectListModel);

            // Act
            var result = await controller.Index(tableParam);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.As<RedmineProjectListModel>().Should().Be(redmineProjectListModel);
        }

        #endregion

        #region database projects

        [Test]
        public void DatabaseProjects_NoCondition_ReturnView()
        {
            var databaseGroups = new List<DatabaseGroup>();
            var model = new DatabaseRedmineProjectsViewModel();

            // Arrange
            databaseGroupService.Setup(x => x.GetDatabaseGroups()).Returns(databaseGroups);
            redmineViewModelFactory.Setup(x => x.CreateDatabaseRedmineProjectsViewModel(databaseGroups)).Returns(model);

            // Act
            var result = controller.DatabaseProjects();

            // Assert
            result.Should().BeViewResult().WithViewName(ViewNames.DatabaseProjects).ModelAs<DatabaseRedmineProjectsViewModel>().Should().BeEquivalentTo(model);
        }

        [Test]
        public async Task DatabaseProjects_NoCondition_ReturnRedmineProjectsListModel()
        {
            // Arrange
            var redmineProjectListModel = new RedmineProjectListModel();
            var projects = new List<RedmineProject>();
            var tableParam = new DatabaseRedmineProjectTableParam();

            databaseReaderService.Setup(x => x.GetFilteredDatabaseRedmineProjectListAsync(tableParam)).ReturnsAsync(projects);
            redmineViewModelFactory.Setup(x => x.CreateRedmineProjectListModel(projects)).Returns(redmineProjectListModel);

            // Act
            var result = await controller.DatabaseProjects(tableParam);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.As<RedmineProjectListModel>().Should().Be(redmineProjectListModel);
        }

        #endregion
    }
}