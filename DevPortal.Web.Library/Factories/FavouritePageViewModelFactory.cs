using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using System;
using System.Collections.Generic;

namespace DevPortal.Web.Library
{
    public class FavouritePageViewModelFactory : IFavouritePageViewModelFactory
    {
        #region ctor

        readonly IBreadCrumbFactory breadCrumbFactory;

        public FavouritePageViewModelFactory(IBreadCrumbFactory breadCrumbFactory)
        {
            this.breadCrumbFactory = breadCrumbFactory;
        }

        #endregion

        public FavouritePagesViewModel CreateFavouritePagesViewModel(ICollection<FavouritePage> favouritePages)
        {
            var partialModel = new FavouritePagesPartialViewModel
            {
                FavouritePages = favouritePages
            };

            return new FavouritePagesViewModel
            {
                FavouritePagesPartialModel = partialModel,
                BreadCrumbViewModel = breadCrumbFactory.CreateFavouritePagesModel()
            };
        }

        public FavouritePagesPartialViewModel CreateFavouritePagesPartialViewModel(ICollection<FavouritePage> favouritePages)
        {
            return new FavouritePagesPartialViewModel
            {
                FavouritePages = favouritePages
            };
        }

        public FavouritePage CreateAddFavouritePageModel(string pageName, Uri pageUrl, int userId)
        {
            if (pageUrl == null)
            {
                return null;
            }

            return new FavouritePage
            {
                PageName = pageName,
                PageUrl = pageUrl.ToString(),
                UserId = userId
            };
        }
    }
}