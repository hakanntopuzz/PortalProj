using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;

namespace DevPortal.Web.Library
{
    public class ApplicationNugetPackageViewModelFactory : IApplicationNugetPackageViewModelFactory
    {
        #region ctor

        readonly IBreadCrumbFactory breadCrumbFactory;

        readonly IAuthorizationServiceWrapper authorizationWrapper;

        public ApplicationNugetPackageViewModelFactory(
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

        #region application nuget package

        public ApplicationNugetPackageViewModel CreateDetailApplicationNugetPackageViewModel(Application application, ApplicationNugetPackage applicationNugetPackage, string nugetPackageUrl)
        {
            if (application == null || applicationNugetPackage == null)
            {
                return null;
            }

            var isAuthorized = CheckUserHasAdminDeveloperPolicy();
            var breadCrumbViewModel = breadCrumbFactory.CreateDetailNugetPackageModel(application.Id);
            var favouritePageName = $"{application.Name} - NuGet Paketi - {applicationNugetPackage.NugetPackageName}";

            var model = new ApplicationNugetPackageViewModel
            {
                ApplicationNugetPackage = applicationNugetPackage,
                NugetPackageUrl = nugetPackageUrl,
                IsAuthorized = isAuthorized,
                BreadCrumbViewModel = breadCrumbViewModel,
                FavouritePageName = favouritePageName
            };
            model.ApplicationNugetPackage.ApplicationName = application.Name;

            return model;
        }

        public ApplicationNugetPackageViewModel CreateApplicationNugetPackageViewModel(Application application)
        {
            return new ApplicationNugetPackageViewModel
            {
                ApplicationNugetPackage = new ApplicationNugetPackage
                {
                    ApplicationId = application.Id,
                    ApplicationName = application.Name
                },
                BreadCrumbViewModel = breadCrumbFactory.CreateNewNugetPackageModel(application.Id)
            };
        }

        public ApplicationNugetPackageViewModel CreateEditApplicationNugetPackageViewModel(Application application, ApplicationNugetPackage applicationNugetPackage, string nugetPackageUrl)
        {
            var model = new ApplicationNugetPackageViewModel
            {
                ApplicationNugetPackage = applicationNugetPackage,
                NugetPackageUrl = nugetPackageUrl,
                BreadCrumbViewModel = breadCrumbFactory.CreateEditNugetPackageModel(application.Id, applicationNugetPackage.NugetPackageId)
            };
            model.ApplicationNugetPackage.ApplicationName = application.Name;

            return model;
        }

        #endregion
    }
}