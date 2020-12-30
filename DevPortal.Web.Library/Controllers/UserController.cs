using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Identity.Business.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DevPortal.Web.Library.Controllers
{
    [PolicyBasedAuthorize(Policy.Admin)]
    public class UserController : BaseController
    {
        #region ctor

        readonly IIdentityUserService identityUserService;

        readonly IUserService userService;

        readonly IUserViewModelFactory userViewModelFactory;

        readonly IUrlGeneratorService urlHelper;

        readonly IGuidService guidService;

        readonly IRouteValueFactory routeValueFactory;

        public UserController(
            IUserSessionService userSessionService,
            IIdentityUserService identityUserService,
            IUserService userService,
            IUserViewModelFactory viewModelFactory,
            IUrlGeneratorService urlHelper,
            IGuidService guidService,
            IRouteValueFactory routeValueFactory) : base(userSessionService)
        {
            this.identityUserService = identityUserService;
            this.userService = userService;
            this.userViewModelFactory = viewModelFactory;
            this.urlHelper = urlHelper;
            this.guidService = guidService;
            this.routeValueFactory = routeValueFactory;
        }

        #endregion

        #region index

        public IActionResult Index()
        {
            var viewModel = userViewModelFactory.CreateUserListViewModel();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserSearchFilter searchFilter)
        {
            var users = await userService.GetFilteredUsersJqTableAsync(searchFilter);
            return Ok(users);
        }

        #endregion

        #region detail

        public async Task<IActionResult> Detail(int id = 0)
        {
            var user = await userService.GetUserAsync(id);

            if (user == null)
            {
                return UserNotFoundResult();
            }

            var model = userViewModelFactory.CreateUserDetailViewModel(user);

            return View(ViewNames.Detail, model);
        }

        IActionResult UserNotFoundResult()
        {
            SetErrorResultMessageTempData(Messages.UserNotFound);

            return RedirectToAction(UserControllerActionNames.Index, ControllerNames.User);
        }

        #endregion

        #region add

        public async Task<IActionResult> Add()
        {
            return await AddView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return await AddView();
            }

            model.User.RecordUpdateInfo.CreatedBy = CurrentUserId;
            model.User.RecordUpdateInfo.ModifiedBy = CurrentUserId;
            model.User.SecureId = guidService.NewGuidString();

            var result = await identityUserService.CreateUserAsync(model.User, model.Password);

            if (result.IsSuccess)
            {
                SetSuccessResultMessageTempData(result.Message);

                return RedirectToAction(UserControllerActionNames.Detail, ControllerNames.User, new { id = result.Value });
            }

            SetErrorResultMessageTempData(result.Message);

            return await AddView();
        }

        async Task<IActionResult> AddView()
        {
            var userStatus = await userService.GetUserStatusListAsync();
            var userTypes = await userService.GetUserTypeListAsync();
            var viewModel = userViewModelFactory.CreateUserAddViewModel(userStatus, userTypes);

            return View(viewModel);
        }

        #endregion

        #region edit

        public async Task<IActionResult> Edit(int id)
        {
            var user = await userService.GetUserAsync(id);

            if (user == null)
            {
                return UserNotFoundResult();
            }

            var viewModel = await CreateEditUserViewModel(user);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User user)
        {
            if (!ModelState.IsValid)
            {
                return await EditErrorView(user);
            }

            user.RecordUpdateInfo.ModifiedBy = CurrentUserId;

            var updateResult = await userService.UpdateUserAsync(user);

            if (!updateResult.IsSuccess)
            {
                SetErrorResultMessageTempData(updateResult.Message);

                return await EditErrorView(user);
            }

            return EditSuccessView(user.Id, updateResult.Message);
        }

        async Task<EditUserViewModel> CreateEditUserViewModel(User user)
        {
            var userStatus = await userService.GetUserStatusListAsync();
            var userTypes = await userService.GetUserTypeListAsync();

            return userViewModelFactory.CreateEditUserViewModel(user, userStatus, userTypes);
        }

        async Task<IActionResult> EditErrorView(User model)
        {
            var userStatus = await userService.GetUserStatusListAsync();
            var userTypes = await userService.GetUserTypeListAsync();
            var viewModel = userViewModelFactory.CreateEditUserViewModel(model, userStatus, userTypes);

            return View(ViewNames.Edit, viewModel);
        }

        IActionResult EditSuccessView(int id, string message)
        {
            SetSuccessResultMessageTempData(message);

            var routeData = routeValueFactory.CreateRouteValueForId(id);

            return RedirectToAction(UserControllerActionNames.Detail, ControllerNames.User, routeData);
        }

        #endregion

        #region delete

        public JsonResult Delete(int id)
        {
            var result = userService.DeleteUserAsync(id);
            var redirectUrl = urlHelper.GenerateUrl(UserControllerActionNames.Index, ControllerNames.User);

            if (!result.Result.IsSuccess)
            {
                return Json(RedirectableClientActionResult.Error(redirectUrl, result.Result.Message));
            }

            SetSuccessResultMessageTempData(result.Result.Message);

            return Json(RedirectableClientActionResult.Success(redirectUrl, result.Result.Message));
        }

        #endregion
    }
}