using DevPortal.SvnAdmin.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Model
{
    public class SvnRepositoryListViewModel : AuthorizedBaseViewModel
    {
        public ICollection<SvnRepositoryFolderListItem> Items { get; set; }
    }
}