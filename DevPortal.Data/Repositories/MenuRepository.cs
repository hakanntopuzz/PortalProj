using AB.Data.DapperClient.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Factories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.Data.Repositories
{
    public class MenuRepository : BaseDevPortalRepository, IMenuRepository
    {
        #region ctor

        readonly IDataClient dataClient;

        public MenuRepository(
            IDataClient dataClient,
            IApplicationDataRequestFactory applicationDataRequestFactory,
            ISettings settings)
            : base(dataClient, applicationDataRequestFactory, settings)
        {
            this.dataClient = dataClient;
        }

        #endregion

        public async Task<ICollection<MenuModel>> GetFilteredMenuListAsync(MenuTableParam tableParam)
        {
            var dataRequest = dataRequestFactory.GetFilteredMenuList(tableParam);
            var defaultReturnValue = new List<MenuModel>();

            return await dataClient.GetCollectionAsync<MenuModel, RecordUpdateInfo, MenuModel>(
                dataRequest,
                DataClientMapFactory.MenusMap,
                defaultReturnValue,
                dataRequest.SplitOnParameters);
        }

        public async Task<ICollection<MenuModel>> GetMenuListAsync()
        {
            var dataRequest = dataRequestFactory.GetMenuList();
            var defaultReturnValue = new List<MenuModel>();

            return await dataClient.GetCollectionAsync(dataRequest, defaultReturnValue);
        }

        public async Task<bool> AddMenuAsync(MenuModel menu)
        {
            var dataRequest = dataRequestFactory.AddMenu(menu);
            const bool defaultReturnValue = false;

            return await dataClient.GetScalarAsync(dataRequest, defaultReturnValue);
        }

        public async Task<MenuModel> GetMenuAsync(int id)
        {
            var dataRequest = dataRequestFactory.GetMenu(id);
            const MenuModel defaultReturnValue = null;

            return await dataClient.GetItemAsync(dataRequest, defaultReturnValue);
        }

        public async Task<IEnumerable<MenuModel>> GetSubMenuListAsync(int id)
        {
            var dataRequest = dataRequestFactory.GetSubMenuList(id);
            ICollection<MenuModel> defaultReturnValue = new List<MenuModel>();

            return await dataClient.GetCollectionAsync(dataRequest, defaultReturnValue);
        }

        public async Task<bool> UpdateMenuAsync(MenuModel menu)
        {
            var dataRequest = dataRequestFactory.UpdateMenu(menu);
            const bool defaultReturnValue = false;

            return await dataClient.GetScalarAsync(dataRequest, defaultReturnValue);
        }

        public async Task<bool> DeleteMenuAsync(int menuId)
        {
            var dataRequest = dataRequestFactory.DeleteMenu(menuId);
            const bool defaultReturnValue = false;

            return await dataClient.GetScalarAsync(dataRequest, defaultReturnValue);
        }

        public async Task<ICollection<MenuGroup>> GetMenuGroupsAsync()
        {
            var dataRequest = dataRequestFactory.GetMenuGroups();
            var defaultReturnValue = new List<MenuGroup>();

            return await dataClient.GetCollectionAsync(dataRequest, defaultReturnValue);
        }

        public async Task<RecordUpdateInfo> GetMenuUpdateInfoAsync(int id)
        {
            var dataRequest = dataRequestFactory.GetMenuUpdateInfo(id);

            const RecordUpdateInfo defaultReturnValue = null;

            return await dataClient.GetItemAsync(dataRequest, defaultReturnValue);
        }
    }
}