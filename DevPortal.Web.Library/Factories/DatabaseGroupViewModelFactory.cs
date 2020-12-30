using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Factories
{
    public class DatabaseGroupViewModelFactory : IDatabaseGroupViewModelFactory
    {
        #region ctor

        readonly IBreadCrumbFactory breadCrumbFactory;

        readonly IAuthorizationServiceWrapper authorizationWrapper;

        public DatabaseGroupViewModelFactory(
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

        public DatabaseGroupViewModel CreateDatabaseGroupViewModel(ICollection<DatabaseGroup> databaseGroups)
        {
            var isAuthorized = CheckUserHasAdminDeveloperPolicy();
            var breadCrumbViewModel = breadCrumbFactory.CreateDatabaseGroupsModel();

            return new DatabaseGroupViewModel
            {
                DatabaseGroups = databaseGroups,
                IsAuthorized = isAuthorized,
                BreadCrumbViewModel = breadCrumbViewModel
            };
        }

        public DatabaseGroupViewModel CreateDatabaseGroupAddView()
        {
            return new DatabaseGroupViewModel
            {
                BreadCrumbViewModel = breadCrumbFactory.CreateDatabaseGroupAddModel()
            };
        }

        public DatabaseGroupViewModel CreateDatabaseGroupDetailViewModel(DatabaseGroup databaseGroup, ICollection<Database> databases)
        {
            var isAuthorized = CheckUserHasAdminDeveloperPolicy();
            var breadCrumbViewModel = breadCrumbFactory.CreateDatabaseGroupDetailModel(databaseGroup.Id);

            return new DatabaseGroupViewModel
            {
                DatabaseGroup = databaseGroup,
                Databases = databases,
                IsAuthorized = isAuthorized,
                BreadCrumbViewModel = breadCrumbViewModel
            };
        }

        public DatabaseGroupViewModel CreateDatabaseGroupEditViewModel(DatabaseGroup databaseGroup)
        {
            var breadCrumbViewModel = breadCrumbFactory.CreateDatabaseGroupEditModel(databaseGroup.Id);

            return new DatabaseGroupViewModel
            {
                DatabaseGroup = databaseGroup,
                BreadCrumbViewModel = breadCrumbViewModel
            };
        }
    }
}