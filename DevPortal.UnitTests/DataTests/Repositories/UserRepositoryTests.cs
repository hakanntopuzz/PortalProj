using AB.Data.DapperClient.Abstract;
using AB.Data.DapperClient.Model.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Factories;
using DevPortal.Data.Repositories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.UnitTests.DataTests.Repositories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class UserRepositoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IDataClient> dataClient;

        StrictMock<IApplicationDataRequestFactory> dataRequestFactory;

        StrictMock<ISettings> settings;

        UserRepository repository;

        [SetUp]
        public void Initialize()
        {
            dataClient = new StrictMock<IDataClient>();
            dataRequestFactory = new StrictMock<IApplicationDataRequestFactory>();
            settings = new StrictMock<ISettings>();
            SetupDataClient();
            repository = new UserRepository(dataClient.Object, dataRequestFactory.Object, settings.Object);
        }

        void SetupDataClient()
        {
            const string devPortalDbConnectionString = "devPortalDbConnectionString";
            settings.SetupGet(x => x.DevPortalDbConnectionString).Returns(devPortalDbConnectionString);
            dataClient.Setup(x => x.SetConnectionString(devPortalDbConnectionString));
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            dataClient.VerifyAll();
            dataRequestFactory.VerifyAll();
            settings.VerifyAll();
        }

        #endregion

        #region get filtered user list

        [Test]
        public async Task GetFilteredUserListAsync_NoCondition_ReturnUserList()
        {
            //Arrange
            Mock<IDataRequest> dataRequest = new Mock<IDataRequest>();
            var defaultReturnValue = new List<User>();
            var searchFilter = new UserSearchFilter();
            ICollection<User> expectedValue = null;

            dataRequestFactory.Setup(x => x.GetFilteredUserList(searchFilter)).Returns(dataRequest.Object);
            dataClient.Setup(x => x.GetCollectionAsync(
               dataRequest.Object,
               DataClientMapFactory.UsersMap,
               defaultReturnValue,
               dataRequest.Object.SplitOnParameters))
               .ReturnsAsync(expectedValue);

            //Act
            var result = await repository.GetFilteredUserListAsync(searchFilter);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        #endregion

        #region get user

        [Test]
        public async Task GetUserAsync_NoCondition_ReturnSuccessResult()
        {
            //Arrange
            var id = 3;
            IDataRequest dataRequest = null;
            User defaultValue = null;
            var expectedValue = new User();

            dataRequestFactory.Setup(x => x.GetUser(id)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItemAsync(dataRequest, defaultValue)).ReturnsAsync(expectedValue);

            //Act
            var result = await repository.GetUserAsync(id);

            //Assert
            result.Should().Be(expectedValue);
        }

        #endregion

        #region update user

        [Test]
        public async Task UpdateUserAsync_NoCondition_ReturnSuccessResult()
        {
            //Arrange
            var user = new User();
            IDataRequest dataRequest = null;
            var defaultValue = false;
            var expectedValue = true;

            dataRequestFactory.Setup(x => x.UpdateUser(user)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalarAsync(dataRequest, defaultValue)).ReturnsAsync(expectedValue);

            //Act
            var result = await repository.UpdateUserAsync(user);

            //Assert
            result.Should().Be(expectedValue);
        }

        #endregion

        #region GetUserStatusList

        [Test]
        public async Task GetUserStatusListAsync_NoCondition_ReturnUserStatusList()
        {
            //Arrange
            IDataRequest dataRequest = null;
            var defaultValue = new List<UserStatus>();
            ICollection<UserStatus> expectedValue = null;

            dataRequestFactory.Setup(x => x.GetUserStatusList()).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollectionAsync(dataRequest, defaultValue)).ReturnsAsync(expectedValue);

            //Act
            var result = await repository.GetUserStatusListAsync();

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        #endregion

        #region GetUserTypeList

        [Test]
        public async Task GetUserTypeListAsync_NoCondition_ReturnUserTypeList()
        {
            //Arrange
            IDataRequest dataRequest = null;
            var defaultValue = new List<UserType>();
            ICollection<UserType> expectedValue = null;

            dataRequestFactory.Setup(x => x.GetUserTypeList()).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollectionAsync(dataRequest, defaultValue)).ReturnsAsync(expectedValue);

            //Act
            var result = await repository.GetUserTypeListAsync();

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        #endregion

        #region AddUser

        [Test]
        public async Task AddUserAsync_NoCondition_ReturnUserId()
        {
            //Arrange
            var user = new User
            {
                Id = 1
            };

            IDataRequest dataRequest = null;
            const int defaultValue = 0;

            dataRequestFactory.Setup(x => x.AddUser(user)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItemAsync(dataRequest, defaultValue)).ReturnsAsync(user.Id);

            //Act
            var result = await repository.AddUserAsync(user);

            //Assert
            result.Should().Be(user.Id);
        }

        #endregion

        #region GetUser

        [Test]
        public async Task GetUserAsync_NoCondition_ReturnUser()
        {
            //Arrange
            var svnUserName = "svnusername";
            IDataRequest dataRequest = null;
            User defaultValue = null;
            var expectedValue = new User
            {
                SvnUserName = svnUserName
            };

            dataRequestFactory.Setup(x => x.GetUser(svnUserName)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItemAsync(dataRequest, defaultValue)).ReturnsAsync(expectedValue);

            //Act
            var result = await repository.GetUserAsync(svnUserName);

            //Assert
            result.Should().Be(expectedValue);
            result.SvnUserName.Should().Be(svnUserName);
        }

        #endregion

        #region GetUserByEmailAddress

        [Test]
        public async Task GetUserByEmailAddressAsync_NoCondition_ReturnUser()
        {
            //Arrange
            var emailAddress = "user1@activebuilder.com";
            IDataRequest dataRequest = null;
            User defaultValue = null;
            var expectedValue = new User
            {
                EmailAddress = emailAddress
            };

            dataRequestFactory.Setup(x => x.GetUserByEmailAddress(emailAddress)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItemAsync(dataRequest, defaultValue)).ReturnsAsync(expectedValue);

            //Act
            var result = await repository.GetUserByEmailAddressAsync(emailAddress);

            //Assert
            result.Should().Be(expectedValue);
            result.EmailAddress.Should().Be(emailAddress);
        }

        #endregion

        #region AddUserLogOnLog

        [Test]
        public async Task AddUserLogOnLogAsync_NoCondition_ReturnTrue()
        {
            var userLogonLog = new UserLogOnLog();
            IDataRequest dataRequest = null;
            var defaultValue = false;
            var expectedValue = true;

            dataRequestFactory.Setup(x => x.AddUserLogOnLog(userLogonLog)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalarAsync(dataRequest, defaultValue)).ReturnsAsync(expectedValue);

            //Act
            var result = await repository.AddUserLogOnLogAsync(userLogonLog);

            //Assert
            result.Should().Be(expectedValue);
        }

        #endregion

        #region delete user

        [Test]
        public async Task DeleteUserAsync_NoCondition_ReturnSuccessResult()
        {
            //Arrange
            var userId = 1;
            IDataRequest dataRequest = null;
            var defaultValue = false;
            var expectedValue = true;

            dataRequestFactory.Setup(x => x.DeleteUser(userId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalarAsync(dataRequest, defaultValue)).ReturnsAsync(expectedValue);

            //Act
            var result = await repository.DeleteUserAsync(userId);

            //Assert
            result.Should().Be(expectedValue);
        }

        #endregion

        #region update own user

        [Test]
        public async Task UpdateOwnUserAsync_NoCondition_ReturnSuccessResult()
        {
            //Arrange
            var user = new User();
            IDataRequest dataRequest = null;
            var defaultValue = false;
            var expectedValue = true;

            dataRequestFactory.Setup(x => x.UpdateOwnUser(user)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalarAsync(dataRequest, defaultValue)).ReturnsAsync(expectedValue);

            //Act
            var result = await repository.UpdateOwnUserAsync(user);

            //Assert
            result.Should().Be(expectedValue);
        }

        #endregion

        #region update own user

        [Test]
        public async Task GetUserUpdateInfoAsync_NoCondition_ReturnSuccessResult()
        {
            //Arrange
            var id = 1;
            IDataRequest dataRequest = null;
            RecordUpdateInfo defaultValue = null;
            var expectedValue = new RecordUpdateInfo();

            dataRequestFactory.Setup(x => x.GetUserUpdateInfo(id)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItemAsync(dataRequest, defaultValue)).ReturnsAsync(expectedValue);

            //Act
            var result = await repository.GetUserUpdateInfoAsync(id);

            //Assert
            result.Should().Be(expectedValue);
        }

        #endregion

        #region AddPasswordResetRequest

        [Test]
        public async Task AddPasswordResetRequestAsync_NoCondition_ReturnSuccessResult()
        {
            //Arrange
            var userId = 1;
            var ipAddress = "1.1.1.1";
            var requestCode = "requestcode";
            IDataRequest dataRequest = null;
            var defaultValue = false;
            var expectedValue = true;

            dataRequestFactory.Setup(x => x.AddPasswordResetRequest(userId, ipAddress, requestCode)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalarAsync(dataRequest, defaultValue)).ReturnsAsync(expectedValue);

            //Act
            var result = await repository.AddPasswordResetRequestAsync(userId, ipAddress, requestCode);

            //Assert
            result.Should().Be(expectedValue);
        }

        #endregion

        #region CheckPasswordResetRequest

        [Test]
        public async Task CheckPasswordResetRequestAsync_NoCondition_ReturnSuccessResult()
        {
            //Arrange
            var requestCode = "requestcode";
            IDataRequest dataRequest = null;
            var defaultValue = 0;
            var expectedValue = 1;

            dataRequestFactory.Setup(x => x.CheckPasswordResetRequest(requestCode)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItemAsync(dataRequest, defaultValue)).ReturnsAsync(expectedValue);

            //Act
            var result = await repository.CheckPasswordResetRequestAsync(requestCode);

            //Assert
            result.Should().Be(expectedValue);
        }

        #endregion

        #region UpdatePasswordResetRequest

        [Test]
        public async Task UpdatePasswordResetRequestAsync_NoCondition_ReturnSuccessResult()
        {
            //Arrange
            var userId = 1;
            IDataRequest dataRequest = null;
            var defaultValue = false;
            var expectedValue = true;

            dataRequestFactory.Setup(x => x.UpdatePasswordResetRequest(userId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalarAsync(dataRequest, defaultValue)).ReturnsAsync(expectedValue);

            //Act
            var result = await repository.UpdatePasswordResetRequestAsync(userId);

            //Assert
            result.Should().Be(expectedValue);
        }

        #endregion
    }
}