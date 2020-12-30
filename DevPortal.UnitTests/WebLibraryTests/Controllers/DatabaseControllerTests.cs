using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.UnitTests.TestHelpers;
using DevPortal.Web.Library.Abstract.Factories;
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
    public class DatabaseControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IDatabaseWriterService> databaseWriterService;

        StrictMock<IDatabaseReaderService> databaseReaderService;

        StrictMock<IDatabaseViewModelFactory> viewModelFactory;

        StrictMock<IDatabaseTypeService> databaseTypeService;

        StrictMock<IUrlGeneratorService> urlHelper;

        DatabaseController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            databaseWriterService = new StrictMock<IDatabaseWriterService>();
            databaseReaderService = new StrictMock<IDatabaseReaderService>();
            viewModelFactory = new StrictMock<IDatabaseViewModelFactory>();
            databaseTypeService = new StrictMock<IDatabaseTypeService>();
            urlHelper = new StrictMock<IUrlGeneratorService>();

            controller = new DatabaseController(
                userSessionService.Object,
                databaseWriterService.Object,
                databaseReaderService.Object,
                viewModelFactory.Object,
                databaseTypeService.Object,
                urlHelper.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            databaseWriterService.VerifyAll();
            databaseReaderService.VerifyAll();
            userSessionService.VerifyAll();
            viewModelFactory.VerifyAll();
            databaseTypeService.VerifyAll();
            urlHelper.VerifyAll();
        }

        #endregion

        #region helpers

        public void SetResultMessageTempData(Dictionary<string, string> tempDataKeyValuePairs)
        {
            controller.TempData = SetupHelpers.CreateResultMessageTempData(tempDataKeyValuePairs);
        }

        #endregion

        #region index

        [Test]
        public void Index_NoCondition_ReturnDatabasesViewModel()
        {
            // Arrange
            var databaseGroups = new List<DatabaseGroup>();
            var databases = new List<Database>();
            var viewModel = new DatabasesViewModel();

            databaseReaderService.Setup(x => x.GetDatabaseGroups()).Returns(databaseGroups);
            databaseReaderService.Setup(x => x.GetDatabases()).Returns(databases);
            viewModelFactory.Setup(x => x.CreateDatabasesViewModel(databaseGroups, databases)).Returns(viewModel);

            // Act
            var result = controller.Index();

            // Assert
            result.Should().BeViewResult(ViewNames.Index).ModelAs<DatabasesViewModel>().Should().BeEquivalentTo(viewModel);
        }

        [Test]
        public async Task Index_NoCondition_ReturnDatabasesListModel()
        {
            // Arrange
            var databaseListModel = new DatabaseListModel();
            var databases = new List<Database>();
            var tableParam = new DatabaseTableParam();

            databaseReaderService.Setup(x => x.GetFilteredDatabaseListAsync(tableParam)).ReturnsAsync(databases);
            viewModelFactory.Setup(x => x.CreateDatabaseListModel(databases)).Returns(databaseListModel);

            // Act
            var result = await controller.Index(tableParam);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.As<DatabaseListModel>().Should().Be(databaseListModel);
        }

        #endregion

        #region detail

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void GetDatabase_InvalidIdParameterOnTestsCases_ReturnErrorMessageAndRedirectToDatabaseList(int id)
        {
            // Arrange
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.DatabaseNotFound);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = controller.Detail(id);

            // Assert
            AssertHelpers.AssertRedirectToAction(result, DatabaseControllerActionNames.Index, ControllerNames.Database);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
            databaseReaderService.Verify(x => x.GetDatabase(id), Times.Never);
        }

        [Test]
        public void GetDatabase_DatabaseExists_ReturnDatabase()
        {
            // Arrange
            var id = 1;
            var database = new Database();
            var viewModel = new DatabaseViewModel();

            databaseReaderService.Setup(x => x.GetDatabase(id)).Returns(database);
            viewModelFactory.Setup(x => x.CreateDatabase(database)).Returns(viewModel);

            // Act
            var result = controller.Detail(id);

            // Assert
            result.Should().BeViewResult(ViewNames.Detail).ModelAs<DatabaseViewModel>().Should().Be(viewModel);
        }

        [Test]
        public void GetDatabase_DatabaseDoesNotExist_ReturnErrorMessageAndRedirectToDatabaseList()
        {
            // Arrange
            var id = 1;
            Database database = null;

            databaseReaderService.Setup(x => x.GetDatabase(id)).Returns(database);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.DatabaseNotFound);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = controller.Detail(id);

            // Assert
            AssertHelpers.AssertRedirectToAction(result, DatabaseControllerActionNames.Index, ControllerNames.Database);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        #endregion

        #region add- get

        [Test]
        public void Add_NoCondition_ReturnDatabaseViewModel()
        {
            //Arrange
            var viewModel = new DatabaseViewModel();
            var database = new Database();
            var databaseTypes = new List<DatabaseType>();
            var databaseGroups = new List<DatabaseGroup>();

            databaseReaderService.Setup(x => x.GetDatabaseTypes()).Returns(databaseTypes);
            databaseReaderService.Setup(x => x.GetDatabaseGroups()).Returns(databaseGroups);
            viewModelFactory.Setup(x => x.CreateAddViewModel(databaseTypes, databaseGroups)).Returns(viewModel);

            //Act
            var result = controller.Add();

            //Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<DatabaseViewModel>().Should().Be(viewModel);
            databaseWriterService.Verify(x => x.AddDatabase(database), Times.Never);
        }

        #endregion

        #region add - post

        [Test]
        public void Add_InvalidModel_ReturnDatabaseViewModel()
        {
            //Arrange
            var viewModel = new DatabaseViewModel();
            var database = new Database();
            var databaseTypes = new List<DatabaseType>();
            var databaseGroups = new List<DatabaseGroup>();

            controller.ModelState.AddModelError("", "invalid model");
            databaseReaderService.Setup(x => x.GetDatabaseTypes()).Returns(databaseTypes);
            databaseReaderService.Setup(x => x.GetDatabaseGroups()).Returns(databaseGroups);
            viewModelFactory.Setup(x => x.CreateAddViewModel(databaseTypes, databaseGroups)).Returns(viewModel);

            //Act
            var result = controller.Add(database);

            //Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<DatabaseViewModel>().Should().Be(viewModel);
            databaseWriterService.Verify(x => x.AddDatabase(database), Times.Never);
        }

        [Test]
        public void Add_AddFails_ReturnDatabaseViewModel()
        {
            //Arrange
            var viewModel = new DatabaseViewModel();
            var database = new Database();
            var databaseTypes = new List<DatabaseType>();
            var databaseGroups = new List<DatabaseGroup>();
            var addResult = ServiceResult.Error("Error");
            var userId = 1;

            databaseWriterService.Setup(x => x.AddDatabase(database)).Returns(addResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            databaseReaderService.Setup(x => x.GetDatabaseTypes()).Returns(databaseTypes);
            databaseReaderService.Setup(x => x.GetDatabaseGroups()).Returns(databaseGroups);
            viewModelFactory.Setup(x => x.CreateAddViewModel(databaseTypes, databaseGroups)).Returns(viewModel);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, addResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Add(database);

            //Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<DatabaseViewModel>().Should().Be(viewModel);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        [Test]
        public void Add_AddSuccess_ReturnDatabaseViewModel()
        {
            //Arrange
            var viewModel = new DatabaseViewModel();
            var database = new Database();
            var databaseTypes = new List<DatabaseType>();
            var databaseGroups = new List<DatabaseGroup>();
            var addResult = ServiceResult.Success("Success");
            var userId = 1;

            databaseWriterService.Setup(x => x.AddDatabase(database)).Returns(addResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, addResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Add(database);

            //Assert
            result.Should().BeRedirectToActionResult().WithActionName(DatabaseControllerActionNames.Index).WithControllerName(ControllerNames.Database);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        #endregion

        #region edit

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void Edit_InvalidIdParameterOnTestsCases_ReturnErrorMessageAndRedirectToDatabaseList(int id)
        {
            // Arrange
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.DatabaseNotFound);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = controller.Edit(id);

            // Assert
            result.Should().BeRedirectToActionResult().WithActionName(DatabaseControllerActionNames.Index).WithControllerName(ControllerNames.Database);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Edit_DatabaseNotFound_ReturnBadRequest()
        {
            // Arrange
            var id = 1;
            Database database = null;

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.DatabaseNotFound);
            SetResultMessageTempData(expectedTempData);

            databaseReaderService.Setup(x => x.GetDatabase(id)).Returns(database);

            // Act
            var result = controller.Edit(id);

            // Assert
            result.Should().BeRedirectToActionResult().WithActionName(DatabaseControllerActionNames.Index).WithControllerName(ControllerNames.Database);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Edit_DatabaseFound_CreateEditViewModelAndReturnEditView()
        {
            // Arrange
            var id = 1;
            var database = new Database();
            var databaseGroups = new List<DatabaseGroup>();
            var databaseTypes = new List<DatabaseType>();
            var viewModel = new EditDatabaseViewModel();

            databaseReaderService.Setup(x => x.GetDatabase(id)).Returns(database);
            databaseReaderService.Setup(x => x.GetDatabaseGroups()).Returns(databaseGroups);
            databaseTypeService.Setup(x => x.GetDatabaseTypes()).Returns(databaseTypes);
            viewModelFactory.Setup(x => x.CreateEditDatabase(database, databaseGroups, databaseTypes)).Returns(viewModel);

            // Act
            var result = controller.Edit(id);

            // Assert
            result.Should().BeViewResult(ViewNames.Edit).ModelAs<EditDatabaseViewModel>().Should().Be(viewModel);
        }

        [Test]
        public void Edit_ModelStateIsNotValid_ReturnEditView()
        {
            // Arrange
            var database = new Database();
            var databaseGroups = new List<DatabaseGroup>();
            var databaseTypes = new List<DatabaseType>();
            var viewModel = new EditDatabaseViewModel
            {
                Database = database
            };

            databaseReaderService.Setup(x => x.GetDatabaseGroups()).Returns(databaseGroups);
            databaseTypeService.Setup(x => x.GetDatabaseTypes()).Returns(databaseTypes);
            viewModelFactory.Setup(x => x.CreateEditDatabase(database, databaseGroups, databaseTypes)).Returns(viewModel);

            controller.ModelState.AddModelError("", "invalid model");

            // Act
            var result = controller.Edit(viewModel);

            // Assert
            result.Should().BeViewResult(ViewNames.Edit).ModelAs<EditDatabaseViewModel>().Should().Be(viewModel);
        }

        [Test]
        public void Edit_ServiceReturnsError_SetErrorMessageToTempDataAndReturnEditView()
        {
            // Arrange

            var database = new Database();
            var databaseGroups = new List<DatabaseGroup>();
            var databaseTypes = new List<DatabaseType>();
            var viewModel = new EditDatabaseViewModel
            {
                Database = database
            };
            var userId = 3;
            var serviceResult = ServiceResult.Error(Messages.UpdateFails);

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            databaseWriterService.Setup(x => x.UpdateDatabase(viewModel.Database)).Returns(serviceResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            databaseReaderService.Setup(x => x.GetDatabaseGroups()).Returns(databaseGroups);
            databaseTypeService.Setup(x => x.GetDatabaseTypes()).Returns(databaseTypes);
            viewModelFactory.Setup(x => x.CreateEditDatabase(database, databaseGroups, databaseTypes)).Returns(viewModel);

            // Act
            var result = controller.Edit(viewModel);

            // Assert
            result.Should().BeViewResult(ViewNames.Edit).ModelAs<EditDatabaseViewModel>().Should().Be(viewModel);

            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        [Test]
        public void Edit_ServiceReturnsSuccess_SetSuccessMessageToTempDataAndReturnIndexView()
        {
            // Arrange
            var database = new Database();
            var viewModel = new EditDatabaseViewModel
            {
                Database = database
            };

            var serviceResult = ServiceResult.Success(Messages.UpdateSucceeds);
            var userId = 3;

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            databaseWriterService.Setup(x => x.UpdateDatabase(viewModel.Database)).Returns(serviceResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, Messages.UpdateSucceeds);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = controller.Edit(viewModel);

            // Assert
            result.Should().BeRedirectToActionResult().WithActionName(DatabaseControllerActionNames.Detail).WithRouteValue("id", viewModel.Database.Id);

            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        #endregion

        #region delete

        [Test]
        public void Delete_Error_ReturnErrorResult()
        {
            //Arrange
            var id = 1;
            var redirectUrl = $"/database/index";
            var deleteResult = ServiceResult.Error();

            databaseWriterService.Setup(x => x.DeleteDatabase(id)).Returns(deleteResult);
            urlHelper.Setup(x => x.GenerateUrl(DatabaseControllerActionNames.Index, ControllerNames.Database)).Returns(redirectUrl);

            //Act
            var result = controller.Delete(id);

            //Assert
            var expectedResult = RedirectableClientActionResult.Error(redirectUrl);
            AssertHelpers.AssertRedirectableClientActionResult(result, expectedResult);
        }

        [Test]
        public void Delete_Success_ReturnSuccessAndRedirectUrl()
        {
            //Arrange
            var id = 1;
            var redirectUrl = $"/database/index";
            var deleteResult = ServiceResult.Success();

            databaseWriterService.Setup(x => x.DeleteDatabase(id)).Returns(deleteResult);
            urlHelper.Setup(x => x.GenerateUrl(DatabaseControllerActionNames.Index, ControllerNames.Database)).Returns(redirectUrl);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, Messages.DatabaseDeleted);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Delete(id);

            //Assert
            var expectedResult = RedirectableClientActionResult.Success(redirectUrl);
            AssertHelpers.AssertRedirectableClientActionResult(result, expectedResult);
        }

        #endregion
    }
}