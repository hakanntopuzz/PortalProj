using AB.Framework.Logger.Nlog.Abstract;
using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Identity.Business.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.Identity.Business
{
    public class IdentityUserService : IIdentityUserService
    {
        #region ctor

        readonly UserManager<User> userManager;

        readonly IHttpContextWrapper httpContextWrapper;

        readonly IIdentityFactory identityFactory;

        readonly IUserRepository userRepository;

        readonly ILoggingService loggingService;

        readonly IUrlGeneratorService urlHelper;

        readonly IRouteValueFactory routeValueFactory;

        readonly IEmailSenderService emailSenderService;

        public IdentityUserService(UserManager<User> userManager,
            IIdentityFactory identityFactory,
            IHttpContextWrapper httpContextWrapper,
            IUserRepository userRepository,
            ILoggingService loggingService,
            IUrlGeneratorService urlHelper,
            IRouteValueFactory routeValueFactory,
            IEmailSenderService emailSenderService)
        {
            this.userManager = userManager;
            this.identityFactory = identityFactory;
            this.httpContextWrapper = httpContextWrapper;
            this.userRepository = userRepository;
            this.loggingService = loggingService;
            this.urlHelper = urlHelper;
            this.routeValueFactory = routeValueFactory;
            this.emailSenderService = emailSenderService;
        }

        #endregion

        public async Task<Int32ServiceResult> CreateUserAsync(User user, string password)
        {
            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                return Int32ServiceResult.Success(Messages.AddingNewUserSucceed, user.Id);
            }

            var error = IdentityErrorHelper.GetIdentityError(result.Errors);

            return Int32ServiceResult.Error(error);
        }

        public async Task<ServiceResult> UpdateUserAsync(User user)
        {
            var entityUser = await GetUserAsync();

            entityUser.FirstName = user.FirstName;
            entityUser.LastName = user.LastName;
            entityUser.SvnUserName = user.SvnUserName;

            var result = await userManager.UpdateAsync(entityUser);

            if (result.Succeeded)
            {
                return ServiceResult.Success(Messages.UserUpdateSucceeds);
            }

            return ReturnErrorServiceResult(result.Errors);
        }

        public async Task<ServiceResult> ChangePasswordAsync(string password, string newPassword)
        {
            var user = await GetUserAsync();

            var result = await userManager.ChangePasswordAsync(user, password, newPassword);

            if (result.Succeeded)
            {
                return ServiceResult.Success(Messages.PasswordChangeSucceeds);
            }

            return ReturnErrorServiceResult(result.Errors);
        }

        public async Task<ServiceResult> ResetPasswordAsync(string password, string token)
        {
            var userId = await userRepository.CheckPasswordResetRequestAsync(token);

            if (userId == 0)
            {
                return ServiceResult.Error(Messages.InvalidPasswordRequestToken);
            }

            var user = await userManager.FindByIdAsync(userId.ToString());
            var result = await userManager.ResetPasswordAsync(user, token, password);

            if (result.Succeeded)
            {
                await userRepository.UpdatePasswordResetRequestAsync(user.Id);

                return ServiceResult.Success(Messages.ResetPasswordSucceed);
            }

            return ReturnErrorServiceResult(result.Errors);
        }

        public async Task<ServiceResult> ForgotPasswordAsync(string emailAddress)
        {
            var user = await userManager.FindByEmailAsync(emailAddress);

            if (user == null)
            {
                return ServiceResult.Error(Messages.UserNotFoundByEmailAddress);
            }

            var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
            var ipAddress = httpContextWrapper.GetUserIpAddress();

            bool isSuccess = await userRepository.AddPasswordResetRequestAsync(user.Id, ipAddress, resetToken);

            if (isSuccess)
            {
                var routeValue = routeValueFactory.CreateRouteValuesForToken(resetToken);
                var resetUrl = urlHelper.GenerateUrl(AccountControllerActionNames.ResetPassword, ControllerNames.Account, routeValue);
                emailSenderService.SendForgotPasswordMail(emailAddress, resetUrl);

                return ServiceResult.Success(Messages.PasswordResetRequestSucceeds);
            }

            return ServiceResult.Error(Messages.GeneralError);
        }

        public async Task<ServiceResult> LoginAsync(string emailAddress, string password, bool isRemember)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(emailAddress);

                if (user == null)
                {
                    return ServiceResult.Error(Messages.InvalidUserInformation);
                }

                var result = await userManager.CheckPasswordAsync(user, password);

                if (!result)
                {
                    return ServiceResult.Error(Messages.InvalidUserInformation);
                }

                var userClaims = identityFactory.CreateUserClaims(user);

                await httpContextWrapper.SignInAsync(userClaims, isRemember);
                await AddLoginLog(emailAddress, user.Id);

                return ServiceResult.Success();
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(LoginAsync), Messages.GeneralError, ex);

                return ServiceResult.Error(Messages.GeneralError);
            }
        }

        public async Task<ServiceResult> LogOutAsync()
        {
            try
            {
                await httpContextWrapper.SignOutAsync();

                return ServiceResult.Success();
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(LogOutAsync), Messages.GeneralError, ex);

                return ServiceResult.Error(Messages.GeneralError);
            }
        }

        public async Task<User> GetUserAsync()
        {
            var userClaims = httpContextWrapper.GetCurrentUser();

            return await userManager.GetUserAsync(userClaims);
        }

        public async Task<User> GetUserWithUpdateInfoAsync(int userId)
        {
            var user = await GetUserAsync();
            user.RecordUpdateInfo = await userRepository.GetUserUpdateInfoAsync(userId);

            return user;
        }

        #region Private methods

        private async Task<bool> AddLoginLog(string emailAddress, int userId)
        {
            var userLogOnLog = identityFactory.CreateUserLogOnLog(emailAddress, userId);

            return await userRepository.AddUserLogOnLogAsync(userLogOnLog);
        }

        ServiceResult ReturnErrorServiceResult(IEnumerable<IdentityError> errors)
        {
            var error = IdentityErrorHelper.GetIdentityError(errors);

            return ServiceResult.Error(error);
        }

        #endregion
    }
}