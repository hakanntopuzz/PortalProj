using AB.Framework.UnitTests;
using DevPortal.Framework.Abstract;
using DevPortal.Identity.Business.Abstract;
using DevPortal.Model;
using DevPortal.UnitTests.TestHelpers;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Controllers;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DevPortal.UnitTests.WebLibraryTests.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class AccountControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IIdentityUserService> identityUserService;

        StrictMock<IUserViewModelFactory> userViewModelFactory;

        AccountController controller;

        ClaimsPrincipal claimsPrincipal;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            identityUserService = new StrictMock<IIdentityUserService>();
            userViewModelFactory = new StrictMock<IUserViewModelFactory>();

            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "example name"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
            }));

            controller = new AccountController(
                userSessionService.Object,
                identityUserService.Object,
                userViewModelFactory.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            identityUserService.VerifyAll();
            userViewModelFactory.VerifyAll();
        }

        #endregion

        #region helpers

        public void SetResultMessageTempData(Dictionary<string, string> tempDataKeyValuePairs)
        {
            controller.TempData = SetupHelpers.CreateResultMessageTempData(tempDataKeyValuePairs);
        }

        #endregion

        #region Login

        [Test]
        public void Login_NoCondition_ReturnLoginView()
        {
            // Arrange
            const string returnUrl = "ReturnUrl";
            var viewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };

            userViewModelFactory.Setup(x => x.CreateLoginViewModel(returnUrl)).Returns(viewModel);

            // Act
            var result = controller.Login(returnUrl);

            // Assert
            result.Should().BeViewResult(ViewNames.Login).ModelAs<LoginViewModel>().Should().BeEquivalentTo(viewModel);
        }

        [Test]
        public async Task Login_ModelStateIsNotValid_ReturnLoginView()
        {
            // Arrange
            var viewModel = new LoginViewModel();

            controller.ModelState.AddModelError("", "invalid model");

            // Act
            var result = await controller.Login(viewModel);

            // Assert
            result.Should().BeViewResult(ViewNames.Login)
                .ModelAs<LoginViewModel>()
                .Should().Be(viewModel);
        }

        [Test]
        public async Task Login_ServiceResultIsSuccessFalse_ReturnLoginView()
        {
            // Arrange
            var errorMessage = "errormessage";
            var viewModel = new LoginViewModel
            {
                EmailAddress = "user1@activebuilder.com",
                Password = "h123456",
                IsRemember = true
            };

            var serviceResult = ServiceResult.Error(errorMessage);
            identityUserService.Setup(s => s.LoginAsync(viewModel.EmailAddress, viewModel.Password, viewModel.IsRemember)).ReturnsAsync(serviceResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, errorMessage);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = await controller.Login(viewModel);

            // Assert
            result.Should().BeViewResult(ViewNames.Login)
                .ModelAs<LoginViewModel>()
                .Should().Be(viewModel);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        [Test]
        public async Task Login_ServiceResultIsSuccessTrueAndReturnUrlNull_ReturnRedirectToPage()
        {
            // Arrange
            var viewModel = new LoginViewModel
            {
                EmailAddress = "user1@activebuilder.com",
                Password = "h123456",
                IsRemember = true
            };

            var serviceResult = ServiceResult.Success();

            identityUserService.Setup(s => s.LoginAsync(viewModel.EmailAddress, viewModel.Password, viewModel.IsRemember)).ReturnsAsync(serviceResult);

            // Act
            var result = await controller.Login(viewModel);

            // Assert
            AssertHelpers.AssertRedirectToAction(result, HomeControllerActionNames.Index, ControllerNames.Home);
        }

        [Test]
        public async Task Login_ServiceResultIsSuccessTrue_ReturnRedirectToPage()
        {
            // Arrange

            var viewModel = new LoginViewModel
            {
                EmailAddress = "user1@activebuilder.com",
                Password = "h123456",
                ReturnUrl = "returnUrl",
                IsRemember = true
            };

            var serviceResult = ServiceResult.Success();

            identityUserService.Setup(s => s.LoginAsync(viewModel.EmailAddress, viewModel.Password, viewModel.IsRemember)).ReturnsAsync(serviceResult);

            // Act
            var result = await controller.Login(viewModel);

            // Assert
            result.Should().BeRedirectResult().WithUrl(viewModel.ReturnUrl);
        }

        #endregion

        #region LogOut

        [Test]
        public void LogOut_ServiceResultSuccess_ReturnRedirectToAction()
        {
            // Arrange
            var serviceResult = ServiceResult.Success();

            identityUserService.Setup(s => s.LogOutAsync()).ReturnsAsync(serviceResult);

            // Act
            var result = controller.LogOut();

            // Assert
            result.Result.Should().BeRedirectToActionResult().WithActionName(AccountControllerActionNames.Login);
        }

        [Test]
        public void LogOut_ServiceResultFail_ReturnRedirectToAction()
        {
            // Arrange
            var errorMessage = "errorMessage";
            var serviceResult = ServiceResult.Error(errorMessage);

            identityUserService.Setup(s => s.LogOutAsync()).ReturnsAsync(serviceResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, errorMessage);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = controller.LogOut();

            // Assert
            result.Result.Should().BeRedirectToActionResult().
                WithActionName(HomeControllerActionNames.Index).
                WithControllerName(ControllerNames.Home);

            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        #endregion

        #region Profile

        [Test]
        public async Task Profile_NoCondition_ReturnProfileView()
        {
            // Arrange
            var user = new User();
            int currentUserId = 45;

            var viewModel = new UserDetailViewModel
            {
                User = user
            };

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(currentUserId);
            identityUserService.Setup(s => s.GetUserWithUpdateInfoAsync(currentUserId)).ReturnsAsync(user);
            userViewModelFactory.Setup(x => x.CreateUserProfileViewModel(user)).Returns(viewModel);

            // Act
            var result = await controller.Profile();

            // Assert
            result.Should().BeViewResult(ViewNames.Profile).ModelAs<UserDetailViewModel>().Should().BeEquivalentTo(viewModel);
        }

        [Test]
        public async Task Profile_ModelStateIsNotValid_ReturnEditView()
        {
            // Arrange
            var viewModel = new UserDetailViewModel();

            controller.ModelState.AddModelError("", "invalid model");

            // Act
            var result = await controller.Profile(viewModel);

            // Assert
            result.Should().BeViewResult(ViewNames.Profile)
                .ModelAs<UserDetailViewModel>()
                .Should().Be(viewModel);
        }

        [Test]
        public async Task Profile_ServiceResultIsSuccessTrue_ReturnEditView()
        {
            // Arrange
            var message = "successMessage";
            int currentUserId = 45;
            var viewModel = new UserDetailViewModel
            {
                User = new User()
            };

            var serviceResult = ServiceResult.Success(message);

            identityUserService.Setup(s => s.UpdateUserAsync(viewModel.User)).ReturnsAsync(serviceResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(currentUserId);
            identityUserService.Setup(s => s.GetUserWithUpdateInfoAsync(currentUserId)).ReturnsAsync(viewModel.User);
            userViewModelFactory.Setup(s => s.CreateUserProfileViewModel(viewModel.User)).Returns(viewModel);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = await controller.Profile(viewModel);

            // Assert
            result.Should().BeViewResult(ViewNames.Profile)
                    .ModelAs<UserDetailViewModel>()
                    .Should().Be(viewModel);

            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public async Task Profile_ServiceResultIsSuccessFalse_ReturnEditView()
        {
            // Arrange
            var message = "errorMessage";
            int currentUserId = 45;
            var viewModel = new UserDetailViewModel
            {
                User = new User()
            };

            var serviceResult = ServiceResult.Error(message);

            identityUserService.Setup(s => s.UpdateUserAsync(viewModel.User)).ReturnsAsync(serviceResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(currentUserId);
            identityUserService.Setup(s => s.GetUserWithUpdateInfoAsync(currentUserId)).ReturnsAsync(viewModel.User);
            userViewModelFactory.Setup(s => s.CreateUserProfileViewModel(viewModel.User)).Returns(viewModel);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = await controller.Profile(viewModel);

            // Assert
            result.Should().BeViewResult(ViewNames.Profile)
                    .ModelAs<UserDetailViewModel>()
                    .Should().Be(viewModel);

            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        #endregion

        #region Password

        [Test]
        public void Password_NoCondition_ReturnPasswordView()
        {
            // Arrange
            var viewModel = new ChangePasswordViewModel();

            userViewModelFactory.Setup(x => x.CreateChangePasswordViewModel()).Returns(viewModel);

            // Act
            var result = controller.Password();

            // Assert
            result.Should().BeViewResult(ViewNames.Password).
                ModelAs<ChangePasswordViewModel>().
                Should().
                BeEquivalentTo(viewModel);
        }

        [Test]
        public async Task Password_ModelStateIsNotValid_ReturnPasswordView()
        {
            // Arrange
            var viewModel = new ChangePasswordViewModel();

            controller.ModelState.AddModelError("", "invalid model");

            // Act
            var result = await controller.Password(viewModel);

            // Assert
            result.Should().BeViewResult(ViewNames.Password)
                .ModelAs<ChangePasswordViewModel>()
                .Should().Be(viewModel);
        }

        [Test]
        public async Task Password_ServiceResultIsSuccessTrue_ReturnPasswordView()
        {
            // Arrange
            var message = "successMessage";
            var viewModel = new ChangePasswordViewModel
            {
                Password = "password",
                NewPassword = "newPassword"
            };

            var serviceResult = ServiceResult.Success(message);

            identityUserService.Setup(s => s.ChangePasswordAsync(viewModel.Password, viewModel.NewPassword)).ReturnsAsync(serviceResult);
            userViewModelFactory.Setup(x => x.CreateChangePasswordViewModel()).Returns(viewModel);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = await controller.Password(viewModel);

            // Assert
            result.Should().BeViewResult(ViewNames.Password)
                    .ModelAs<ChangePasswordViewModel>()
                    .Should().Be(viewModel);

            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public async Task Password_ServiceResultIsSuccessFalse_ReturnPasswordView()
        {
            // Arrange
            var message = "errorMessage";
            var viewModel = new ChangePasswordViewModel
            {
                Password = "password",
                NewPassword = "newPassword"
            };

            var serviceResult = ServiceResult.Error(message);

            identityUserService.Setup(s => s.ChangePasswordAsync(viewModel.Password, viewModel.NewPassword)).ReturnsAsync(serviceResult);
            userViewModelFactory.Setup(x => x.CreateChangePasswordViewModel()).Returns(viewModel);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = await controller.Password(viewModel);

            // Assert
            result.Should().BeViewResult(ViewNames.Password)
                    .ModelAs<ChangePasswordViewModel>()
                    .Should().Be(viewModel);

            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        #endregion

        #region ForgotPassword

        [Test]
        public void ForgotPassword_NoCondition_ReturnForgotPasswordView()
        {
            // Arrange
            var viewModel = new ForgotPasswordViewModel();

            userViewModelFactory.Setup(x => x.CreateForgotPasswordViewModel()).Returns(viewModel);

            // Act
            var result = controller.ForgotPassword();

            // Assert
            result.Should().BeViewResult(ViewNames.ForgotPassword).
                ModelAs<ForgotPasswordViewModel>().
                Should().
                BeEquivalentTo(viewModel);
        }

        [Test]
        public async Task ForgotPassword_ModelStateIsNotValid_ReturnPasswordView()
        {
            // Arrange
            var viewModel = new ForgotPasswordViewModel();

            controller.ModelState.AddModelError("", "invalid model");

            // Act
            var result = await controller.ForgotPassword(viewModel);

            // Assert
            result.Should().BeViewResult(ViewNames.ForgotPassword)
                .ModelAs<ForgotPasswordViewModel>()
                .Should().Be(viewModel);
        }

        [Test]
        public async Task ForgotPassword_ServiceResultIsSuccessFalse_ReturnForgotPasswordView()
        {
            // Arrange
            var message = "errorMessage";
            var viewModel = new ForgotPasswordViewModel
            {
                EmailAddress = "dev@activebuilder.com"
            };

            var serviceResult = ServiceResult.Error(message);

            identityUserService.Setup(s => s.ForgotPasswordAsync(viewModel.EmailAddress)).ReturnsAsync(serviceResult);
            userViewModelFactory.Setup(x => x.CreateForgotPasswordViewModel()).Returns(viewModel);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = await controller.ForgotPassword(viewModel);

            // Assert
            result.Should().BeViewResult(ViewNames.ForgotPassword)
                    .ModelAs<ForgotPasswordViewModel>()
                    .Should().Be(viewModel);

            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public async Task ForgotPassword_ServiceResultIsSuccessTrue_ReturnForgotPasswordView()
        {
            // Arrange
            var message = "successMessage";
            var viewModel = new ForgotPasswordViewModel
            {
                EmailAddress = "dev@activebuilder.com"
            };

            var serviceResult = ServiceResult.Success(message);

            identityUserService.Setup(s => s.ForgotPasswordAsync(viewModel.EmailAddress)).ReturnsAsync(serviceResult);
            userViewModelFactory.Setup(x => x.CreateForgotPasswordViewModel()).Returns(viewModel);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = await controller.ForgotPassword(viewModel);

            // Assert
            result.Should().BeViewResult(ViewNames.ForgotPassword)
                    .ModelAs<ForgotPasswordViewModel>()
                    .Should().Be(viewModel);

            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        #endregion

        #region ResetPassword

        [Test]
        public void ResetPassword_ResetTokenIsNull_ReturnResetPasswordView()
        {
            // Arrange
            string token = null;

            // Act
            var result = controller.ResetPassword(token);

            // Assert
            result.Should().BeRedirectToActionResult().WithActionName(HomeControllerActionNames.Index).WithControllerName(ControllerNames.Home);
        }

        [Test]
        public void ResetPassword_ResetTokenIsNotNull_ReturnResetPasswordView()
        {
            // Arrange
            var token = "token";
            var viewModel = new ResetPasswordViewModel();

            userViewModelFactory.Setup(x => x.CreateResetPasswordViewModel(token)).Returns(viewModel);

            // Act
            var result = controller.ResetPassword(token);

            // Assert
            result.Should().BeViewResult(ViewNames.ResetPassword).
                ModelAs<ResetPasswordViewModel>().
                Should().
                BeEquivalentTo(viewModel);
        }

        [Test]
        public async Task ResetPassword_ModelStateIsNotValid_ReturnResetPasswordView()
        {
            // Arrange
            var viewModel = new ResetPasswordViewModel();

            controller.ModelState.AddModelError("", "invalid model");

            // Act
            var result = await controller.ResetPassword(viewModel);

            // Assert
            result.Should().BeViewResult(ViewNames.ResetPassword)
                .ModelAs<ResetPasswordViewModel>()
                .Should().Be(viewModel);
        }

        [Test]
        public async Task ResetPassword_ServiceResultIsSuccessFalse_ReturnResetPasswordView()
        {
            // Arrange
            var message = "errorMessage";
            var viewModel = new ResetPasswordViewModel
            {
                NewPassword = "h123456",
                Token = "token"
            };

            var serviceResult = ServiceResult.Error(message);

            identityUserService.Setup(s => s.ResetPasswordAsync(viewModel.NewPassword, viewModel.Token)).ReturnsAsync(serviceResult);
            userViewModelFactory.Setup(x => x.CreateResetPasswordViewModel(viewModel.Token)).Returns(viewModel);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = await controller.ResetPassword(viewModel);

            // Assert
            result.Should().BeViewResult(ViewNames.ResetPassword)
                    .ModelAs<ResetPasswordViewModel>()
                    .Should().Be(viewModel);

            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public async Task ResetPassword_ServiceResultIsSuccessTrue_ReturnResetPasswordView()
        {
            // Arrange
            var message = "successMessage";
            var viewModel = new ResetPasswordViewModel
            {
                NewPassword = "h123456",
                Token = "token"
            };

            var serviceResult = ServiceResult.Success(message);

            identityUserService.Setup(s => s.ResetPasswordAsync(viewModel.NewPassword, viewModel.Token)).ReturnsAsync(serviceResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = await controller.ResetPassword(viewModel);

            // Assert
            result.Should().BeRedirectToActionResult().WithActionName(AccountControllerActionNames.Login);

            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        #endregion
    }
}