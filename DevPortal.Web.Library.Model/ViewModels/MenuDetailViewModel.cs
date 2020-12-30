using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Model
{
    public class MenuDetailViewModel : AuthorizedBaseViewModel
    {
        public MenuModel Menu { get; set; }

        public ICollection<MenuModel> SubMenuList { get; set; }

        public ICollection<Audit> AuditList { get; set; }
    }
}