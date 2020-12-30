using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Business.Abstract
{
    public interface IFavouritePageService
    {
        ICollection<Link> GetFavouritePageLinks(int userId);

        ICollection<Link> GetFavouritePageLinksByCount(int userId, int take);

        ServiceResult AddFavouritePage(FavouritePage favouritePage);

        ICollection<FavouritePage> GetFavouritePages(int userId);

        bool IsPageInFavourites(int userId, string pageUrl);

        ServiceResult DeleteFavouritePage(int favouriteId);

        FavouritePage GetFavouritePage(int userId, string pageUrl);

        ServiceResult SortFavouritePages(Dictionary<string, List<int>> pageList);
    }
}