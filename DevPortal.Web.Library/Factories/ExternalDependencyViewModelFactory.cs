using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;

namespace DevPortal.Web.Library
{
    public class ExternalDependencyViewModelFactory : IExternalDependencyViewModelFactory
    {
        #region ctor

        readonly IBreadCrumbFactory breadCrumbFactory;

        readonly IAuthorizationServiceWrapper authorizationWrapper;

        public ExternalDependencyViewModelFactory(IBreadCrumbFactory breadCrumbFactory,
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

        public ExternalDependencyViewModel CreateExternalDependencyViewModel(ExternalDependency externalDependency)
        {
            if (externalDependency == null)
            {
                return null;
            }

            var isAuthorized = CheckUserHasAdminDeveloperPolicy();
            var favouritePageName = $"{externalDependency.ApplicationName} - Harici Bağımlılık - {externalDependency.Name}";

            return new ExternalDependencyViewModel
            {
                ExternalDependency = externalDependency,
                IsAuthorized = isAuthorized,
                BreadCrumbViewModel = breadCrumbFactory.CreateExternalDependencyDetailModel(externalDependency.ApplicationId),
                FavouritePageName = favouritePageName
            };
        }

        public ExternalDependencyViewModel CreateAddExternalDependencyViewModel(string applicationName, int applicationId)
        {
            return new ExternalDependencyViewModel
            {
                ExternalDependency = new ExternalDependency
                {
                    ApplicationId = applicationId,
                    ApplicationName = applicationName
                },
                BreadCrumbViewModel = breadCrumbFactory.CreateExternalDependencyAddModel(applicationId)
            };
        }

        public ExternalDependencyViewModel CreateEditExternalDependencyViewModel(ExternalDependency externalDependency)
        {
            return new ExternalDependencyViewModel
            {
                ExternalDependency = externalDependency,
                BreadCrumbViewModel = breadCrumbFactory.CreateExternalDependencyEditModel(externalDependency.ApplicationId, externalDependency.Id)
            };
        }
    }
}