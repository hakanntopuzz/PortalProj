using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Factories
{
    public class ApplicationSvnViewModelFactory : IApplicationSvnViewModelFactory
    {
        #region ctor

        readonly IBreadCrumbFactory breadCrumbFactory;

        readonly IAuthorizationServiceWrapper authorizationWrapper;

        public ApplicationSvnViewModelFactory(
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

        #region application svn repositories

        public ApplicationSvnViewModel CreateApplicationSvnViewModel(int applicationId, string applicationName, string svnUrl, ICollection<SvnRepositoryType> repositoryTypeList)
        {
            return new ApplicationSvnViewModel
            {
                ApplicationSvn = new SvnRepository
                {
                    ApplicationId = applicationId,
                    SvnUrl = svnUrl,
                    ApplicationName = applicationName
                },
                SvnRepositoryTypeList = repositoryTypeList,
                BreadCrumbViewModel = breadCrumbFactory.CreateNewSvnRepositoryModel(applicationId)
            };
        }

        public ApplicationSvnViewModel CreateApplicationSvnDetailViewModel(SvnRepository svnRepository)
        {
            var isAuthorized = CheckUserHasAdminDeveloperPolicy();
            var breadCrumbViewModel = breadCrumbFactory.CreateDetailSvnRepositoryModel(svnRepository.ApplicationId);
            var favouritePageName = $"{svnRepository.ApplicationName} - SVN Deposu - {svnRepository.Name}";

            return new ApplicationSvnViewModel
            {
                ApplicationSvn = svnRepository,
                IsAuthorized = isAuthorized,
                BreadCrumbViewModel = breadCrumbViewModel,
                FavouritePageName = favouritePageName
            };
        }

        public ApplicationSvnViewModel CreateApplicationSvnEditViewModel(SvnRepository svnRepository, ICollection<SvnRepositoryType> repositoryTypeList)
        {
            return new ApplicationSvnViewModel
            {
                ApplicationSvn = svnRepository,
                SvnRepositoryTypeList = repositoryTypeList,
                BreadCrumbViewModel = breadCrumbFactory.CreateEditSvnRepositoryModel(svnRepository.ApplicationId, svnRepository.Id)
            };
        }

        #endregion
    }
}