using DevPortal.Framework.Abstract;
using DevPortal.SvnAdmin.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library
{
    public class SvnAdminViewModelFactory : ISvnAdminViewModelFactory
    {
        #region ctor

        readonly IBreadCrumbFactory breadCrumbFactory;

        readonly IAuthorizationServiceWrapper authorizationWrapper;

        public SvnAdminViewModelFactory(
            IBreadCrumbFactory breadCrumbFactory,
            IAuthorizationServiceWrapper authorizationWrapper)
        {
            this.breadCrumbFactory = breadCrumbFactory;
            this.authorizationWrapper = authorizationWrapper;
        }

        #endregion

        #region private methods

        bool CheckUserHasAdminDeveloperPolicy()
        {
            return authorizationWrapper.CheckUserHasAdminDeveloperPolicy();
        }

        #endregion

        #region svn repository list

        public SvnRepositoryListViewModel CreateSvnRepositoryListViewModel(ICollection<SvnRepositoryFolderListItem> svnRepositoryListItems)
        {
            var isAuthorized = CheckUserHasAdminDeveloperPolicy();
            var breadCrumbViewModel = breadCrumbFactory.CreateSvnAdminListModel();

            return new SvnRepositoryListViewModel
            {
                Items = svnRepositoryListItems,
                IsAuthorized = isAuthorized,
                BreadCrumbViewModel = breadCrumbViewModel
            };
        }

        #endregion

        #region svn repository folder

        public SvnRepositoryFolderViewModel CreateSvnRepositoryFolderViewModel(SvnRepositoryFolder folder)
        {
            var isAuthorized = CheckUserHasAdminDeveloperPolicy();
            var breadCrumbViewModel = breadCrumbFactory.CreateSvnRepositoryFolderModel();

            return new SvnRepositoryFolderViewModel
            {
                Folder = folder,
                IsAuthorized = isAuthorized,
                BreadCrumbViewModel = breadCrumbViewModel
            };
        }

        #endregion
    }
}