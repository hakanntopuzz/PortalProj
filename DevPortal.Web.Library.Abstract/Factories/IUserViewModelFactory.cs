using DevPortal.Model;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Abstract
{
    public interface IUserViewModelFactory
    {
        UserListViewModel CreateUserListViewModel();

        UserDetailViewModel CreateUserDetailViewModel(User user);

        UserDetailViewModel CreateUserProfileViewModel(User user);

        AddUserViewModel CreateUserAddViewModel(ICollection<UserStatus> userStatusList, ICollection<UserType> userTypeList);

        LoginViewModel CreateLoginViewModel(string returnUrl);

        ChangePasswordViewModel CreateChangePasswordViewModel();

        ResetPasswordViewModel CreateResetPasswordViewModel(string token);

        EditUserViewModel CreateEditUserViewModel(User user, ICollection<UserStatus> userStatusList, ICollection<UserType> userTypeList);

        ForgotPasswordViewModel CreateForgotPasswordViewModel();
    }
}