namespace DevPortal.Web.Library.Model
{
    public class FavouritesControllerActionNames : BaseActionNames
    {
        public static string GetFavouritePageByUserIdAndPageUrl => SetActionName(nameof(GetFavouritePageByUserIdAndPageUrl));

        public static string SortPages => SetActionName(nameof(SortPages));

        public static string List => SetActionName(nameof(List));
    }
}