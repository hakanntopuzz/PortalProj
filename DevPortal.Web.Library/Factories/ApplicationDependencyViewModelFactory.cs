using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library
{
    public class ApplicationDependencyViewModelFactory : IApplicationDependencyViewModelFactory
    {
        #region ctor

        readonly IBreadCrumbFactory breadCrumbFactory;

        readonly IAuthorizationServiceWrapper authorizationWrapper;

        public ApplicationDependencyViewModelFactory(IBreadCrumbFactory breadCrumbFactory,
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

        public ApplicationDependencyViewModel CreateApplicationDependencyViewModel(ApplicationDependency applicationDependency)
        {
            if (applicationDependency == null)
            {
                return null;
            }

            var isAuthorized = CheckUserHasAdminDeveloperPolicy();
            var favouritePageName = $"{applicationDependency.ApplicationName} - Uygulama Bağımlılığı - {applicationDependency.Name}";

            return new ApplicationDependencyViewModel
            {
                ApplicationDependency = applicationDependency,
                IsAuthorized = isAuthorized,
                BreadCrumbViewModel = breadCrumbFactory.CreateApplicationDependencyDetailModel(applicationDependency.DependentApplicationId),
                FavouritePageName = favouritePageName
            };
        }

        public ApplicationDependencyViewModel CreatApplicationDependencyViewModelAddView(int applicationId, ICollection<ApplicationListItem> applicationLists, ICollection<ApplicationGroup> applicationGroups)
        {
            return new ApplicationDependencyViewModel
            {
                ApplicationGroups = applicationGroups,
                Applications = applicationLists,
                ApplicationDependency = new ApplicationDependency
                {
                    DependentApplicationId = applicationId
                },
                BreadCrumbViewModel = breadCrumbFactory.CreateApplicationDependencyAddModel(applicationId),
            };
        }

        public ApplicationDependencyViewModel CreateApplicationDependencyEditViewModel(ApplicationDependency applicationDependency)
        {
            return new ApplicationDependencyViewModel
            {
                ApplicationDependency = applicationDependency,
                BreadCrumbViewModel = breadCrumbFactory.CreateApplicationDependencyEditModel(applicationDependency.DependentApplicationId, applicationDependency.Id)
            };
        }
    }
}