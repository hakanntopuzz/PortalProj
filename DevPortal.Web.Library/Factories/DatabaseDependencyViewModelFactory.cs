using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library
{
    public class DatabaseDependencyViewModelFactory : IDatabaseDependencyViewModelFactory
    {
        #region ctor

        readonly IBreadCrumbFactory breadCrumbFactory;

        readonly IAuthorizationServiceWrapper authorizationWrapper;

        public DatabaseDependencyViewModelFactory(IBreadCrumbFactory breadCrumbFactory,
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

        public DatabaseDependencyViewModel CreateDatabaseDependencyViewModel(DatabaseDependency databaseDependency)
        {
            if (databaseDependency == null)
            {
                return null;
            }

            var isAuthorized = CheckUserHasAdminDeveloperPolicy();
            var favouritePageName = $"{databaseDependency.ApplicationName} - Veritabanı Bağımlılığı - {databaseDependency.Name}";

            return new DatabaseDependencyViewModel
            {
                DatabaseDependency = databaseDependency,
                IsAuthorized = isAuthorized,
                BreadCrumbViewModel = breadCrumbFactory.CreateDatabaseDependencyDetailModel(databaseDependency.DependentApplicationId),
                FavouritePageName = favouritePageName
            };
        }

        public DatabaseDependencyViewModel CreatDatabaseDependencyViewModelAddView(int applicationId, ICollection<DatabaseGroup> databaseGroups, ICollection<Database> databases)
        {
            return new DatabaseDependencyViewModel
            {
                DatabaseGroups = databaseGroups,
                Databases = databases,
                DatabaseDependency = new DatabaseDependency
                {
                    ApplicationId = applicationId
                },
                BreadCrumbViewModel = breadCrumbFactory.CreateDatabaseDependencyAddModel(applicationId),
            };
        }

        public DatabaseDependencyViewModel CreateDatabaseDependencyEditViewModel(DatabaseDependency databaseDependency)
        {
            return new DatabaseDependencyViewModel
            {
                DatabaseDependency = databaseDependency,
                BreadCrumbViewModel = breadCrumbFactory.CreateDatabaseDependencyEditModel(databaseDependency.DependentApplicationId, databaseDependency.Id)
            };
        }
    }
}