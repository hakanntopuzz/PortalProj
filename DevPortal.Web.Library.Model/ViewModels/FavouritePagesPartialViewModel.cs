using DevPortal.Model;
using System.Collections.Generic;
using System.Linq;

namespace DevPortal.Web.Library.Model
{
    public class FavouritePagesPartialViewModel
    {
        public ICollection<FavouritePage> FavouritePages { get; set; }

        public bool HasFavouritePages
        {
            get
            {
                return this.FavouritePages.Any();
            }
        }
    }
}