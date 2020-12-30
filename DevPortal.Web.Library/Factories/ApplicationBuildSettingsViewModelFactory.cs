using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Abstract.Factories;
using DevPortal.Web.Library.Model;

namespace DevPortal.Web.Library.Factories
{
    public class ApplicationBuildSettingsViewModelFactory : IApplicationBuildSettingsViewModelFactory
    {
        #region ctor

        readonly IBreadCrumbFactory breadCrumbFactory;

        readonly IAuthorizationServiceWrapper authorizationWrapper;

        public ApplicationBuildSettingsViewModelFactory(
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

        public ApplicationBuildSettingsViewModel CreateApplicationBuildSettingsViewModel(ApplicationBuildSettings buildSettings, Application application)
        {
            if (string.IsNullOrEmpty(application.Name))
            {
                return null;
            }

            var applicationName = GetApplicationName(application);
            var breadCrumbViewModel = GetBreadCrumbViewModel(application.Id);

            if (buildSettings == null)
            {
                return CreateNewApplicationBuildSettingsViewModel(application, breadCrumbViewModel);
            }

            return CreateUpdateApplicationBuildSettingsViewModel(buildSettings, applicationName, breadCrumbViewModel);
        }

        static string GetApplicationName(Application application)
        {
            return application.Name;
        }

        BreadCrumbViewModel GetBreadCrumbViewModel(int applicationId)
        {
            return breadCrumbFactory.CreateApplicationBuildSettingsModel(applicationId);
        }

        ApplicationBuildSettingsViewModel CreateNewApplicationBuildSettingsViewModel(Application application, BreadCrumbViewModel breadCrumbViewModel)
        {
            var isAuthorized = CheckUserHasAdminDeveloperPolicy();

            return new ApplicationBuildSettingsViewModel
            {
                BuildSettings = new ApplicationBuildSettings { ApplicationId = application.Id },
                ApplicationName = application.Name,
                BreadCrumbViewModel = breadCrumbViewModel,
                IsAuthorized = isAuthorized
            };
        }

        ApplicationBuildSettingsViewModel CreateUpdateApplicationBuildSettingsViewModel(ApplicationBuildSettings buildSettings, string applicationName, BreadCrumbViewModel breadCrumbViewModel)
        {
            var isAuthorized = CheckUserHasAdminDeveloperPolicy();

            return new ApplicationBuildSettingsViewModel
            {
                BuildSettings = buildSettings,
                ApplicationName = applicationName,
                BreadCrumbViewModel = breadCrumbViewModel,
                IsAuthorized = isAuthorized
            };
        }
    }
}
