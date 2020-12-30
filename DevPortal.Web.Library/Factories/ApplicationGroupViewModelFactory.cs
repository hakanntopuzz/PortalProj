using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library
{
    public class ApplicationGroupViewModelFactory : IApplicationGroupViewModelFactory
    {
        #region ctor

        readonly IBreadCrumbFactory breadCrumbFactory;

        readonly IAuthorizationServiceWrapper authorizationWrapper;

        public ApplicationGroupViewModelFactory(
            IBreadCrumbFactory breadCrumbFactory,
            IAuthorizationServiceWrapper authorizationWrapper)
        {
            this.breadCrumbFactory = breadCrumbFactory;
            this.authorizationWrapper = authorizationWrapper;
        }

        #endregion

        bool CheckUserHasAdminDeveloperPolicy()
        {
            return authorizationWrapper.CheckUserHasAdminDeveloperPolicy();
        }

        #region application group

        public ApplicationGroupViewModel CreateApplicationGroupsViewModel(ICollection<ApplicationGroup> applicationGroups)
        {
            var isAuthorized = CheckUserHasAdminDeveloperPolicy();
            var breadCrumbViewModel = breadCrumbFactory.CreateApplicationGroupsModel();

            return new ApplicationGroupViewModel
            {
                ApplicationGroups = applicationGroups,
                IsAuthorized = isAuthorized,
                BreadCrumbViewModel = breadCrumbViewModel
            };
        }

        public ApplicationGroupViewModel CreateApplicationGroupAddViewModel(ICollection<ApplicationGroupStatus> status)
        {
            return new ApplicationGroupViewModel
            {
                Status = status,
                BreadCrumbViewModel = breadCrumbFactory.CreateApplicationGroupAddModel()
            };
        }

        public ApplicationGroupViewModel CreateEditApplicationGroup(ApplicationGroup applicationGroup, ICollection<ApplicationGroupStatus> status)
        {
            return new ApplicationGroupViewModel
            {
                ApplicationGroup = applicationGroup,
                Status = status,
                BreadCrumbViewModel = breadCrumbFactory.CreateApplicationGroupEditModel()
            };
        }

        public ApplicationGroupViewModel CreateDetailApplicationGroup(ApplicationGroup applicationGroup, ICollection<ApplicationListItem> applicationLists)
        {
            var isAurhorized = CheckUserHasAdminDeveloperPolicy();
            var breadCrumbViewModel = breadCrumbFactory.CreateApplicationGroupDetailModel();

            return new ApplicationGroupViewModel
            {
                ApplicationGroup = applicationGroup,
                ApplicationList = applicationLists,
                IsAuthorized = isAurhorized,
                BreadCrumbViewModel = breadCrumbViewModel
            };
        }

        #endregion
    }
}