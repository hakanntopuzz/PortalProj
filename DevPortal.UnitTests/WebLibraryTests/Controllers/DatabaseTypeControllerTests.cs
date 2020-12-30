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
    public class DatabaseTypeControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IDatabaseTypeService> databaseTypeService;

        StrictMock<IDatabaseReaderService> databaseReaderService;

        StrictMock<IDatabaseTypeViewModelFactory> databasetypeViewModelFactory;

        DatabaseTypeController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            databaseTypeService = new StrictMock<IDatabaseTypeService>();
            databaseReaderService = new StrictMock<IDatabaseReaderService>();
            databasetypeViewModelFactory = new StrictMock<IDatabaseTypeViewModelFactory>();

            controller = new DatabaseTypeController(
                userSessionService.Object,
                databaseTypeService.Object,
                databaseReaderService.Object,
                databasetypeViewModelFactory.Object
                );
        }

        #endregion

        #region verify mock

        protected override void VerifyMocks()
        {
            databaseTypeService.VerifyAll();
            databaseReaderService.VerifyAll();
            userSessionService.VerifyAll();
            databasetypeViewModelFactory.VerifyAll();
        }

        #endregion

        #region index

        [Test]
        public void Index_NoCondition_ReturnDatabaseTypeViewModel()
        {
            //Arrange
            var databaseTypes = new List<DatabaseType>();
            var viewModel = new DatabaseTypeViewModel();

            databaseTypeService.Setup(x => x.GetDatabaseTypes()).Returns(databaseTypes);
            databasetypeViewModelFactory.Setup(x => x.CreateDatabaseTypeViewModel(databaseTypes)).Returns(viewModel);

            //Act
            var result = controller.Index();

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Index).ModelAs<DatabaseTypeViewModel>().Should().Be(viewModel);
        }

        #endregion

        #region add- get

        [Test]
        public void Add_NoCondition_ReturnDatabaseTypeViewModel()
        {
            //Arrange
            var viewModel = new DatabaseTypeViewModel();
            var databaseType = new DatabaseType();

            databasetypeViewModelFactory.Setup(x => x.CreateDatabaseTypeAddView()).Returns(viewModel);

            //Act
            var result = controller.Add();

            //Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<DatabaseTypeViewModel>().Should().Be(viewModel);
            databaseTypeService.Verify(x => x.AddDatabaseType(databaseType), Times.Never);
        }

        #endregion

        #region add - post

        [Test]
        public void Add_InvalidModel_ReturnDatabaseTypeViewModel()
        {
            //Arrange
            var viewModel = new DatabaseTypeViewModel();
            var databaseType = new DatabaseType();

            controller.ModelState.AddModelError("", "invalid model");
            databasetypeViewModelFactory.Setup(x => x.CreateDatabaseTypeAddView()).Returns(viewModel);

            //Act
            var result = controller.Add(databaseType);

            //Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<DatabaseTypeViewModel>().Should().Be(viewModel);
            databaseTypeService.Verify(x => x.AddDatabaseType(databaseType), Times.Never);
        }

        [Test]
        public void Add_AddFails_ReturnDatabaseTypeViewModel()
        {
            //Arrange
            var viewModel = new DatabaseTypeViewModel();
            var databaseType = new DatabaseType();
            var addResult = ServiceResult.Error("Error");
            var userId = 1;

            databaseTypeService.Setup(x => x.AddDatabaseType(databaseType)).Returns(addResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            databasetypeViewModelFactory.Setup(x => x.CreateDatabaseTypeAddView()).Returns(viewModel);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, addResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Add(databaseType);

            //Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<DatabaseTypeViewModel>().Should().Be(viewModel);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        [Test]
        public void Add_AddSuccess_ReturnDatabaseTypeViewModel()
        {
            //Arrange
            var databaseType = new DatabaseType();
            var addResult = ServiceResult.Success("Success");
            var userId = 1;

            databaseTypeService.Setup(x => x.AddDatabaseType(databaseType)).Returns(addResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, addResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Add(databaseType);

            //Assert
            result.Should().BeRedirectToActionResult().WithActionName(DatabaseTypeControllerActionNames.Index).WithControllerName(ControllerNames.DatabaseType);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        #endregion

        #region edit - get

        [Test]
        public void Edit_DatabaseTypeNotFound_ReturnDatabaseTypeListView()
        {
            //Arrange
            var id = 1;
            DatabaseType databaseType = null;

            databaseTypeService.Setup(s => s.GetDatabaseType(id)).Returns(databaseType);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.DatabaseTypeNotFound);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Edit(id);

            //Assert
            result.Should().BeRedirectToActionResult().WithActionName(DatabaseTypeControllerActionNames.Index).WithControllerName(ControllerNames.DatabaseType);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Edit_DatabaseTypeExists_ReturnDatabaseTypeViewModel()
        {
            //Arrange
            var id = 1;
            var viewModel = new DatabaseTypeViewModel();
            var databaseType = new DatabaseType();

            databaseTypeService.Setup(s => s.GetDatabaseType(id)).Returns(databaseType);
            databasetypeViewModelFactory.Setup(x => x.CreateDatabaseTypeEditViewModel(databaseType)).Returns(viewModel);

            //Act
            var result = controller.Edit(id);

            //Assert
            result.Should().BeViewResult(ViewNames.Edit).ModelAs<DatabaseTypeViewModel>().Should().Be(viewModel);
            databaseTypeService.Verify(x => x.UpdateDatabaseType(databaseType), Times.Never);
        }

        #endregion

        #region edit - post

        [Test]
        public void Edit_InvalidModel_ReturnDatabaseTypeViewModel()
        {
            //Arrange
            var viewModel = new DatabaseTypeViewModel();
            var databaseType = new DatabaseType();

            controller.ModelState.AddModelError("", "invalid model");
            databasetypeViewModelFactory.Setup(x => x.CreateDatabaseTypeEditViewModel(databaseType)).Returns(viewModel);

            //Act
            var result = controller.Edit(databaseType);

            //Assert
            result.Should().BeViewResult(ViewNames.Edit).ModelAs<DatabaseTypeViewModel>().Should().Be(viewModel);
            databaseTypeService.Verify(x => x.UpdateDatabaseType(databaseType), Times.Never);
        }

        [Test]
        public void Edit_UpdateFails_ReturnDatabaseTypeViewModel()
        {
            //Arrange
            var databaseType = new DatabaseType();
            var viewModel = new DatabaseTypeViewModel();

            var serviceResult = ServiceResult.Error("Error");
            var userId = 1;

            databaseTypeService.Setup(x => x.UpdateDatabaseType(databaseType)).Returns(serviceResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            databaseTypeService.Setup(s => s.GetDatabaseType(databaseType.Id)).Returns(databaseType);
            databasetypeViewModelFactory.Setup(x => x.CreateDatabaseTypeEditViewModel(databaseType)).Returns(viewModel);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Edit(databaseType);

            //Assert
            result.Should().BeViewResult(ViewNames.Edit).ModelAs<DatabaseTypeViewModel>().Should().Be(viewModel);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        [Test]
        public void Edit_UpdateSuccess_ReturnDatabaseTypeDetailView()
        {
            //Arrange
            var databaseType = new DatabaseType();
            var viewModel = new DatabaseTypeViewModel();

            var serviceResult = ServiceResult.Success("Success");
            var userId = 1;

            databaseTypeService.Setup(x => x.UpdateDatabaseType(databaseType)).Returns(serviceResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Edit(databaseType);

            //Assert
            result.Should().BeRedirectToActionResult().WithActionName(DatabaseTypeControllerActionNames.Detail).WithControllerName(ControllerNames.DatabaseType)
                .WithRouteValue("id", databaseType.Id);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        #endregion

        #region detail

        [Test]
        public void Detail_DatabaseTypeExists_ReturnDatabaseTypeNotFound()
        {
            //Arrange
            var id = 1;
            DatabaseType databaseType = null;

            databaseTypeService.Setup(x => x.GetDatabaseType(id)).Returns(databaseType);

            var serviceResult = ServiceResult.Error(Messages.DatabaseTypeNotFound);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Detail(id);

            //Assert
            result.Should().BeRedirectToActionResult(DatabaseControllerActionNames.Index).WithControllerName(ControllerNames.DatabaseType);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Detail_DatabaseTypeExists_ReturnDatabaseTypeViewModel()
        {
            //Arrange
            var id = 1;
            var databaseType = new DatabaseType();
            var database = new List<Database>();
            var viewModel = new DatabaseTypeViewModel();

            databaseTypeService.Setup(x => x.GetDatabaseType(id)).Returns(databaseType);
            databaseReaderService.Setup(x => x.GetDatabaseByDatabaseTypeId(id)).Returns(database);
            databasetypeViewModelFactory.Setup(x => x.CreateDatabaseTypeDetailViewModel(databaseType, database)).Returns(viewModel);

            //Act
            var result = controller.Detail(id);

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Detail).ModelAs<DatabaseTypeViewModel>().Should().Be(viewModel);
        }

        #endregion

        #region delete

        [Test]
        public void Delete_Error_ReturnErrorResult()
        {
            //Arrange
            var id = 1;
            var redirectUrl = $"/databasetype/index";
            var deleteResult = RedirectableClientActionResult.Error(redirectUrl, Messages.DeleteFails);

            databaseTypeService.Setup(x => x.DeleteDatabaseType(id)).Returns(deleteResult);

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
            var redirectUrl = $"/databasetype/index";
            var deleteResult = RedirectableClientActionResult.Success(redirectUrl, Messages.DatabaseTypeDeleted);

            databaseTypeService.Setup(x => x.DeleteDatabaseType(id)).Returns(deleteResult);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, Messages.EnvironmentDeleted);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Delete(id);

            //Assert
            var expectedResult = RedirectableClientActionResult.Success(deleteResult.RedirectUrl, Messages.DatabaseTypeDeleted);
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