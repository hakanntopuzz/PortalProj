using AB.Data.DapperClient.Abstract;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Repositories
{
    public class FavouritePageRepository : BaseDevPortalRepository, IFavouritePageRepository
    {
        #region ctor

        readonly IDataClient dataClient;

        public FavouritePageRepository(
            IDataClient dataClient,
            IApplicationDataRequestFactory applicationDataRequestFactory,
            ISettings settings)
            : base(dataClient, applicationDataRequestFactory, settings)
        {
            this.dataClient = dataClient;
        }

        #endregion

        public ICollection<Link> GetFavouritePageLinksByUserId(int userId)
        {
            var dataRequest = dataRequestFactory.GetFavouritePagesByUserId(userId);

            var defaultReturnValue = new List<Link>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public bool AddFavouritePage(FavouritePage favouritePage)
        {
            var dataRequest = dataRequestFactory.AddFavouritePage(favouritePage);

            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public ICollection<FavouritePage> GetFavouritePages(int userId)
        {
            var dataRequest = dataRequestFactory.GetFavouritePages(userId);

            var defaultReturnValue = new List<FavouritePage>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public int GetLargestFavouritePageOrderByUserId(int userId)
        {
            var dataRequest = dataRequestFactory.GetLargestFavouritePageOrderByUserId(userId);

            const int defaultReturnValue = 0;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public bool IsPageInFavourites(int userId, string pageUrl)
        {
            var dataRequest = dataRequestFactory.IsPageInFavourites(userId, pageUrl);

            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public bool DeleteFavouritePage(int favouriteId)
        {
            var dataRequest = dataRequestFactory.DeleteFavouritePage(favouriteId);

            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public FavouritePage GetFavouritePage(int userId, string pageUrl)
        {
            var dataRequest = dataRequestFactory.GetFavouritePage(userId, pageUrl);

            const FavouritePage defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public bool SortFavouritePages(List<int> pageIdList)
        {
            var dataRequest = dataRequestFactory.SortFavouritePages(pageIdList);

            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }
    }
}