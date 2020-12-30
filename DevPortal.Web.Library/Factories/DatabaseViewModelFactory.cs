using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Abstract.Factories;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Factories
{
    public class DatabaseViewModelFactory : IDatabaseViewModelFactory
    {
        #region ctor

        readonly IBreadCrumbFactory breadCrumbFactory;

        readonly IAuthorizationServiceWrapper authorizationWrapper;

        public DatabaseViewModelFactory(
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

        public DatabasesViewModel CreateDatabasesViewModel(ICollection<DatabaseGroup> databaseGroups, ICollection<Database> databases)
        {
            var isAuthorized = CheckUserHasAdminDeveloperPolicy();
            var breadCrumbViewModel = breadCrumbFactory.CreateDatabasesModel();

            return new DatabasesViewModel
            {
                DatabaseGroups = databaseGroups,
                Databases = databases,
                IsAuthorized = isAuthorized,
                BreadCrumbViewModel = breadCrumbViewModel
            };
        }

        public DatabaseListModel CreateDatabaseListModel(IEnumerable<Database> databaseList)
        {
            return new DatabaseListModel
            {
                data = databaseList
            };
        }

        public DatabaseViewModel CreateDatabase(Database database)
        {
            var isAuthorized = CheckUserHasAdminDeveloperPolicy();
            var breadCrumbViewModel = breadCrumbFactory.CreateDatabaseDetailModel(database.Id);

            return new DatabaseViewModel
            {
                Database = database,
                IsAuthorized = isAuthorized,
                BreadCrumbViewModel = breadCrumbViewModel
            };
        }

        public EditDatabaseViewModel CreateEditDatabase(Database database, ICollection<DatabaseGroup> databaseGroups, ICollection<DatabaseType> databaseTypes)
        {
            return new EditDatabaseViewModel
            {
                DatabaseGroups = databaseGroups,
                DatabaseTypes = databaseTypes,
                Database = database,
                BreadCrumbViewModel = breadCrumbFactory.CreateDatabaseEditModel(database.Id)
            };
        }

        public DatabaseViewModel CreateAddViewModel(ICollection<DatabaseType> databaseTypes, ICollection<DatabaseGroup> databaseGroups)
        {
            var isAuthorized = CheckUserHasAdminDeveloperPolicy();
            var breadCrumbViewModel = breadCrumbFactory.CreateDatabaseAddModel();

            return new DatabaseViewModel
            {
                DatabaseTypes = databaseTypes,
                DatabaseGroups = databaseGroups,
                IsAuthorized = isAuthorized,
                BreadCrumbViewModel = breadCrumbViewModel
            };
        }
    }
}