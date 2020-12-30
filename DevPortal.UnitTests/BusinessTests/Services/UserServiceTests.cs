using AB.Framework.Logger.Nlog.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Business.Services;
using DevPortal.Data.Abstract;
using DevPortal.Model;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class UserServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserRepository> userRepository;

        StrictMock<IAuditService> auditService;

        StrictMock<ILoggingService> loggingService;

        StrictMock<IAuditFactory> auditFactory;

        UserService service;

        [SetUp]
        public void Initialize()
        {
            userRepository = new StrictMock<IUserRepository>();
            auditService = new StrictMock<IAuditService>();
            loggingService = new StrictMock<ILoggingService>();
            auditFactory = new StrictMock<IAuditFactory>();

            service = new UserService(userRepository.Object, auditService.Object, loggingService.Object, auditFactory.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            userRepository.VerifyAll();
            auditService.VerifyAll();
            loggingService.VerifyAll();
        }

        #endregion

        #region get user

        [Test]
        public async Task GetUserAsync_UserNotFound_ReturnNull()
        {
            // Arrange
            var id = 3;
            User user = null;

            userRepository.Setup(x => x.GetUserAsync(id)).ReturnsAsync(user);

            // Act
            var result = await service.GetUserAsync(id);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task GetUserAsync_UserFound_ReturnUser()
        {
            // Arrange
            var id = 3;
            var user = new User();
            var recordUpdateInfo = new RecordUpdateInfo();

            userRepository.Setup(x => x.GetUserAsync(id)).ReturnsAsync(user);
            userRepository.Setup(x => x.GetUserUpdateInfoAsync(id)).ReturnsAsync(recordUpdateInfo);

            // Act
            var result = await service.GetUserAsync(id);

            // Assert
            result.Should().Be(user);
        }

        #endregion

        #region get filtered users jq table

        [Test]
        public async Task GetFilteredUsersJqTableAsync_NoCondition_ReturnUser()
        {
            // Arrange
            var users = new List<User>();
            var searchFilter = new UserSearchFilter();
            var jqtable = new JQTable
            {
                data = users,
                recordsFiltered = 0,
                recordsTotal = 0
            };

            userRepository.Setup(x => x.GetFilteredUserListAsync(searchFilter)).ReturnsAsync(users);

            // Act
            var result = await service.GetFilteredUsersJqTableAsync(searchFilter);

            // Assert
            result.Should().BeEquivalentTo(jqtable);
        }

        #endregion

        #region update user

        [Test]
        public async Task UpdateUserAsync_UserIsNull_ReturnError()
        {
            // Arrange
            User user = null;

            // Act
            var result = await service.UpdateUserAsync(user);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.NullParameterError);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task UpdateUserAsync_UserEmailAddressExists_ReturnServiceErrorResult()
        {
            // Arrange
            var user = new User { Id = 1, EmailAddress = "test@test.com" };
            var returnUser = new User { Id = 2, EmailAddress = "test@test.com" };
            var serviceResult = ServiceResult.Error(Messages.UserEmailAddressExists);

            userRepository.Setup(x => x.GetUserByEmailAddressAsync(user.EmailAddress)).ReturnsAsync(returnUser);

            // Act
            var result = await service.UpdateUserAsync(user);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public async Task UpdateUserAsync_UserSvnUserNameExists_ReturnServiceErrorResult()
        {
            // Arrange
            var user = new User { Id = 1, SvnUserName = "test" };
            var returnUser = new User { Id = 2, SvnUserName = "test" };
            var serviceResult = ServiceResult.Error(Messages.SvnUserNameExists);

            userRepository.Setup(x => x.GetUserByEmailAddressAsync(user.EmailAddress)).ReturnsAsync(user);
            userRepository.Setup(x => x.GetUserAsync(user.SvnUserName)).ReturnsAsync(returnUser);

            // Act
            var result = await service.UpdateUserAsync(user);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public async Task UpdateUserAsync_HasNoChange_ReturnSuccess()
        {
            // Arrange

            var user = new User { Id = 2, SvnUserName = "name" };
            var returnUser = new User { Id = 2, SvnUserName = "name2" };
            User oldUser = null;

            userRepository.Setup(x => x.GetUserByEmailAddressAsync(user.EmailAddress)).ReturnsAsync(user);
            userRepository.Setup(x => x.GetUserAsync(user.SvnUserName)).ReturnsAsync(returnUser);
            userRepository.Setup(x => x.GetUserAsync(user.Id)).ReturnsAsync(oldUser);
            auditService.Setup(x => x.IsChanged(oldUser, user, nameof(User))).Returns(false);

            // Act
            var result = await service.UpdateUserAsync(user);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.UserUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task UpdateUserAsync_IsNotSuccess_ReturnError()
        {
            // Arrange
            var user = new User { Id = 1, EmailAddress = "test@test1.com" };
            var returnUser = new User { Id = 1, SvnUserName = "svn-name", EmailAddress = "test@test1.com" };
            User oldUser = null;

            userRepository.Setup(x => x.GetUserByEmailAddressAsync(user.EmailAddress)).ReturnsAsync(returnUser);
            userRepository.Setup(x => x.GetUserAsync(user.SvnUserName)).ReturnsAsync(returnUser);
            userRepository.Setup(x => x.GetUserAsync(user.Id)).ReturnsAsync(oldUser);
            auditService.Setup(x => x.IsChanged(oldUser, user, nameof(User))).Returns(true);
            userRepository.Setup(x => x.UpdateUserAsync(user)).ReturnsAsync(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = await service.UpdateUserAsync(user);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task UpdateUserAsync_AddAuditFalse_ReturnError()
        {
            // Arrange
            var user = new User();
            var isSuccess = true;
            User returnUser = null;
            var oldUser = new User();
            var auditInfo = new AuditInfo();

            userRepository.Setup(x => x.GetUserByEmailAddressAsync(user.EmailAddress)).ReturnsAsync(returnUser);
            userRepository.Setup(x => x.GetUserAsync(user.SvnUserName)).ReturnsAsync(returnUser);
            userRepository.Setup(x => x.GetUserAsync(user.Id)).ReturnsAsync(oldUser);
            auditService.Setup(x => x.IsChanged(oldUser, user, nameof(User))).Returns(true);
            userRepository.Setup(x => x.UpdateUserAsync(user)).ReturnsAsync(isSuccess);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(User), user.Id, oldUser, oldUser, Convert.ToInt32(user.RecordUpdateInfo.ModifiedBy))).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = await service.UpdateUserAsync(user);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task UpdateUserAsync_IsSuccess_ReturnOk()
        {
            // Arrange
            var user = new User();
            var isSuccess = true;
            User returnUser = null;
            var oldUser = new User();
            var auditInfo = new AuditInfo();

            userRepository.Setup(x => x.GetUserByEmailAddressAsync(user.EmailAddress)).ReturnsAsync(returnUser);
            userRepository.Setup(x => x.GetUserAsync(user.SvnUserName)).ReturnsAsync(returnUser);
            userRepository.Setup(x => x.GetUserAsync(user.Id)).ReturnsAsync(oldUser);
            auditService.Setup(x => x.IsChanged(oldUser, user, nameof(User))).Returns(true);
            userRepository.Setup(x => x.UpdateUserAsync(user)).ReturnsAsync(isSuccess);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(User), user.Id, oldUser, oldUser, Convert.ToInt32(user.RecordUpdateInfo.ModifiedBy))).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(true);

            // Act
            var result = await service.UpdateUserAsync(user);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.UserUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region get user status list

        [Test]
        public async Task GetUserStatusListAsync_NoCondition_ReturnUserStatusList()
        {
            // Arrange
            var userStatusList = new List<UserStatus>
            {
                new UserStatus()
                {
                    Id = 1,
                    Name = "Aktif"
                }
            };

            userRepository.Setup(x => x.GetUserStatusListAsync()).ReturnsAsync(userStatusList);

            // Act
            var result = await service.GetUserStatusListAsync();

            // Assert
            result.Should().NotBeEmpty();
            result.Should().BeEquivalentTo(userStatusList);
        }

        #endregion

        #region get user type list

        [Test]
        public async Task GetUserTypeListAsync_NoCondition_ReturnUserTypeList()
        {
            // Arrange
            var userTypeList = new List<UserType>
            {
                new UserType()
                {
                    Id = 1,
                    Name = "Geliştirici"
                }
            };

            userRepository.Setup(x => x.GetUserTypeListAsync()).ReturnsAsync(userTypeList);

            // Act
            var result = await service.GetUserTypeListAsync();

            // Assert
            result.Should().NotBeEmpty();
            result.Should().BeEquivalentTo(userTypeList);
        }

        #endregion

        #region delete user

        [Test]
        public async Task DeleteUserAsync_DeleteUserFails_ReturnServiceErrorResult()
        {
            // Arrange
            var userId = 1;
            var serviceResult = ServiceResult.Error(Messages.DeleteFails);
            var deleteResult = false;

            userRepository.Setup(x => x.DeleteUserAsync(userId)).ReturnsAsync(deleteResult);

            // Act
            var result = await service.DeleteUserAsync(userId);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public async Task DeleteUserAsync_DeleteUserSucceeds_ReturnServiceErrorResult()
        {
            // Arrange
            var userId = 1;
            var serviceResult = ServiceResult.Success(Messages.UserDeleted);
            var deleteResult = true;

            userRepository.Setup(x => x.DeleteUserAsync(userId)).ReturnsAsync(deleteResult);

            // Act
            var result = await service.DeleteUserAsync(userId);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion
    }
}