using DevPortal.Framework.Abstract;
using DevPortal.Identity.Business.Abstract;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DevPortal.Web.Library.Controllers
{
    public class AccountController : BaseController
    {
        #region ctor

        readonly IIdentityUserService identityUserService;

        readonly IUserViewModelFactory userViewModelFactory;

        public AccountController(
            IUserSessionService userSessionService,
            IIdentityUserService identityUserService,
            IUserViewModelFactory userViewModelFactory) : base(userSessionService)
        {
            this.identityUserService = identityUserService;
            this.userViewModelFactory = userViewModelFactory;
        }

        #endregion

        #region login

        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            var model = userViewModelFactory.CreateLoginViewModel(returnUrl);

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await identityUserService.LoginAsync(model.EmailAddress, model.Password, model.IsRemember);

            if (result.IsSuccess)
            {
                return RedirectUrlToPage(model.ReturnUrl);
            }

            SetErrorResultMessageTempData(result.Message);

            return View(model);
        }

        IActionResult RedirectUrlToPage(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction(HomeControllerActionNames.Index, ControllerNames.Home);
            }

            return Redirect(returnUrl);
        }

        #endregion

        #region logout

        public async Task<IActionResult> LogOut()
        {
            var result = await identityUserService.LogOutAsync();

            if (result.IsSuccess)
            {
                return RedirectToAction(AccountControllerActionNames.Login, ControllerNames.Account);
            }

            SetErrorResultMessageTempData(result.Message);

            return RedirectToAction(HomeControllerActionNames.Index, ControllerNames.Home);
        }

        #endregion

        #region profile

        public async Task<IActionResult> Profile()
        {
            return await UserDetailPageViewAsync();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(UserDetailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await identityUserService.UpdateUserAsync(model.User);

            if (result.IsSuccess)
            {
                SetSuccessResultMessageTempData(result.Message);
                return await UserDetailPageViewAsync();
            }

            SetErrorResultMessageTempData(result.Message);

            return await UserDetailPageViewAsync();
        }

        async Task<IActionResult> UserDetailPageViewAsync()
        {
            var user = await identityUserService.GetUserWithUpdateInfoAsync(CurrentUserId);
            var model = userViewModelFactory.CreateUserProfileViewModel(user);

            return View(model);
        }

        #endregion

        #region password

        public IActionResult Password()
        {
            return PasswordView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Password(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await identityUserService.ChangePasswordAsync(model.Password, model.NewPassword);

            if (result.IsSuccess)
            {
                SetSuccessResultMessageTempData(result.Message);

                return PasswordView();
            }

            SetErrorResultMessageTempData(result.Message);

            return PasswordView();
        }

        IActionResult PasswordView()
        {
            var model = userViewModelFactory.CreateChangePasswordViewModel();

            return View(model);
        }

        #endregion

        #region forgot password

        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return ForgotPasswordView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await identityUserService.ForgotPasswordAsync(model.EmailAddress);

            if (result.IsSuccess)
            {
                SetSuccessResultMessageTempData(result.Message);

                return ForgotPasswordView();
            }

            SetErrorResultMessageTempData(result.Message);

            return ForgotPasswordView();
        }

        IActionResult ForgotPasswordView()
        {
            var model = userViewModelFactory.CreateForgotPasswordViewModel();

            return View(model);
        }

        #endregion

        #region reset password

        [AllowAnonymous]
        public IActionResult ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(HomeControllerActionNames.Index, ControllerNames.Home);
            }

            return ResetPasswordView(token);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await identityUserService.ResetPasswordAsync(model.NewPassword, model.Token);

            if (result.IsSuccess)
            {
                SetSuccessResultMessageTempData(result.Message);

                return RedirectToAction(AccountControllerActionNames.Login, ControllerNames.Account);
            }

            SetErrorResultMessageTempData(result.Message);

            return ResetPasswordView(model.Token);
        }

        IActionResult ResetPasswordView(string token)
        {
            var model = userViewModelFactory.CreateResetPasswordViewModel(token);

            return View(model);
        }

        #endregion

        #region get user identity name

        [Route("account/get-user-identity-name")]
        public string GetUserIdentityName()
        {
            return User.Identity.Name;
        }

        #endregion
    }
}