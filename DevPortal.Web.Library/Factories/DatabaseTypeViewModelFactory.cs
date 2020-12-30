using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Factories
{
    public class DatabaseTypeViewModelFactory : IDatabaseTypeViewModelFactory
    {
        #region ctor

        readonly IBreadCrumbFactory breadCrumbFactory;

        readonly IAuthorizationServiceWrapper authorizationWrapper;

        public DatabaseTypeViewModelFactory(
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

        public DatabaseTypeViewModel CreateDatabaseTypeViewModel(ICollection<DatabaseType> databaseTypes)
        {
            var isAuthorized = CheckUserHasAdminDeveloperPolicy();
            var breadCrumbViewModel = breadCrumbFactory.CreateDatabaseTypesModel();

            return new DatabaseTypeViewModel
            {
                DatabaseTypes = databaseTypes,
                IsAuthorized = isAuthorized,
                BreadCrumbViewModel = breadCrumbViewModel
            };
        }

        public DatabaseTypeViewModel CreateDatabaseTypeAddView()
        {
            return new DatabaseTypeViewModel
            {
                BreadCrumbViewModel = breadCrumbFactory.CreateDatabaseTypeAddModel()
            };
        }

        public DatabaseTypeViewModel CreateDatabaseTypeDetailViewModel(DatabaseType databaseType, ICollection<Database> databases)
        {
            var isAuthorized = CheckUserHasAdminDeveloperPolicy();
            var breadCrumbViewModel = breadCrumbFactory.CreateDatabaseTypeDetailModel(databaseType.Id);

            return new DatabaseTypeViewModel
            {
                DatabaseType = databaseType,
                Databases = databases,
                IsAuthorized = isAuthorized,
                BreadCrumbViewModel = breadCrumbViewModel
            };
        }

        public DatabaseTypeViewModel CreateDatabaseTypeEditViewModel(DatabaseType databaseType)
        {
            var breadCrumbViewModel = breadCrumbFactory.CreateDatabaseTypeEditModel(databaseType.Id);

            return new DatabaseTypeViewModel
            {
                DatabaseType = databaseType,
                BreadCrumbViewModel = breadCrumbViewModel
            };
        }
    }
}