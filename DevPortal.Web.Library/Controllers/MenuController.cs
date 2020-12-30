using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.Web.Library.Controllers
{
    public class MenuController : BaseController
    {
        #region ctor

        readonly IMenuService menuService;

        readonly IMenuViewModelFactory menuViewModelFactory;

        readonly IUrlGeneratorService urlHelper;

        public MenuController(
            IUserSessionService userSessionService,
            IMenuService menuService,
            IMenuViewModelFactory viewModelFactory,
            IUrlGeneratorService urlHelper) : base(userSessionService)
        {
            this.menuService = menuService;
            this.menuViewModelFactory = viewModelFactory;
            this.urlHelper = urlHelper;
        }

        #endregion

        #region index

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public IActionResult Index()
        {
            var viewModel = menuViewModelFactory.CreateMenuListViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]

        //TODO: ValidateAntiforgeryToken eklenmeli.
        public async Task<IActionResult> Index(MenuTableParam tableParam)
        {
            var jqTable = await menuService.GetFilteredMenuListAsJqTableAsync(tableParam);

            return Ok(jqTable);
        }

        #endregion

        #region add

        [PolicyBasedAuthorize(Policy.Admin)]
        public async Task<IActionResult> Add()
        {
            var viewModel = await CreateAddMenuViewModelAsync();

            return View(viewModel);
        }

        [HttpPost]
        [PolicyBasedAuthorize(Policy.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(MenuModel menu)
        {
            if (!ModelState.IsValid)
            {
                return await AddErrorViewAsync(menu);
            }

            SetUserInfoForAdd(menu);

            var addResult = await menuService.AddMenuAsync(menu);

            if (!addResult.IsSuccess)
            {
                SetErrorResultMessageTempData(addResult.Message);

                return await AddErrorViewAsync(menu);
            }

            return AddSuccessView(addResult.Message);
        }

        void SetUserInfoForAdd(MenuModel menu)
        {
            int userId = CurrentUserId;
            menu.RecordUpdateInfo.CreatedBy = userId;
            menu.RecordUpdateInfo.ModifiedBy = userId;
        }

        async Task<AddMenuViewModel> CreateAddMenuViewModelAsync()
        {
            var allMenu = await menuService.GetMenuListAsync();

            var menuGroups = await menuService.GetMenuGroupsAsync();

            return menuViewModelFactory.CreateAddMenuViewModel(allMenu, menuGroups);
        }

        async Task<IActionResult> AddErrorViewAsync(MenuModel model)
        {
            var viewModel = await CreateAddMenuViewModelAsync();
            viewModel.Menu = model;

            return View(ViewNames.Add, viewModel);
        }

        IActionResult AddSuccessView(string message)
        {
            SetSuccessResultMessageTempData(message);

            return RedirectToAction(ApplicationControllerActionNames.Index, ControllerNames.Menu);
        }

        #endregion

        #region detail

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public async Task<IActionResult> Detail(int id = 0)
        {
            var menu = await menuService.GetMenuAsync(id);

            if (menu == null)
            {
                return MenuNotFoundResult();
            }

            var subMenuList = await menuService.GetSubMenuListAsync(id);

            var model = menuViewModelFactory.CreateMenuDetailViewModel(menu, subMenuList);

            return View(ViewNames.Detail, model);
        }

        IActionResult MenuNotFoundResult()
        {
            SetErrorResultMessageTempData(Messages.MenuNotFound);

            return RedirectToAction(ApplicationGroupControllerActionNames.Index, ControllerNames.Menu);
        }

        #endregion

        #region edit

        [PolicyBasedAuthorize(Policy.Admin)]
        public async Task<IActionResult> Edit(int id)
        {
            var menu = await menuService.GetMenuAsync(id);

            if (menu == null)
            {
                return MenuNotFoundResult();
            }

            var viewModel = await CreateEditMenuViewModelAsync(menu);

            return View(viewModel);
        }

        [HttpPost]
        [PolicyBasedAuthorize(Policy.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MenuModel menu)
        {
            if (!ModelState.IsValid)
            {
                return await AddErrorViewAsync(menu);
            }

            SetUserInfoForUpdate(menu);

            var updateResult = await menuService.UpdateMenuAsync(menu);

            if (!updateResult.IsSuccess)
            {
                SetErrorResultMessageTempData(updateResult.Message);

                return await UpdateErrorViewAsync(menu);
            }

            return AddSuccessView(updateResult.Message);
        }

        async Task<EditMenuViewModel> CreateEditMenuViewModelAsync(MenuModel menu)
        {
            var allMenu = await menuService.GetMenuListAsync();
            var menuGroups = await menuService.GetMenuGroupsAsync();

            return menuViewModelFactory.CreateEditMenuViewModel(menu, allMenu, menuGroups);
        }

        void SetUserInfoForUpdate(MenuModel menu)
        {
            menu.RecordUpdateInfo.ModifiedBy = CurrentUserId;
        }

        async Task<IActionResult> UpdateErrorViewAsync(MenuModel menu)
        {
            var viewModel = await CreateEditMenuViewModelAsync(menu);
            viewModel.Menu = menu;

            return View(ViewNames.Edit, viewModel);
        }

        #endregion

        #region delete

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public async Task<JsonResult> Delete(int id)
        {
            var deleteResult = await menuService.DeleteMenuAsync(id);
            var redirectUrl = urlHelper.GenerateUrl(ApplicationControllerActionNames.Index, ControllerNames.Menu);

            if (!deleteResult.IsSuccess)
            {
                return Json(RedirectableClientActionResult.Error(redirectUrl, deleteResult.Message));
            }

            SetSuccessResultMessageTempData(deleteResult.Message);

            return Json(RedirectableClientActionResult.Success(redirectUrl, deleteResult.Message));
        }

        #endregion

        #region get menu for jenkins and nuget module

        [HttpGet]
        [Route("/get-menu")]
        public IEnumerable<Menu> MenuList()
        {
            return menuService.GetMenuListAsParentChild();
        }

        #endregion
    }
}