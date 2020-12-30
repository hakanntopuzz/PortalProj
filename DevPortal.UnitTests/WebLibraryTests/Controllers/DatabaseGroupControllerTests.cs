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
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.WebLibraryTests.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class DatabaseGroupControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IDatabaseGroupService> databaseGroupService;

        StrictMock<IDatabaseReaderService> databaseReaderService;

        StrictMock<IDatabaseGroupViewModelFactory> databaseGroupViewModelFactory;

        DatabaseGroupController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            databaseGroupService = new StrictMock<IDatabaseGroupService>();
            databaseReaderService = new StrictMock<IDatabaseReaderService>();
            databaseGroupViewModelFactory = new StrictMock<IDatabaseGroupViewModelFactory>();

            controller = new DatabaseGroupController(
                userSessionService.Object,
                databaseGroupService.Object,
                databaseReaderService.Object,
                databaseGroupViewModelFactory.Object
                );
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            databaseGroupService.VerifyAll();
            userSessionService.VerifyAll();
            databaseGroupViewModelFactory.VerifyAll();
            databaseReaderService.VerifyAll();
        }

        #endregion

        #region index

        [Test]
        public void Index_NoCondition_ReturnDatabaseGroupViewModel()
        {
            //Arrange
            var databaseGroups = new List<DatabaseGroup>();
            var viewModel = new DatabaseGroupViewModel();

            databaseGroupService.Setup(x => x.GetDatabaseGroups()).Returns(databaseGroups);
            databaseGroupViewModelFactory.Setup(x => x.CreateDatabaseGroupViewModel(databaseGroups)).Returns(viewModel);

            //Act
            var result = controller.Index();

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Index).ModelAs<DatabaseGroupViewModel>().Should().Be(viewModel);
        }

        #endregion

        #region add- get

        [Test]
        public void Add_NoCondition_ReturnDatabaseGroupViewModel()
        {
            //Arrange
            var viewModel = new DatabaseGroupViewModel();
            var databaseGroup = new DatabaseGroup();

            databaseGroupViewModelFactory.Setup(x => x.CreateDatabaseGroupAddView()).Returns(viewModel);

            //Act
            var result = controller.Add();

            //Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<DatabaseGroupViewModel>().Should().Be(viewModel);
            databaseGroupService.Verify(x => x.AddDatabaseGroup(databaseGroup), Times.Never);
        }

        #endregion

        #region add - post

        [Test]
        public void Add_InvalidModel_ReturnDatabaseGroupViewModel()
        {
            //Arrange
            var viewModel = new DatabaseGroupViewModel();
            var databaseGroup = new DatabaseGroup();

            controller.ModelState.AddModelError("", "invalid model");
            databaseGroupViewModelFactory.Setup(x => x.CreateDatabaseGroupAddView()).Returns(viewModel);

            //Act
            var result = controller.Add(databaseGroup);

            //Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<DatabaseGroupViewModel>().Should().Be(viewModel);
            databaseGroupService.Verify(x => x.AddDatabaseGroup(databaseGroup), Times.Never);
        }

        [Test]
        public void Add_AddFails_ReturnDatabaseGroupViewModel()
        {
            //Arrange
            var viewModel = new DatabaseGroupViewModel();
            var databaseGroup = new DatabaseGroup();
            var addResult = ServiceResult.Error("Error");
            var userId = 1;

            databaseGroupService.Setup(x => x.AddDatabaseGroup(databaseGroup)).Returns(addResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            databaseGroupViewModelFactory.Setup(x => x.CreateDatabaseGroupAddView()).Returns(viewModel);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, addResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Add(databaseGroup);

            //Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<DatabaseGroupViewModel>().Should().Be(viewModel);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        [Test]
        public void Add_AddSuccess_ReturnDatabaseGroupViewModel()
        {
            //Arrange
            var databaseGroup = new DatabaseGroup();
            var addResult = ServiceResult.Success("Success");
            var userId = 1;

            databaseGroupService.Setup(x => x.AddDatabaseGroup(databaseGroup)).Returns(addResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, addResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Add(databaseGroup);

            //Assert
            result.Should().BeRedirectToActionResult().WithActionName(DatabaseGroupControllerActionNames.Index).WithControllerName(ControllerNames.DatabaseGroup);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        #endregion

        #region detail

        [Test]
        public void Detail_DatabaseGroupExists_ReturnDatabaseGroupNotFound()
        {
            //Arrange
            var id = 1;
            DatabaseGroup databaseGroup = null;

            databaseGroupService.Setup(x => x.GetDatabaseGroup(id)).Returns(databaseGroup);

            var serviceResult = ServiceResult.Error(Messages.DatabaseGroupNotFound);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Detail(id);

            //Assert
            result.Should().BeRedirectToActionResult(DatabaseGroupControllerActionNames.Index).WithControllerName(ControllerNames.DatabaseGroup);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Detail_DatabaseGroupExists_ReturnDatabaseGroupViewModel()
        {
            //Arrange
            var id = 1;
            var databaseGroup = new DatabaseGroup();
            var database = new List<Database>();
            var viewModel = new DatabaseGroupViewModel();

            databaseGroupService.Setup(x => x.GetDatabaseGroup(id)).Returns(databaseGroup);
            databaseReaderService.Setup(x => x.GetDatabasesByDatabaseGroupId(id)).Returns(database);
            databaseGroupViewModelFactory.Setup(x => x.CreateDatabaseGroupDetailViewModel(databaseGroup, database)).Returns(viewModel);

            //Act
            var result = controller.Detail(id);

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Detail).ModelAs<DatabaseGroupViewModel>().Should().Be(viewModel);
        }

        #endregion

        #region edit - get

        [Test]
        public void Edit_DatabaseGroupNotFound_ReturnDatabaseGroupListView()
        {
            //Arrange
            var id = 1;
            DatabaseGroup databaseGroup = null;

            databaseGroupService.Setup(s => s.GetDatabaseGroup(id)).Returns(databaseGroup);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.DatabaseGroupNotFound);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Edit(id);

            //Assert
            result.Should().BeRedirectToActionResult().WithActionName(DatabaseGroupControllerActionNames.Index).WithControllerName(ControllerNames.DatabaseGroup);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Edit_DatabaseGroupExists_ReturnDatabaseGroupViewModel()
        {
            //Arrange
            var id = 1;
            var viewModel = new DatabaseGroupViewModel();
            var databaseGroup = new DatabaseGroup();

            databaseGroupService.Setup(s => s.GetDatabaseGroup(id)).Returns(databaseGroup);
            databaseGroupViewModelFactory.Setup(x => x.CreateDatabaseGroupEditViewModel(databaseGroup)).Returns(viewModel);

            //Act
            var result = controller.Edit(id);

            //Assert
            result.Should().BeViewResult(ViewNames.Edit).ModelAs<DatabaseGroupViewModel>().Should().Be(viewModel);
            databaseGroupService.Verify(x => x.UpdateDatabaseGroup(databaseGroup), Times.Never);
        }

        #endregion

        #region edit - post

        [Test]
        public void Edit_InvalidModel_ReturnDatabaseGroupViewModel()
        {
            //Arrange
            var viewModel = new DatabaseGroupViewModel();
            var databaseGroup = new DatabaseGroup();

            controller.ModelState.AddModelError("", "invalid model");
            databaseGroupViewModelFactory.Setup(x => x.CreateDatabaseGroupEditViewModel(databaseGroup)).Returns(viewModel);

            //Act
            var result = controller.Edit(databaseGroup);

            //Assert
            result.Should().BeViewResult(ViewNames.Edit).ModelAs<DatabaseGroupViewModel>().Should().Be(viewModel);
            databaseGroupService.Verify(x => x.UpdateDatabaseGroup(databaseGroup), Times.Never);
        }

        [Test]
        public void Edit_UpdateFails_ReturnDatabaseGroupViewModel()
        {
            //Arrange
            var databaseGroup = new DatabaseGroup();
            var viewModel = new DatabaseGroupViewModel();

            var serviceResult = ServiceResult.Error("Error");
            var userId = 1;

            databaseGroupService.Setup(x => x.UpdateDatabaseGroup(databaseGroup)).Returns(serviceResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            databaseGroupService.Setup(s => s.GetDatabaseGroup(databaseGroup.Id)).Returns(databaseGroup);
            databaseGroupViewModelFactory.Setup(x => x.CreateDatabaseGroupEditViewModel(databaseGroup)).Returns(viewModel);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Edit(databaseGroup);

            //Assert
            result.Should().BeViewResult(ViewNames.Edit).ModelAs<DatabaseGroupViewModel>().Should().Be(viewModel);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        [Test]
        public void Edit_UpdateSuccess_ReturnDatabaseGroupDetailView()
        {
            //Arrange
            var databaseGroup = new DatabaseGroup();
            var viewModel = new DatabaseGroupViewModel();

            var serviceResult = ServiceResult.Success("Success");
            var userId = 1;

            databaseGroupService.Setup(x => x.UpdateDatabaseGroup(databaseGroup)).Returns(serviceResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Edit(databaseGroup);

            //Assert
            result.Should().BeRedirectToActionResult().WithActionName(DatabaseGroupControllerActionNames.Detail).WithControllerName(ControllerNames.DatabaseGroup)
                .WithRouteValue("id", databaseGroup.Id);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        #endregion

        #region delete

        [Test]
        public void Delete_Error_ReturnErrorResult()
        {
            //Arrange
            var id = 1;
            var redirectUrl = $"/databasegroup/index";
            var deleteResult = RedirectableClientActionResult.Error(redirectUrl, Messages.DeleteFails);

            databaseGroupService.Setup(x => x.DeleteDatabaseGroup(id)).Returns(deleteResult);

            //Act
            var result = controller.Delete(id);

            //Assert
            var expectedResult = RedirectableClientActionResult.Error(deleteResult.RedirectUrl, Messages.DeleteFails);
            AssertHelpers.AssertRedirectableClientActionResult(result, expectedResult);
        }

        [Test]
        public void Delete_Success_ReturnSuccessAndRedirectUrl()
        {
            //Arrange
            var id = 1;
            var redirectUrl = $"/databasegroup/index";
            var deleteResult = RedirectableClientActionResult.Success(redirectUrl, Messages.DatabaseGroupDeleted);

            databaseGroupService.Setup(x => x.DeleteDatabaseGroup(id)).Returns(deleteResult);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, Messages.DatabaseGroupDeleted);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Delete(id);

            //Assert
            var expectedResult = RedirectableClientActionResult.Success(deleteResult.RedirectUrl, Messages.DatabaseGroupDeleted);
            AssertHelpers.AssertRedirectableClientActionResult(result, expectedResult);
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