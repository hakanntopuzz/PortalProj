using AB.Framework.UnitTests;
using DevPortal.Model;
using DevPortal.Web.Library;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.WebLibraryTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class UserViewModelFactoryTests : BaseTestFixture
    {
        #region members & setup

        UserViewModelFactory factory;

        StrictMock<IBreadCrumbFactory> breadCrumbFactory;

        [SetUp]
        public void Initialize()
        {
            breadCrumbFactory = new StrictMock<IBreadCrumbFactory>();

            factory = new UserViewModelFactory(breadCrumbFactory.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            breadCrumbFactory.VerifyAll();
        }

        #endregion

        #region user

        [Test]
        public void CreateUserListViewModel_NoCondition_ReturnUserListViewModel()
        {
            // Arrange
            var userListViewModel = new UserListViewModel();
            BreadCrumbViewModel model = null;

            breadCrumbFactory.Setup(x => x.CreateUserListModel()).Returns(model);

            // Act
            var result = factory.CreateUserListViewModel();

            // Assert
            var expectedResult = new UserListViewModel
            {
                BreadCrumbViewModel = model
            };
            result.Should().BeEquivalentTo(result);
        }

        [Test]
        public void CreateUserDetailViewModel_NoCondition_ReturnUserDetailViewModel()
        {
            // Arrange
            var user = new User();
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var viewModel = new UserDetailViewModel
            {
                User = user,
                BreadCrumbViewModel = breadcrumbViewModel
            };

            breadCrumbFactory.Setup(x => x.CreateDetailUserModel()).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateUserDetailViewModel(user);

            // Assert
            result.Should().BeEquivalentTo(viewModel);
        }

        [Test]
        public void CreateUserProfileViewModel_NoCondition_ReturnUserDetailViewModel()
        {
            // Arrange
            var user = new User();
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var userDetailViewModel = new UserDetailViewModel
            {
                User = user,
                BreadCrumbViewModel = breadcrumbViewModel
            };

            breadCrumbFactory.Setup(x => x.CreateUserProfileModel()).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateUserProfileViewModel(user);

            // Assert
            result.Should().BeEquivalentTo(userDetailViewModel);
            result.User.Should().BeSameAs(userDetailViewModel.User);
            result.BreadCrumbViewModel.Should().BeSameAs(userDetailViewModel.BreadCrumbViewModel);
        }

        [Test]
        public void CreateUserAddViewModel_NoCondition_ReturnAddUserViewModel()
        {
            // Arrange
            ICollection<UserStatus> userStatusList = null;
            ICollection<UserType> userTypeList = null;
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var addUserViewModel = new AddUserViewModel
            {
                UserStatus = userStatusList,
                UserTypes = userTypeList,
                BreadCrumbViewModel = breadcrumbViewModel
            };

            breadCrumbFactory.Setup(x => x.CreateUserAddModel()).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateUserAddViewModel(userStatusList, userTypeList);

            // Assert
            result.Should().BeEquivalentTo(addUserViewModel);
            result.UserStatus.Should().BeSameAs(addUserViewModel.UserStatus);
            result.UserTypes.Should().BeSameAs(addUserViewModel.UserTypes);
            result.BreadCrumbViewModel.Should().BeSameAs(addUserViewModel.BreadCrumbViewModel);
        }

        [Test]
        public void CreateLoginViewModel_NoCondition_ReturnLoginViewModel()
        {
            // Arrange
            var returnUrl = "returnUrl";
            var loginViewModel = new LoginViewModel { ReturnUrl = returnUrl };

            // Act
            var result = factory.CreateLoginViewModel(returnUrl);

            // Assert
            result.Should().BeEquivalentTo(loginViewModel);
            result.ReturnUrl.Should().BeSameAs(loginViewModel.ReturnUrl);
        }

        [Test]
        public void CreateChangePasswordViewModel_NoCondition_ReturnChangePasswordViewModel()
        {
            // Arrange
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var changePasswordViewModel = new ChangePasswordViewModel
            {
                BreadCrumbViewModel = breadcrumbViewModel
            };

            breadCrumbFactory.Setup(x => x.CreateChangePasswordModel()).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateChangePasswordViewModel();

            // Assert
            result.Should().BeEquivalentTo(changePasswordViewModel);
            result.BreadCrumbViewModel.Should().BeSameAs(changePasswordViewModel.BreadCrumbViewModel);
        }

        [Test]
        public void CreateResetPasswordViewModel_NoCondition_ReturnResetPasswordViewModel()
        {
            // Arrange
            var token = "token";
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var resetPasswordViewModel = new ResetPasswordViewModel
            {
                Token = token,
                BreadCrumbViewModel = breadcrumbViewModel
            };

            breadCrumbFactory.Setup(x => x.CreateResetPasswordModel()).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateResetPasswordViewModel(token);

            // Assert
            result.Should().BeEquivalentTo(resetPasswordViewModel);
            result.Token.Should().BeSameAs(resetPasswordViewModel.Token);
            result.BreadCrumbViewModel.Should().BeSameAs(resetPasswordViewModel.BreadCrumbViewModel);
        }

        [Test]
        public void CreateEditUserViewModel_NoCondition_ReturnEditUserViewModel()
        {
            // Arrange
            var user = new User();
            ICollection<UserStatus> userStatusList = null;
            ICollection<UserType> userTypeList = null;
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var editUserViewModel = new EditUserViewModel
            {
                User = user,
                UserStatus = userStatusList,
                UserTypes = userTypeList,
                BreadCrumbViewModel = breadcrumbViewModel
            };

            breadCrumbFactory.Setup(x => x.CreateEditUserModel(user.Id)).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateEditUserViewModel(user, userStatusList, userTypeList);

            // Assert
            result.Should().BeEquivalentTo(editUserViewModel);
            result.User.Should().BeSameAs(editUserViewModel.User);
            result.UserStatus.Should().BeSameAs(editUserViewModel.UserStatus);
            result.UserTypes.Should().BeSameAs(editUserViewModel.UserTypes);
            result.BreadCrumbViewModel.Should().BeSameAs(editUserViewModel.BreadCrumbViewModel);
        }

        [Test]
        public void CreateForgotPasswordViewModel_NoCondition_ReturnForgotPasswordViewModel()
        {
            // Arrange
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var forgotPasswordViewModel = new ForgotPasswordViewModel
            {
                BreadCrumbViewModel = breadcrumbViewModel
            };

            breadCrumbFactory.Setup(x => x.CreateForgotPasswordModel()).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateForgotPasswordViewModel();

            // Assert
            result.Should().BeEquivalentTo(forgotPasswordViewModel);
            result.BreadCrumbViewModel.Should().BeSameAs(forgotPasswordViewModel.BreadCrumbViewModel);
        }

        #endregion
    }
}