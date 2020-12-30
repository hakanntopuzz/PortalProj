using System.Collections.Generic;

namespace DevPortal.Web.Library.Model
{
    public class BreadCrumbViewModel
    {
        public string ModuleName { get; set; }

        public List<BreadCrumbModel> PageList { get; set; }

        public BreadCrumbViewModel()
        {
            this.PageList = new List<BreadCrumbModel>();
        }
    }
}