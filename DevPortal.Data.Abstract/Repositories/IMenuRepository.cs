using DevPortal.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.Data.Abstract
{
    public interface IMenuRepository
    {
        Task<ICollection<MenuModel>> GetFilteredMenuListAsync(MenuTableParam tableParam);

        Task<ICollection<MenuModel>> GetMenuListAsync();

        Task<bool> AddMenuAsync(MenuModel menu);

        Task<MenuModel> GetMenuAsync(int id);

        Task<IEnumerable<MenuModel>> GetSubMenuListAsync(int id);

        Task<bool> UpdateMenuAsync(MenuModel menu);

        Task<bool> DeleteMenuAsync(int menuId);

        Task<ICollection<MenuGroup>> GetMenuGroupsAsync();

        Task<RecordUpdateInfo> GetMenuUpdateInfoAsync(int id);
    }
}