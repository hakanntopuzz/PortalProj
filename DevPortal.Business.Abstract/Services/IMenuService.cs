using DevPortal.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.Business.Abstract
{
    public interface IMenuService
    {
        Task<IEnumerable<MenuModel>> GetFilteredMenuListAsync(MenuTableParam tableParam);

        Task<JQTable> GetFilteredMenuListAsJqTableAsync(MenuTableParam tableParam);

        Task<IEnumerable<MenuModel>> GetMenuListAsync();

        Task<ServiceResult> AddMenuAsync(MenuModel menu);

        Task<MenuModel> GetMenuAsync(int id);

        Task<IEnumerable<MenuModel>> GetSubMenuListAsync(int id);

        Task<ServiceResult> UpdateMenuAsync(MenuModel menu);

        ICollection<Menu> GetMenuListAsParentChild();

        Task<ServiceResult> DeleteMenuAsync(int menuId);

        Task<IEnumerable<MenuGroup>> GetMenuGroupsAsync();
    }
}