using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Model;
using System.Collections.Generic;
using System.Linq;

namespace DevPortal.Business.Services
{
    public class FavouritePageService : IFavouritePageService
    {
        #region ctor

        readonly IFavouritePageRepository favouritePageRepository;

        public FavouritePageService(IFavouritePageRepository favouritePageRepository)
        {
            this.favouritePageRepository = favouritePageRepository;
        }

        #endregion

        #region get favourite pages by user id

        public ICollection<Link> GetFavouritePageLinks(int userId)
        {
            return favouritePageRepository.GetFavouritePageLinksByUserId(userId);
        }

        #endregion

        #region get favourite page by count

        public ICollection<Link> GetFavouritePageLinksByCount(int userId, int take)
        {
            return favouritePageRepository.GetFavouritePageLinksByUserId(userId).Take(take).ToList();
        }

        #endregion

        #region add favourite page

        public ServiceResult AddFavouritePage(FavouritePage favouritePage)
        {
            var isPageInFavourites = IsPageInFavourites(favouritePage.UserId, favouritePage.PageUrl);

            if (isPageInFavourites)
            {
                return ServiceResult.Error(Messages.FavoriteExists);
            }

            SetFavoritePageOrder(favouritePage);

            var isSuccess = favouritePageRepository.AddFavouritePage(favouritePage);

            if (!isSuccess)
            {
                return ServiceResult.Error(Messages.AddingFails);
            }

            return ServiceResult.Success(Messages.FavoriteAdded);
        }

        void SetFavoritePageOrder(FavouritePage favouritePage)
        {
            var currentMaxOrder = GetLargestFavouritePageOrderByUserId(favouritePage.UserId);
            currentMaxOrder++;
            favouritePage.Order = currentMaxOrder;
        }

        int GetLargestFavouritePageOrderByUserId(int userId)
        {
            return favouritePageRepository.GetLargestFavouritePageOrderByUserId(userId);
        }

        #endregion

        #region get favourite pages

        public ICollection<FavouritePage> GetFavouritePages(int userId)
        {
            return favouritePageRepository.GetFavouritePages(userId);
        }

        #endregion

        #region delete favourite page

        public ServiceResult DeleteFavouritePage(int favouriteId)
        {
            var isSuccess = favouritePageRepository.DeleteFavouritePage(favouriteId);

            if (!isSuccess)
            {
                return ServiceResult.Error(Messages.DeleteFails);
            }

            return ServiceResult.Success(Messages.FavoriteDeleted);
        }

        #endregion

        #region is page in favourites

        public bool IsPageInFavourites(int userId, string pageUrl)
        {
            return favouritePageRepository.IsPageInFavourites(userId, pageUrl);
        }

        #endregion

        public FavouritePage GetFavouritePage(int userId, string pageUrl)
        {
            return favouritePageRepository.GetFavouritePage(userId, pageUrl);
        }

        public ServiceResult SortFavouritePages(Dictionary<string, List<int>> pageList)
        {
            var isSuccess = favouritePageRepository.SortFavouritePages(pageList["item"]);

            if (!isSuccess)
            {
                return ServiceResult.Error(Messages.SortFails);
            }

            return ServiceResult.Success(Messages.FavoritePageSorted);
        }
    }
}