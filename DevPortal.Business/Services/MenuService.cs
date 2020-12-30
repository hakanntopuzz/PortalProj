using AB.Framework.Logger.Nlog.Abstract;
using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.Framework;
using DevPortal.Framework.Factories;
using DevPortal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevPortal.Business.Services
{
    public class MenuService : IMenuService
    {
        #region ctor

        readonly IMenuRepository menuRepository;

        readonly IAuditService auditService;

        readonly IAuditFactory auditFactory;

        readonly ILoggingService loggingService;

        public MenuService(
            IMenuRepository menuRepository,
            IAuditService auditService,
            IAuditFactory auditFactory,
            ILoggingService loggingService)
        {
            this.menuRepository = menuRepository;
            this.auditService = auditService;
            this.auditFactory = auditFactory;
            this.loggingService = loggingService;
        }

        #endregion

        #region get filtered menu list async

        public async Task<IEnumerable<MenuModel>> GetFilteredMenuListAsync(MenuTableParam tableParam)
        {
            return await menuRepository.GetFilteredMenuListAsync(tableParam);
        }

        #endregion

        #region get filtered menu list as jqTable async

        public async Task<JQTable> GetFilteredMenuListAsJqTableAsync(MenuTableParam tableParam)
        {
            var data = await menuRepository.GetFilteredMenuListAsync(tableParam);
            int recordCount = data.Any() ? data.First().TotalCount : 0;

            return new JQTable
            {
                data = data,
                recordsFiltered = recordCount,
                recordsTotal = recordCount
            };
        }

        #endregion

        #region get menu list async

        public async Task<IEnumerable<MenuModel>> GetMenuListAsync()
        {
            return await menuRepository.GetMenuListAsync();
        }

        #endregion

        #region add menu async

        public async Task<ServiceResult> AddMenuAsync(MenuModel menu)
        {
            if (menu == null)
            {
                return ServiceResult.Error(Messages.NullParameterError);
            }

            var addSuccess = await menuRepository.AddMenuAsync(menu);

            if (!addSuccess)
            {
                return ServiceResult.Error(Messages.AddingFails);
            }

            return ServiceResult.Success(Messages.MenuCreated);
        }

        #endregion

        #region get menu async

        public async Task<MenuModel> GetMenuAsync(int id)
        {
            var menu = await menuRepository.GetMenuAsync(id);

            if (menu == null)
            {
                return null;
            }

            menu.RecordUpdateInfo = await menuRepository.GetMenuUpdateInfoAsync(id);

            return menu;
        }

        #endregion

        #region get sub menu list async

        public async Task<IEnumerable<MenuModel>> GetSubMenuListAsync(int id)
        {
            return await menuRepository.GetSubMenuListAsync(id);
        }

        #endregion

        #region update menu async

        public async Task<ServiceResult> UpdateMenuAsync(MenuModel menu)
        {
            if (menu == null)
            {
                return ServiceResult.Error(Messages.NullParameterError);
            }

            var oldMenu = await menuRepository.GetMenuAsync(menu.Id);

            var isChanged = auditService.IsChanged(oldMenu, menu, nameof(BaseMenu));

            if (!isChanged)
            {
                return ServiceResult.Success(Messages.MenuUpdated);
            }

            try
            {
                using (var scope = TransactionScopeFactory.CreateTransactionScope())
                {
                    UpdateMenuCore(menu);

                    var newMenu = await menuRepository.GetMenuAsync(menu.Id);

                    await AddAuditCore(newMenu, oldMenu, menu.RecordUpdateInfo.ModifiedBy);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(UpdateMenuAsync), "Menü güncelleme sırasında beklenmeyen bir hata oluştu.", ex);

                return ServiceResult.Error(Messages.UpdateFails);
            }

            return ServiceResult.Success(Messages.MenuUpdated);
        }

        void UpdateMenuCore(MenuModel menu)
        {
            var isSuccess = menuRepository.UpdateMenuAsync(menu).Result;

            if (!isSuccess)
            {
                throw new TransactionIstopException("Menü güncelleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        async Task AddAuditCore(MenuModel menu, MenuModel oldMenu, int userId)
        {
            var auditInfo = auditFactory.CreateAuditInfo(nameof(Menu), menu.Id, oldMenu, menu, userId);
            var isSuccess = await auditService.AddAsync(auditInfo);

            if (!isSuccess)
            {
                throw new TransactionIstopException("Audit ekleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        #endregion

        #region get menu list as parent child

        public ICollection<Menu> GetMenuListAsParentChild()
        {
            var menuList = GetMenuListAsync().Result;

            return GetParentMenuWithChildren(null, menuList);
        }

        List<Menu> GetParentMenuWithChildren(int? parentId, IEnumerable<MenuModel> menuList)
        {
            return menuList.Where(x => x.ParentId == parentId).OrderBy(x => x.Order).Select(menu =>
            {
                return new Menu
                {
                    Name = menu.Name,
                    Link = menu.Link,
                    Group = menu.MenuGroupId,
                    Icon = menu.Icon,
                    Children = GetParentMenuWithChildren(menu.Id, menuList)
                };
            }).ToList();
        }

        #endregion

        #region delete menu async

        public async Task<ServiceResult> DeleteMenuAsync(int menuId)
        {
            var isSuccess = await menuRepository.DeleteMenuAsync(menuId);

            if (!isSuccess)
            {
                return ServiceResult.Error(Messages.DeleteFails);
            }

            return ServiceResult.Success(Messages.MenuDeleted);
        }

        #endregion

        #region get menu groups async

        public async Task<IEnumerable<MenuGroup>> GetMenuGroupsAsync()
        {
            return await menuRepository.GetMenuGroupsAsync();
        }

        #endregion
    }
}