using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library
{
    public class UserViewModelFactory : IUserViewModelFactory
    {
        #region ctor

        readonly IBreadCrumbFactory breadCrumbFactory;

        public UserViewModelFactory(
            IBreadCrumbFactory breadCrumbFactory)
        {
            this.breadCrumbFactory = breadCrumbFactory;
        }

        #endregion

        public UserListViewModel CreateUserListViewModel()
        {
            return new UserListViewModel
            {
                BreadCrumbViewModel = breadCrumbFactory.CreateUserListModel()
            };
        }

        public UserDetailViewModel CreateUserDetailViewModel(User user)
        {
            return new UserDetailViewModel
            {
                User = user,
                BreadCrumbViewModel = breadCrumbFactory.CreateDetailUserModel()
            };
        }

        public UserDetailViewModel CreateUserProfileViewModel(User user)
        {
            return new UserDetailViewModel
            {
                User = user,
                BreadCrumbViewModel = breadCrumbFactory.CreateUserProfileModel()
            };
        }

        public AddUserViewModel CreateUserAddViewModel(ICollection<UserStatus> userStatusList, ICollection<UserType> userTypeList)
        {
            return new AddUserViewModel
            {
                UserStatus = userStatusList,
                UserTypes = userTypeList,
                BreadCrumbViewModel = breadCrumbFactory.CreateUserAddModel()
            };
        }

        public LoginViewModel CreateLoginViewModel(string returnUrl)
        {
            return new LoginViewModel
            {
                ReturnUrl = returnUrl
            };
        }

        public ChangePasswordViewModel CreateChangePasswordViewModel()
        {
            return new ChangePasswordViewModel
            {
                BreadCrumbViewModel = breadCrumbFactory.CreateChangePasswordModel()
            };
        }

        public ResetPasswordViewModel CreateResetPasswordViewModel(string token)
        {
            return new ResetPasswordViewModel
            {
                Token = token,
                BreadCrumbViewModel = breadCrumbFactory.CreateResetPasswordModel()
            };
        }

        public EditUserViewModel CreateEditUserViewModel(User user, ICollection<UserStatus> userStatusList, ICollection<UserType> userTypeList)
        {
            return new EditUserViewModel
            {
                User = user,
                UserStatus = userStatusList,
                UserTypes = userTypeList,
                BreadCrumbViewModel = breadCrumbFactory.CreateEditUserModel(user.Id)
            };
        }

        public ForgotPasswordViewModel CreateForgotPasswordViewModel()
        {
            return new ForgotPasswordViewModel
            {
                BreadCrumbViewModel = breadCrumbFactory.CreateForgotPasswordModel()
            };
        }
    }
}