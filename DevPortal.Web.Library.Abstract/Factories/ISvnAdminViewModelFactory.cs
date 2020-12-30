using DevPortal.SvnAdmin.Model;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Abstract
{
    public interface ISvnAdminViewModelFactory
    {
        #region repository list

        SvnRepositoryListViewModel CreateSvnRepositoryListViewModel(ICollection<SvnRepositoryFolderListItem> svnRepositoryListItems);

        SvnRepositoryFolderViewModel CreateSvnRepositoryFolderViewModel(SvnRepositoryFolder folder);

        #endregion
    }
}