using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;
using System.Linq;

namespace DevPortal.Web.Library
{
    public class MenuViewModelFactory : IMenuViewModelFactory
    {
        #region ctor

        readonly IBreadCrumbFactory breadCrumbFactory;

        readonly IAuthorizationServiceWrapper authorizationWrapper;

        public MenuViewModelFactory(
            IBreadCrumbFactory breadCrumbFactory,
            IAuthorizationServiceWrapper authorizationWrapper)
        {
            this.breadCrumbFactory = breadCrumbFactory;
            this.authorizationWrapper = authorizationWrapper;
        }

        #endregion

        #region private methods

        bool CheckUserHasAdminPolicy()
        {
            return authorizationWrapper.CheckUserHasAdminPolicy();
        }

        #endregion

        public MenuListViewModel CreateMenuListViewModel()
        {
            var isAuthorized = CheckUserHasAdminPolicy();
            var breadCrumbViewModel = breadCrumbFactory.CreateMenuListModel();

            return new MenuListViewModel
            {
                IsAuthorized = isAuthorized,
                BreadCrumbViewModel = breadCrumbViewModel
            };
        }

        public AddMenuViewModel CreateAddMenuViewModel(IEnumerable<MenuModel> allMenu, IEnumerable<MenuGroup> menuGroups)
        {
            return new AddMenuViewModel
            {
                Menu = new MenuModel(),
                MenuList = allMenu,
                BreadCrumbViewModel = breadCrumbFactory.CreateAddMenuModel(),
                MenuGroups = menuGroups
            };
        }

        public MenuDetailViewModel CreateMenuDetailViewModel(MenuModel menu, IEnumerable<MenuModel> subMenuList)
        {
            var isAuthorized = CheckUserHasAdminPolicy();
            var breadCrumbViewModel = breadCrumbFactory.CreateDetailMenuModel();

            return new MenuDetailViewModel
            {
                Menu = menu,
                SubMenuList = subMenuList.ToList(),
                IsAuthorized = isAuthorized,
                BreadCrumbViewModel = breadCrumbViewModel
            };
        }

        public EditMenuViewModel CreateEditMenuViewModel(MenuModel menu, IEnumerable<MenuModel> allMenu, IEnumerable<MenuGroup> menuGroups)
        {
            return new EditMenuViewModel
            {
                Menu = menu,
                MenuList = allMenu,
                BreadCrumbViewModel = breadCrumbFactory.CreateEditMenuModel(menu.Id),
                MenuGroups = menuGroups
            };
        }
    }
}