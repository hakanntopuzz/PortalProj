using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Model
{
    public class AddMenuViewModel
    {
        public BreadCrumbViewModel BreadCrumbViewModel { get; set; }

        public MenuModel Menu { get; set; }

        public IEnumerable<MenuModel> MenuList { get; set; }

        public IEnumerable<MenuGroup> MenuGroups { get; set; }
    }
}