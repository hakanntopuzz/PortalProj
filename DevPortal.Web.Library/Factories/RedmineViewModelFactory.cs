using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library
{
    public class RedmineViewModelFactory : IRedmineViewModelFactory
    {
        #region ctor

        readonly IBreadCrumbFactory breadCrumbFactory;

        public RedmineViewModelFactory(
            IBreadCrumbFactory breadCrumbFactory)
        {
            this.breadCrumbFactory = breadCrumbFactory;
        }

        #endregion

        public ApplicationRedmineProjectsViewModel CreateApplicationRedmineProjectsViewModel(ICollection<ApplicationGroup> applicationGroups)
        {
            var breadCrumbViewModel = breadCrumbFactory.CreateApplicationRedmineProjectsModel();

            return new ApplicationRedmineProjectsViewModel
            {
                BreadCrumbViewModel = breadCrumbViewModel,
                ApplicationGroups = applicationGroups
            };
        }

        public RedmineProjectListModel CreateRedmineProjectListModel(ICollection<RedmineProject> projects)
        {
            return new RedmineProjectListModel
            {
                data = projects
            };
        }

        public DatabaseRedmineProjectsViewModel CreateDatabaseRedmineProjectsViewModel(ICollection<DatabaseGroup> databaseGroups)
        {
            var breadCrumbViewModel = breadCrumbFactory.CreateDatabaseRedmineProjectsModel();

            return new DatabaseRedmineProjectsViewModel
            {
                BreadCrumbViewModel = breadCrumbViewModel,
                DatabaseGroups = databaseGroups
            };
        }
    }
}