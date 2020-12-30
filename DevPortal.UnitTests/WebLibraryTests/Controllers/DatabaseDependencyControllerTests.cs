using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.UnitTests.TestHelpers;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Controllers;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.WebLibraryTests.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class DatabaseDependencyControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IDatabaseDependencyService> databaseDependencyService;

        StrictMock<IDatabaseDependencyViewModelFactory> viewModelFactory;

        StrictMock<IDatabaseReaderService> databaseReaderService;

        StrictMock<IUrlGeneratorService> urlGeneratorService;

        StrictMock<IRouteValueFactory> routeValueFactory;

        DatabaseDependencyController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            databaseDependencyService = new StrictMock<IDatabaseDependencyService>();
            viewModelFactory = new StrictMock<IDatabaseDependencyViewModelFactory>();
            databaseReaderService = new StrictMock<IDatabaseReaderService>();
            urlGeneratorService = new StrictMock<IUrlGeneratorService>();
            routeValueFactory = new StrictMock<IRouteValueFactory>();

            controller = new DatabaseDependencyController(
                userSessionService.Object,
                databaseDependencyService.Object,
                viewModelFactory.Object,
                databaseReaderService.Object,
                urlGeneratorService.Object,
                routeValueFactory.Object
                );
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            databaseDependencyService.VerifyAll();
            viewModelFactory.VerifyAll();
            userSessionService.VerifyAll();
            databaseReaderService.VerifyAll();
            urlGeneratorService.VerifyAll();
        }

        #endregion

        #region add- get

        [Test]
        public void Add_NoCondition_ReturnDatabaseDependencyViewModel()
        {
            //Arrange
            var viewModel = new DatabaseDependencyViewModel();
            var databaseGroups = new List<DatabaseGroup>();
            var databases = new List<Database>();
            var databaseDependency = new DatabaseDependency();
            var applicationId = 1;

            databaseReaderService.Setup(x => x.GetDatabaseGroups()).Returns(databaseGroups);
            databaseReaderService.Setup(x => x.GetDatabases()).Returns(databases);
            viewModelFactory.Setup(x => x.CreatDatabaseDependencyViewModelAddView(applicationId, databaseGroups, databases)).Returns(viewModel);

            //Act
            var result = controller.Add(applicationId);

            //Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<DatabaseDependencyViewModel>().Should().Be(viewModel);
            databaseDependencyService.Verify(x => x.AddDatabaseDependency(databaseDependency), Times.Never);
        }

        #endregion

        #region add - post

        [Test]
        public void Add_InvalidModel_ReturnDatabaseDependencyViewModel()
        {
            //Arrange
            var viewModel = new DatabaseDependencyViewModel();
            var databaseGroups = new List<DatabaseGroup>();
            var databases = new List<Database>();
            var applicationId = 1;
            var databaseDependency = new DatabaseDependency { ApplicationId = applicationId };

            controller.ModelState.AddModelError("", "invalid model");
            databaseReaderService.Setup(x => x.GetDatabaseGroups()).Returns(databaseGroups);
            databaseReaderService.Setup(x => x.GetDatabases()).Returns(databases);
            viewModelFactory.Setup(x => x.CreatDatabaseDependencyViewModelAddView(applicationId, databaseGroups, databases)).Returns(viewModel);

            //Act
            var result = controller.Add(databaseDependency);

            //Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<DatabaseDependencyViewModel>().Should().Be(viewModel);
            databaseDependencyService.Verify(x => x.AddDatabaseDependency(databaseDependency), Times.Never);
        }

        [Test]
        public void Add_AddFails_ReturnDatabaseDependencyViewModel()
        {
            //Arrange
            var viewModel = new DatabaseDependencyViewModel();
            var databaseGroups = new List<DatabaseGroup>();
            var databases = new List<Database>();
            var applicationId = 1;
            var databaseDependency = new DatabaseDependency { ApplicationId = applicationId };
            var addResult = ServiceResult.Error("Error");
            var userId = 1;

            databaseDependencyService.Setup(x => x.AddDatabaseDependency(databaseDependency)).Returns(addResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            databaseReaderService.Setup(x => x.GetDatabaseGroups()).Returns(databaseGroups);
            databaseReaderService.Setup(x => x.GetDatabases()).Returns(databases);
            viewModelFactory.Setup(x => x.CreatDatabaseDependencyViewModelAddView(applicationId, databaseGroups, databases)).Returns(viewModel);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, addResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Add(databaseDependency);

            //Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<DatabaseDependencyViewModel>().Should().Be(viewModel);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        [Test]
        public void Add_AddSuccess_ReturnDatabaseDependencyViewModel()
        {
            //Arrange
            var applicationId = 1;
            var databaseDependency = new DatabaseDependency { ApplicationId = applicationId };
            var addResult = ServiceResult.Success("Success");
            var userId = 1;
            var redirectUrl = $"application/detail{applicationId}";

            databaseDependencyService.Setup(x => x.AddDatabaseDependency(databaseDependency)).Returns(addResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            urlGeneratorService.Setup(x => x.GenerateUrl(ApplicationControllerActionNames.Detail, ControllerNames.Application, applicationId)).Returns(redirectUrl);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, addResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Add(databaseDependency);

            //Assert
            result.Should().BeRedirectResult().WithUrl(redirectUrl);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        #endregion

        #region edit

        [Test]
        public void Edit_DatabaseDependencyNotFound_ReturnApplicationListView()
        {
            //Arrange
            var id = 1;
            DatabaseDependency databaseDependency = null;

            databaseDependencyService.Setup(s => s.GetDatabaseDependency(id)).Returns(databaseDependency);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.DatabaseDependencyNotFound);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Edit(id);

            //Assert
            result.Should().BeRedirectToActionResult().WithActionName(ApplicationControllerActionNames.Index).WithControllerName(ControllerNames.Application);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Edit_DatabaseDependencyExists_ReturnDatabaseDependencyViewModel()
        {
            //Arrange
            var id = 1;
            var viewModel = new DatabaseDependencyViewModel();
            var databaseDependency = new DatabaseDependency();

            databaseDependencyService.Setup(s => s.GetDatabaseDependency(id)).Returns(databaseDependency);
            viewModelFactory.Setup(x => x.CreateDatabaseDependencyEditViewModel(databaseDependency)).Returns(viewModel);

            //Act
            var result = controller.Edit(id);

            //Assert
            result.Should().BeViewResult(ViewNames.Edit).ModelAs<DatabaseDependencyViewModel>().Should().Be(viewModel);
        }

        [Test]
        public void Edit_InvalidModel_ReturnDatabaseDependencyViewModel()
        {
            //Arrange
            var databaseDependency = new DatabaseDependency();
            var databaseDependencyViewModel = new DatabaseDependencyViewModel
            {
                DatabaseDependency = databaseDependency
            };
            var viewModel = new DatabaseDependencyViewModel();

            controller.ModelState.AddModelError("", "invalid model");

            viewModelFactory.Setup(x => x.CreateDatabaseDependencyEditViewModel(databaseDependency)).Returns(viewModel);

            //Act
            var result = controller.Edit(databaseDependencyViewModel);

            //Assert
            result.Should().BeViewResult(ViewNames.Edit).ModelAs<DatabaseDependencyViewModel>().Should().Be(viewModel);
            databaseDependencyService.Verify(x => x.UpdateDatabaseDependency(databaseDependency), Times.Never);
        }

        [Test]
        public void Edit_UpdateFails_ReturnDatabaseDependencyViewModel()
        {
            //Arrange
            var databaseDependency = new DatabaseDependency { Id = 1 };
            var databaseDependencyViewModel = new DatabaseDependencyViewModel
            {
                DatabaseDependency = databaseDependency
            };
            var viewModel = new DatabaseDependencyViewModel();
            var serviceResult = ServiceResult.Error("Error");
            var userId = 1;

            databaseDependencyService.Setup(x => x.UpdateDatabaseDependency(databaseDependency)).Returns(serviceResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            databaseDependencyService.Setup(s => s.GetDatabaseDependency(databaseDependency.Id)).Returns(databaseDependency);
            viewModelFactory.Setup(x => x.CreateDatabaseDependencyEditViewModel(databaseDependency)).Returns(viewModel);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Edit(databaseDependencyViewModel);

            //Assert
            result.Should().BeViewResult(ViewNames.Edit).ModelAs<DatabaseDependencyViewModel>().Should().Be(viewModel);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        [Test]
        public void Edit_UpdateSuccess_ReturnDatabaseDependencyViewModel()
        {
            //Arrange
            var databaseDependency = new DatabaseDependency { Id = 1 };
            var databaseDependencyViewModel = new DatabaseDependencyViewModel
            {
                DatabaseDependency = databaseDependency
            };
            var serviceResult = ServiceResult.Success("Success");
            var userId = 1;
            var redirectUrl = $"databasedependency/detail{databaseDependency.Id}";

            databaseDependencyService.Setup(x => x.UpdateDatabaseDependency(databaseDependency)).Returns(serviceResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            urlGeneratorService.Setup(x => x.GenerateUrl(DatabaseDependencyControllerActionNames.Detail, ControllerNames.DatabaseDependency, databaseDependency.Id)).Returns(redirectUrl);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Edit(databaseDependencyViewModel);

            //Assert
            result.Should().BeRedirectResult().WithUrl(redirectUrl);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        #endregion

        #region detail

        [Test]
        public void Detail_DatabaseDependencyIdLessThanZero_ReturnDatabaseDependencyNotFound()
        {
            //Arrange
            var id = -1;

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.DatabaseDependencyNotFound);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Detail(id);

            //Assert
            result.Should().BeRedirectToActionResult(ApplicationControllerActionNames.Index).WithControllerName(ControllerNames.Application);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Detail_DatabaseDependencyNotExists_ReturnDatabaseDependencyNotFound()
        {
            //Arrange
            var id = 1;
            DatabaseDependency databaseDependency = null;

            databaseDependencyService.Setup(x => x.GetDatabaseDependency(id)).Returns(databaseDependency);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.DatabaseDependencyNotFound);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Detail(id);

            //Assert
            result.Should().BeRedirectToActionResult(ApplicationControllerActionNames.Index).WithControllerName(ControllerNames.Application);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Detail_DatabaseDependencyExists_ReturnDatabaseDependencyViewModel()
        {
            //Arrange
            var id = 1;
            var databaseDependency = new DatabaseDependency();
            var viewModel = new DatabaseDependencyViewModel();

            databaseDependencyService.Setup(x => x.GetDatabaseDependency(id)).Returns(databaseDependency);
            viewModelFactory.Setup(x => x.CreateDatabaseDependencyViewModel(databaseDependency)).Returns(viewModel);

            //Act
            var result = controller.Detail(id);

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Detail).ModelAs<DatabaseDependencyViewModel>().Should().Be(viewModel);
        }

        #endregion

        #region get databases by database group id

        [Test]
        public void GetDatabasesByDatabaseGroupId_NoCondition_ReturnJsonResult()
        {
            // Arrange
            var databaseGroupId = 28;
            var databases = new List<Database>();

            databaseReaderService.Setup(x => x.GetDatabasesByDatabaseGroupId(databaseGroupId)).Returns(databases);

            // Act
            var result = controller.GetDatabasesByDatabaseGroupId(databaseGroupId);

            // Assert
            result.Should().BeOfType<JsonResult>().Which.Value.As<List<Database>>().Should().BeSameAs(databases);
        }

        #endregion

        #region helpers

        public void SetResultMessageTempData(Dictionary<string, string> tempDataKeyValuePairs)
        {
            controller.TempData = SetupHelpers.CreateResultMessageTempData(tempDataKeyValuePairs);
        }

        #endregion
    }
}