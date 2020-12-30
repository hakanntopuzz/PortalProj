using DevPortal.Model;
using DevPortal.Web.Library.Model;
using System;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Abstract
{
    public interface IFavouritePageViewModelFactory
    {
        FavouritePagesViewModel CreateFavouritePagesViewModel(ICollection<FavouritePage> favouritePages);

        FavouritePagesPartialViewModel CreateFavouritePagesPartialViewModel(ICollection<FavouritePage> favouritePages);

        FavouritePage CreateAddFavouritePageModel(string pageName, Uri pageUrl, int userId);
    }
}