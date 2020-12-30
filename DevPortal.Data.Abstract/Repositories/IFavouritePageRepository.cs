using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Abstract.Repositories
{
    public interface IFavouritePageRepository
    {
        ICollection<Link> GetFavouritePageLinksByUserId(int userId);

        bool AddFavouritePage(FavouritePage favouritePage);

        ICollection<FavouritePage> GetFavouritePages(int userId);

        int GetLargestFavouritePageOrderByUserId(int userId);

        bool IsPageInFavourites(int userId, string pageUrl);

        bool DeleteFavouritePage(int favouriteId);

        FavouritePage GetFavouritePage(int userId, string pageUrl);

        bool SortFavouritePages(List<int> pageIdList);
    }
}