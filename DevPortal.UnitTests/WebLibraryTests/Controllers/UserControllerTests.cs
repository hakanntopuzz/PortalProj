using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Identity.Business.Abstract;
using DevPortal.Model;
using DevPortal.UnitTests.TestHelpers;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Controllers;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.UnitTests.WebLibraryTests.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class UserControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IUserService> userService;

        StrictMock<IUserViewModelFactory> userViewModelFactory;

        StrictMock<IIdentityUserService> identityUserService;

        StrictMock<IUrlGeneratorService> urlHelper;

        StrictMock<IGuidService> guidService;

        StrictMock<IRouteValueFactory> routeValueFactory;

        UserController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            userService = new StrictMock<IUserService>();
            userViewModelFactory = new StrictMock<IUserViewModelFactory>();
            identityUserService = new StrictMock<IIdentityUserService>();
            urlHelper = new StrictMock<IUrlGeneratorService>();
            guidService = new StrictMock<IGuidService>();
            routeValueFactory = new StrictMock<IRouteValueFactory>();

            controller = new UserController(
                userSessionService.Object,
                identityUserService.Object,
                userService.Object,
                userViewModelFactory.Object,
                urlHelper.Object,
                guidService.Object,
                routeValueFactory.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            userService.VerifyAll();
            userViewModelFactory.VerifyAll();
            identityUserService.VerifyAll();
            urlHelper.VerifyAll();
            userSessionService.VerifyAll();
            guidService.VerifyAll();
            routeValueFactory.VerifyAll();
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
        public void Index_NoCondition_ReturnView()
        {
            UserListViewModel viewModel = null;

            // Arrange
            userViewModelFactory.Setup(x => x.CreateUserListViewModel()).Returns(viewModel);

            // Act
            var result = controller.Index();

            // Assert
            result.Should().BeViewResult();
        }

        [Test]
        public async Task Index_NoCondition_ReturnViewWithJQTable()
        {
            UserSearchFilter searchFilter = null;
            var jqTable = new JQTable();

            // Arrange
            userService.Setup(x => x.GetFilteredUsersJqTableAsync(searchFilter)).ReturnsAsync(jqTable);

            // Act
            var result = await controller.Index(searchFilter);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(jqTable);
        }

        #endregion

        #region detail

        [Test]
        public async Task Detail_UserNotFound_RedirectToIndexAndSetErrorMessageTempData()
        {
            // Arrange
            var id = 86;
            User user = null;

            userService.Setup(x => x.GetUserAsync(id)).ReturnsAsync(user);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.UserNotFound);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = await controller.Detail(id);

            // Assert
            result.Should().BeRedirectToActionResult()
                .WithActionName(UserControllerActionNames.Index)
                .WithControllerName(ControllerNames.User);

            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public async Task Detail_UserExists_ReturnUserDetail()
        {
            // Arrange
            var id = 1;
            var user = new User();
            var viewModel = new UserDetailViewModel();

            userService.Setup(x => x.GetUserAsync(id)).ReturnsAsync(user);
            userViewModelFactory.Setup(x => x.CreateUserDetailViewModel(user)).Returns(viewModel);

            // Act
            var result = await controller.Detail(id);

            // Assert
            result.Should().BeViewResult(ViewNames.Detail)
                .ModelAs<UserDetailViewModel>()
                .Should().Be(viewModel);
        }

        #endregion

        #region edit

        [Test]
        public async Task Edit_UserIsNull_CreateEditViewModelAndReturnEditView()
        {
            // Arrange
            var id = 17;
            User user = null;

            userService.Setup(x => x.GetUserAsync(id)).ReturnsAsync(user);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.UserNotFound);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = await controller.Edit(id);

            // Assert
            result.Should().BeRedirectToActionResult()
                .WithActionName(UserControllerActionNames.Index)
                .WithControllerName(ControllerNames.User);

            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public async Task Edit_UserExists_ReturnUserEdit()
        {
            // Arrange
            var id = 1;
            var user = new User();
            var userStatus = new List<UserStatus>();
            var userTypes = new List<UserType>();
            var viewModel = new EditUserViewModel();

            userService.Setup(x => x.GetUserAsync(id)).ReturnsAsync(user);
            userService.Setup(x => x.GetUserStatusListAsync()).ReturnsAsync(userStatus);
            userService.Setup(x => x.GetUserTypeListAsync()).ReturnsAsync(userTypes);

            userViewModelFactory.Setup(x => x.CreateEditUserViewModel(user, userStatus, userTypes)).Returns(viewModel);

            // Act
            var result = await controller.Edit(id);

            // Assert
            result.Should().BeViewResult(ViewNames.Edit)
                .ModelAs<EditUserViewModel>()
                .Should().Be(viewModel);
        }

        [Test]
        public async Task Edit_ModelStateIsNotValid_ReturnEditView()
        {
            // Arrange
            var user = new User();
            var userStatus = new List<UserStatus>();
            List<UserType> userTypes = null;
            var viewModel = new EditUserViewModel();

            controller.ModelState.AddModelError("", "invalid model");

            userService.Setup(x => x.GetUserStatusListAsync()).ReturnsAsync(userStatus);
            userService.Setup(x => x.GetUserTypeListAsync()).ReturnsAsync(userTypes);
            userViewModelFactory.Setup(x => x.CreateEditUserViewModel(user, userStatus, userTypes)).Returns(viewModel);

            // Act
            var result = await controller.Edit(user);

            // Assert
            result.Should().BeViewResult(ViewNames.Edit).ModelAs<AddUserViewModel>().Should().Be(viewModel);
        }

        [Test]
        public async Task Edit_UpdatingFails_SetErrorMessageToTempDataAndReturnEditView()
        {
            // Arrange
            var user = new User { Id = 1 };
            var userStatus = new List<UserStatus>();
            var userTypes = new List<UserType>();
            var viewModel = new EditUserViewModel();
            var addResult = ServiceResult.Error();

            userService.Setup(x => x.GetUserStatusListAsync()).ReturnsAsync(userStatus);
            userService.Setup(x => x.GetUserTypeListAsync()).ReturnsAsync(userTypes);
            userViewModelFactory.Setup(x => x.CreateEditUserViewModel(user, userStatus, userTypes)).Returns(viewModel);
            userSessionService.Setup(s => s.GetCurrentUserId()).Returns(user.Id);
            userService.Setup(x => x.UpdateUserAsync(user)).ReturnsAsync(addResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, addResult.Message);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = await controller.Edit(user);

            // Assert
            result.Should().BeViewResult(ViewNames.Edit).ModelAs<EditUserViewModel>().Should().Be(viewModel);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        [Test]
        public async Task Edit_ServiceReturnsSuccess_SetSuccessMessageToTempDataAndReturnIndexView()
        {
            // Arrange
            var user = new User { Id = 1 };

            string redirectRouteDataKey = "id";
            int redirectRouteDataValue = 1;
            var routeData = new RouteValueDictionary {
                { redirectRouteDataKey, redirectRouteDataValue },
            };
            var addResult = ServiceResult.Success();

            userSessionService.Setup(s => s.GetCurrentUserId()).Returns(user.Id);
            userService.Setup(x => x.UpdateUserAsync(user)).ReturnsAsync(addResult);
            routeValueFactory.Setup(x => x.CreateRouteValueForId(user.Id)).Returns(routeData);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, addResult.Message);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = await controller.Edit(user);

            // Assert
            result.Should().BeRedirectToActionResult().WithActionName(UserControllerActionNames.Detail)
                .WithRouteValue(redirectRouteDataKey, redirectRouteDataValue);

            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        #endregion

        #region add

        [Test]
        public async Task Add_NoCondition_ReturnView()
        {
            AddUserViewModel viewModel = null;
            var userStatus = new List<UserStatus>();
            List<UserType> userTypes = null;

            // Arrange
            userService.Setup(s => s.GetUserStatusListAsync()).ReturnsAsync(userStatus);
            userService.Setup(s => s.GetUserTypeListAsync()).ReturnsAsync(userTypes);
            userViewModelFactory.Setup(s => s.CreateUserAddViewModel(userStatus, userTypes)).Returns(viewModel);

            // Act
            var result = await controller.Add();

            // Assert
            result.Should().BeViewResult();
        }

        [Test]
        public async Task Add_ModelStateIsNotValid_ReturnAddView()
        {
            // Arrange
            var userStatus = new List<UserStatus>();
            List<UserType> userTypes = null;

            var viewModel = new AddUserViewModel();

            controller.ModelState.AddModelError("", "invalid model");

            userService.Setup(s => s.GetUserStatusListAsync()).ReturnsAsync(userStatus);
            userService.Setup(s => s.GetUserTypeListAsync()).ReturnsAsync(userTypes);
            userViewModelFactory.Setup(s => s.CreateUserAddViewModel(userStatus, userTypes)).Returns(viewModel);

            // Act
            var result = await controller.Add(viewModel);

            // Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<AddUserViewModel>().Should().Be(viewModel);
        }

        [Test]
        public async Task Add_ServiceResultFailed_ReturnAddView()
        {
            // Arrange
            var userId = 1;
            var secureId = "SecureId";
            var errorMessage = "error";
            var userStatus = new List<UserStatus>();
            List<UserType> userTypes = null;
            var serviceResult = Int32ServiceResult.Error(errorMessage);

            var viewModel = new AddUserViewModel
            {
                User = new User(),
                Password = "h123456"
            };

            userService.Setup(s => s.GetUserStatusListAsync()).ReturnsAsync(userStatus);
            userService.Setup(s => s.GetUserTypeListAsync()).ReturnsAsync(userTypes);
            userViewModelFactory.Setup(s => s.CreateUserAddViewModel(userStatus, userTypes)).Returns(viewModel);
            userSessionService.Setup(s => s.GetCurrentUserId()).Returns(userId);
            guidService.Setup(s => s.NewGuidString()).Returns(secureId);
            identityUserService.Setup(s => s.CreateUserAsync(viewModel.User, viewModel.Password)).ReturnsAsync(serviceResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = await controller.Add(viewModel);

            // Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<AddUserViewModel>().Should().Be(viewModel);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        [Test]
        public async Task Add_ServiceResultSuccess_ReturnDetailView()
        {
            // Arrange
            var user = new User() { Id = 1, SecureId = "secureId" };
            var serviceResult = Int32ServiceResult.Success(user.Id);

            var viewModel = new AddUserViewModel
            {
                User = user,
                Password = "h123456"
            };

            userSessionService.Setup(s => s.GetCurrentUserId()).Returns(user.Id);
            guidService.Setup(s => s.NewGuidString()).Returns(user.SecureId);
            identityUserService.Setup(s => s.CreateUserAsync(viewModel.User, viewModel.Password)).ReturnsAsync(serviceResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = await controller.Add(viewModel);

            // Assert
            result.Should().BeRedirectToActionResult().WithActionName(UserControllerActionNames.Detail).WithRouteValue("id", user.Id);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        #endregion

        #region delete

        [Test]
        public void Delete_Error_ReturnErrorResult()
        {
            //Arrange
            var id = 1;
            var redirectUrl = $"/user/index";
            var deleteResult = ServiceResult.Error();

            userService.Setup(x => x.DeleteUserAsync(id)).ReturnsAsync(deleteResult);
            urlHelper.Setup(x => x.GenerateUrl(UserControllerActionNames.Index, ControllerNames.User)).Returns(redirectUrl);

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
            var redirectUrl = $"/user/index";
            var deleteResult = ServiceResult.Success(Messages.UserDeleted);

            userService.Setup(x => x.DeleteUserAsync(id)).ReturnsAsync(deleteResult);
            urlHelper.Setup(x => x.GenerateUrl(UserControllerActionNames.Index, ControllerNames.User)).Returns(redirectUrl);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, deleteResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Delete(id);

            //Assert
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
            var expectedResult = RedirectableClientActionResult.Success(redirectUrl, deleteResult.Message);
            AssertHelpers.AssertRedirectableClientActionResult(result, expectedResult);
        }

        #endregion
    }
}