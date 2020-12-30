namespace DevPortal.Web.Library.Model
{
    public class BreadCrumbModel
    {
        public string PageName { get; set; }

        public string PageUrl { get; set; }

        public bool HasUrl
        {
            get
            {
                return !string.IsNullOrEmpty(PageUrl);
            }
        }

        #region factory methods

        public static BreadCrumbModel Create(string pageName, string pageUrl)
        {
            return new BreadCrumbModel
            {
                PageName = pageName,
                PageUrl = pageUrl
            };
        }

        public static BreadCrumbModel Create(string pageName)
        {
            return new BreadCrumbModel
            {
                PageName = pageName
            };
        }

        #endregion
    }
}