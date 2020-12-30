using AB.Framework.Logger.Nlog.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Identity.Business;
using DevPortal.Identity.Business.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DevPortal.UnitTests.IdentityTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class IdentityUserServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IHttpContextWrapper> httpContextWrapper;

        StrictMock<IIdentityFactory> identityFactory;

        StrictMock<IUserRepository> userRepository;

        StrictMock<ILoggingService> loggingService;

        StrictMock<IUrlGeneratorService> urlGeneratorService;

        StrictMock<IRouteValueFactory> routeValueFactory;

        StrictMock<IEmailSenderService> emailSenderService;

        Mock<UserManager<User>> userManager;

        IdentityUserService service;

        [SetUp]
        public void Initialize()
        {
            identityFactory = new StrictMock<IIdentityFactory>();
            userRepository = new StrictMock<IUserRepository>();
            httpContextWrapper = new StrictMock<IHttpContextWrapper>();
            loggingService = new StrictMock<ILoggingService>();
            urlGeneratorService = new StrictMock<IUrlGeneratorService>();
            routeValueFactory = new StrictMock<IRouteValueFactory>();
            emailSenderService = new StrictMock<IEmailSenderService>();

            var store = new Mock<IUserStore<User>>();
            userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            service = new IdentityUserService(
                    userManager.Object,
                    identityFactory.Object,
                    httpContextWrapper.Object,
                    userRepository.Object,
                    loggingService.Object,
                    urlGeneratorService.Object,
                    routeValueFactory.Object,
                    emailSenderService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            userRepository.VerifyAll();
            identityFactory.VerifyAll();
            userManager.VerifyAll();
            httpContextWrapper.VerifyAll();
            loggingService.VerifyAll();
            urlGeneratorService.VerifyAll();
            routeValueFactory.VerifyAll();
            emailSenderService.VerifyAll();
        }

        #endregion

        #region setup helpers

        User CreateUser()
        {
            return new User
            {
                Id = 1,
                EmailAddress = "user1@activebuilder.com"
            };
        }

        User CreateUser(string firstName, string lastName)
        {
            var user = CreateUser();
            user.FirstName = firstName;
            user.LastName = lastName;

            return user;
        }

        #endregion

        #region create user

        [Test]
        public async Task CreateUserAsync_ResultSucceedTrue_ReturnInt32ServiceResult()
        {
            //Arrange
            var user = CreateUser();
            var password = "h123456";
            var identityResult = IdentityResult.Success;

            userManager.Setup(x => x.CreateAsync(user, password)).Returns(Task.FromResult(identityResult));

            //Act
            var result = await service.CreateUserAsync(user, password);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be(Messages.AddingNewUserSucceed);
            result.Value.Should().Be(user.Id);
        }

        [Test]
        public async Task CreateUserAsync_ResultSucceedFalse_ReturnInt32ServiceResult()
        {
            //Arrange
            var user = CreateUser();
            var password = "h123456";
            var identityResult = IdentityResult.Failed();

            userManager.Setup(x => x.CreateAsync(user, password)).Returns(Task.FromResult(identityResult));

            //Act
            var result = await service.CreateUserAsync(user, password);

            //Assert
            result.IsSuccess.Should().BeFalse();
        }

        #endregion

        #region update user

        [Test]
        public async Task UpdateUserAsync_ResultSucceedTrue_ReturnServiceResult()
        {
            //Arrange
            var user = CreateUser();
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity("cookies"));
            var identityResult = IdentityResult.Success;

            httpContextWrapper.Setup(s => s.GetCurrentUser()).Returns(claimsPrincipal);
            userManager.Setup(s => s.GetUserAsync(claimsPrincipal)).ReturnsAsync(user);
            userManager.Setup(x => x.UpdateAsync(user)).ReturnsAsync(identityResult);

            //Act
            var result = await service.UpdateUserAsync(user);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be(Messages.UserUpdateSucceeds);
        }

        [Test]
        public async Task UpdateUserAsync_ResultSucceedFalse_ReturnServiceResult()
        {
            //Arrange
            var user = CreateUser();
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity("cookies"));
            var identityResult = IdentityResult.Failed();

            httpContextWrapper.Setup(s => s.GetCurrentUser()).Returns(claimsPrincipal);
            userManager.Setup(s => s.GetUserAsync(claimsPrincipal)).ReturnsAsync(user);
            userManager.Setup(x => x.UpdateAsync(user)).ReturnsAsync(identityResult);

            //Act
            var result = await service.UpdateUserAsync(user);

            //Assert
            result.IsSuccess.Should().BeFalse();
        }

        #endregion

        #region GetUser

        [Test]
        public async Task GetUserAsync_NoCondition_ReturnUser()
        {
            //Arrange
            var user = CreateUser();
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity("cookies"));

            httpContextWrapper.Setup(s => s.GetCurrentUser()).Returns(claimsPrincipal);
            userManager.Setup(s => s.GetUserAsync(claimsPrincipal)).ReturnsAsync(user);

            //Act
            var result = await service.GetUserAsync();

            //Assert
            result.EmailAddress.Should().Be(user.EmailAddress);
        }

        #endregion

        #region GetUserWithUpdateInfo

        [Test]
        public async Task GetUserWithUpdateInfoAsync_NoCondition_ReturnUser()
        {
            //Arrange
            int userId = 45;

            var user = CreateUser("first-name", "last-name");
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity("cookies"));
            var recordUpdateInfo = new RecordUpdateInfo
            {
                CreatedBy = 14,
                ModifiedBy = 75,
                CreatedDate = DateTime.Now.AddDays(-1),
                ModifiedDate = DateTime.Now
            };
            var expectedUser = CreateUser("first-name", "last-name");
            expectedUser.RecordUpdateInfo = recordUpdateInfo;

            httpContextWrapper.Setup(s => s.GetCurrentUser()).Returns(claimsPrincipal);
            userManager.Setup(s => s.GetUserAsync(claimsPrincipal)).ReturnsAsync(user);
            userRepository.Setup(x => x.GetUserUpdateInfoAsync(userId)).ReturnsAsync(recordUpdateInfo);

            //Act
            var result = await service.GetUserWithUpdateInfoAsync(userId);

            //Assert
            result.Should().BeEquivalentTo(expectedUser);
        }

        #endregion

        #region Login

        [Test]
        public async Task LoginAsync_UserNotFound_ReturnUserServiceResult()
        {
            //Arrange
            var emailAddress = "user2@activebuilder.com";
            var password = "h123456";
            var isRemember = true;
            User user = null;

            userManager.Setup(s => s.FindByEmailAsync(emailAddress)).Returns(Task.FromResult(user));

            //Act
            var result = await service.LoginAsync(emailAddress, password, isRemember);

            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(Messages.InvalidUserInformation);
        }

        [Test]
        public async Task LoginAsync_PasswordInCorrect_ReturnUserServiceResult()
        {
            //Arrange
            var emailAddress = "user1@activebuilder.com";
            var password = "h123456";
            var isRemember = true;
            var user = CreateUser();

            userManager.Setup(s => s.FindByEmailAsync(emailAddress)).Returns(Task.FromResult(user));
            userManager.Setup(s => s.CheckPasswordAsync(user, password)).Returns(Task.FromResult(false));

            //Act
            var result = await service.LoginAsync(emailAddress, password, isRemember);

            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(Messages.InvalidUserInformation);
        }

        [Test]
        public async Task LoginAsync_ResultTrue_ReturnUserServiceResult()
        {
            //Arrange
            var emailAddress = "user1@activebuilder.com";
            var password = "h123456";
            var isRemember = true;
            var user = CreateUser();

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity("cookies"));

            userManager.Setup(s => s.FindByEmailAsync(emailAddress)).Returns(Task.FromResult(user));
            userManager.Setup(s => s.CheckPasswordAsync(user, password)).Returns(Task.FromResult(true));
            identityFactory.Setup(c => c.CreateUserClaims(user)).Returns(claimsPrincipal);

            httpContextWrapper.Setup(s => s.SignInAsync(claimsPrincipal, isRemember)).Returns(Task.FromResult(true));
            AddUserLogonLogTest();

            //Act
            var result = await service.LoginAsync(emailAddress, password, isRemember);

            //Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Test]
        public async Task LoginAsync_Throws_ReturnServiceResult()
        {
            //Arrange
            var emailAddress = "user1@activebuilder.com";
            var password = "h123456";
            var isRemember = true;

            Exception exception = new Exception();

            userManager.Setup(s => s.FindByEmailAsync(emailAddress)).Throws(exception);
            loggingService.Setup(x => x.LogError("LoginAsync", Messages.GeneralError, exception));

            //Act
            var result = await service.LoginAsync(emailAddress, password, isRemember);

            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(Messages.GeneralError);
        }

        void AddUserLogonLogTest()
        {
            //Arrange
            var userAgent = new UserAgent
            {
                BrowserName = "Chrome",
                BrowserVersion = "80.0",
                IpAddress = "1.1.1.1"
            };

            var userLogOnLog = new UserLogOnLog();

            identityFactory.Setup(s => s.CreateUserLogOnLog(It.IsAny<string>(), It.IsAny<int>())).Returns(userLogOnLog);
            userRepository.Setup(s => s.AddUserLogOnLogAsync(SetupAny<UserLogOnLog>())).ReturnsAsync(true);
        }

        #endregion

        #region LogOut

        [Test]
        public async Task LogOutAsync_Success_ReturnTrue()
        {
            //Arrange
            httpContextWrapper.Setup(s => s.SignOutAsync()).Returns(Task.CompletedTask);

            //Act
            var result = await service.LogOutAsync();

            //Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Test]
        public async Task LogOutAsync_LogoutFails_ReturnFalse()
        {
            //Arrange
            Exception exception = new Exception();

            httpContextWrapper.Setup(s => s.SignOutAsync()).Throws(exception);
            loggingService.Setup(x => x.LogError("LogOutAsync", Messages.GeneralError, exception));

            //Act
            var result = await service.LogOutAsync();

            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(Messages.GeneralError);
        }

        #endregion

        #region ChangePassword

        [Test]
        public async Task ChangePasswordAsync_ResultSucceedTrue_ReturnServiceResult()
        {
            //Arrange
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity("cookies"));
            var identityResult = IdentityResult.Success;
            var password = "h123456";
            var newPassword = "h1234567";
            var user = CreateUser();

            httpContextWrapper.Setup(s => s.GetCurrentUser()).Returns(claimsPrincipal);
            userManager.Setup(s => s.GetUserAsync(claimsPrincipal)).ReturnsAsync(user);
            userManager.Setup(x => x.ChangePasswordAsync(user, password, newPassword)).ReturnsAsync(identityResult);

            //Act
            var result = await service.ChangePasswordAsync(password, newPassword);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be(Messages.PasswordChangeSucceeds);
        }

        [Test]
        public async Task ChangePasswordAsync_ResultSucceedFalse_ReturnServiceResult()
        {
            //Arrange
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity("cookies"));
            var identityResult = IdentityResult.Failed();
            var password = "h123456";
            var newPassword = "h123456";
            var user = CreateUser();

            httpContextWrapper.Setup(s => s.GetCurrentUser()).Returns(claimsPrincipal);
            userManager.Setup(s => s.GetUserAsync(claimsPrincipal)).ReturnsAsync(user);
            userManager.Setup(x => x.ChangePasswordAsync(user, password, newPassword)).ReturnsAsync(identityResult);

            //Act
            var result = await service.ChangePasswordAsync(password, newPassword);

            //Assert
            result.IsSuccess.Should().BeFalse();
        }

        #endregion

        #region ForgotPassword

        [Test]
        public async Task ForgotPasswordAsync_UserIsNull_ReturnServiceResult()
        {
            //Arrange
            User user = null;
            var emailAddress = "dev@activebuilder.com";

            userManager.Setup(s => s.FindByEmailAsync(emailAddress)).ReturnsAsync(user);

            //Act
            var result = await service.ForgotPasswordAsync(emailAddress);

            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(Messages.UserNotFoundByEmailAddress);
        }

        [Test]
        public async Task ForgotPasswordAsync_ResultSucceedFalse_ReturnStringServiceResult()
        {
            //Arrange
            var ipAddress = "1.1.1.1";
            var token = "token";
            var emailAddress = "dev@activebuilder.com";
            var user = CreateUser();

            userManager.Setup(s => s.FindByEmailAsync(emailAddress)).ReturnsAsync(user);
            userManager.Setup(s => s.GeneratePasswordResetTokenAsync(user)).ReturnsAsync(token);
            httpContextWrapper.Setup(s => s.GetUserIpAddress()).Returns(ipAddress);

            userRepository.Setup(s => s.AddPasswordResetRequestAsync(user.Id, ipAddress, token)).ReturnsAsync(false);

            //Act
            var result = await service.ForgotPasswordAsync(emailAddress);

            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(Messages.GeneralError);
        }

        [Test]
        public async Task ForgotPasswordAsync_ResultSucceedTrue_ReturnStringServiceResult()
        {
            //Arrange
            var ipAddress = "1.1.1.1";
            var token = "token";
            var emailAddress = "dev@activebuilder.com";
            var resetUrl = "resetUrl";
            var user = CreateUser();

            var routeValue = new RouteValueDictionary();

            userManager.Setup(s => s.FindByEmailAsync(emailAddress)).ReturnsAsync(user);
            userManager.Setup(s => s.GeneratePasswordResetTokenAsync(user)).ReturnsAsync(token);
            httpContextWrapper.Setup(s => s.GetUserIpAddress()).Returns(ipAddress);

            userRepository.Setup(s => s.AddPasswordResetRequestAsync(user.Id, ipAddress, token)).ReturnsAsync(true);
            routeValueFactory.Setup(s => s.CreateRouteValuesForToken(token)).Returns(routeValue);
            urlGeneratorService.Setup(s => s.GenerateUrl(AccountControllerActionNames.ResetPassword, ControllerNames.Account, routeValue)).Returns(resetUrl);

            emailSenderService.Setup(s => s.SendForgotPasswordMail(emailAddress, resetUrl)).Returns(true);

            //Act
            var result = await service.ForgotPasswordAsync(emailAddress);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be(Messages.PasswordResetRequestSucceeds);
        }

        #endregion

        #region ResetPassword

        [Test]
        public async Task ResetPasswordAsync_UserIdEqualsZero_ReturnServiceResult()
        {
            //Arrange
            var userId = 0;
            var token = "token";
            var password = "password";

            userRepository.Setup(s => s.CheckPasswordResetRequestAsync(token)).ReturnsAsync(userId);

            //Act
            var result = await service.ResetPasswordAsync(password, token);

            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(Messages.InvalidPasswordRequestToken);
        }

        [Test]
        public async Task ResetPasswordAsync_ResultSucceedFalse_ReturnServiceResult()
        {
            //Arrange
            var identityResult = IdentityResult.Failed();
            var userId = 1;
            var token = "token";
            var password = "password";
            var user = CreateUser();

            userRepository.Setup(s => s.CheckPasswordResetRequestAsync(token)).ReturnsAsync(userId);
            userManager.Setup(s => s.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            userManager.Setup(s => s.ResetPasswordAsync(user, token, password)).ReturnsAsync(identityResult);

            //Act
            var result = await service.ResetPasswordAsync(password, token);

            //Assert
            result.IsSuccess.Should().BeFalse();
        }

        [Test]
        public async Task ResetPasswordAsync_ResultSucceedTrue_ReturnServiceResult()
        {
            //Arrange
            var identityResult = IdentityResult.Success;
            var userId = 1;
            var token = "token";
            var password = "password";
            var user = CreateUser();

            userRepository.Setup(s => s.CheckPasswordResetRequestAsync(token)).ReturnsAsync(userId);
            userManager.Setup(s => s.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            userManager.Setup(s => s.ResetPasswordAsync(user, token, password)).ReturnsAsync(identityResult);

            userRepository.Setup(s => s.UpdatePasswordResetRequestAsync(user.Id)).ReturnsAsync(true);

            //Act
            var result = await service.ResetPasswordAsync(password, token);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be(Messages.ResetPasswordSucceed);
        }

        #endregion
    }
}