using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Abstract.Factories;
using DevPortal.Web.Library.Model;

namespace DevPortal.Web.Library.Factories
{
    public class NugetPackageDependencyViewModelFactory : INugetPackageDependencyViewModelFactory
    {
        #region ctor

        readonly IBreadCrumbFactory breadCrumbFactory;

        readonly IAuthorizationServiceWrapper authorizationWrapper;

        public NugetPackageDependencyViewModelFactory(IBreadCrumbFactory breadCrumbFactory,
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

        public NugetPackageDependencyViewModel CreateNugetPackageDependencyViewModel(NugetPackageDependency nugetPackageDependency)
        {
            if (nugetPackageDependency == null)
            {
                return null;
            }

            var isAuthorized = CheckUserHasAdminDeveloperPolicy();
            var favouritePageName = $"{nugetPackageDependency.ApplicationName} - Nuget Paketi Bağımlılığı - {nugetPackageDependency.NugetPackageName}";

            return new NugetPackageDependencyViewModel
            {
                NugetPackageDependency = nugetPackageDependency,
                IsAuthorized = isAuthorized,
                BreadCrumbViewModel = breadCrumbFactory.CreateNugetPackageDependencyDetailModel(nugetPackageDependency.DependentApplicationId),
                FavouritePageName = favouritePageName
            };
        }

        public NugetPackageDependencyViewModel CreateNugetPackageDependencyViewModelAddView(Application application)
        {
            return new NugetPackageDependencyViewModel
            {
                NugetPackageDependency = new NugetPackageDependency
                {
                    ApplicationName = application.Name,
                    DependentApplicationId = application.Id
                },
                BreadCrumbViewModel = breadCrumbFactory.CreateNugetPackageDependencyAddModel(application.Id),
            };
        }
    }
}