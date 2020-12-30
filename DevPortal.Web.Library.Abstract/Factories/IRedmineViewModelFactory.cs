using DevPortal.Model;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Abstract
{
    public interface IRedmineViewModelFactory
    {
        ApplicationRedmineProjectsViewModel CreateApplicationRedmineProjectsViewModel(ICollection<ApplicationGroup> applicationGroups);

        RedmineProjectListModel CreateRedmineProjectListModel(ICollection<RedmineProject> projects);

        DatabaseRedmineProjectsViewModel CreateDatabaseRedmineProjectsViewModel(ICollection<DatabaseGroup> databaseGroups);
    }
}