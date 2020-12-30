using DevPortal.Model;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Abstract
{
    public interface IMenuViewModelFactory
    {
        MenuListViewModel CreateMenuListViewModel();

        AddMenuViewModel CreateAddMenuViewModel(IEnumerable<MenuModel> allMenu, IEnumerable<MenuGroup> menuGroups);

        MenuDetailViewModel CreateMenuDetailViewModel(MenuModel menu, IEnumerable<MenuModel> subMenuList);

        EditMenuViewModel CreateEditMenuViewModel(MenuModel menu, IEnumerable<MenuModel> allMenu, IEnumerable<MenuGroup> menuGroups);
    }
}