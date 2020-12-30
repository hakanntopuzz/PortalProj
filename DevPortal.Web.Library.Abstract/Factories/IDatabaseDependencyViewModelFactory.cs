using DevPortal.Model;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Abstract
{
    public interface IDatabaseDependencyViewModelFactory
    {
        DatabaseDependencyViewModel CreateDatabaseDependencyViewModel(DatabaseDependency databaseDependency);

        DatabaseDependencyViewModel CreatDatabaseDependencyViewModelAddView(int applicationId, ICollection<DatabaseGroup> databaseGroups, ICollection<Database> databases);

        DatabaseDependencyViewModel CreateDatabaseDependencyEditViewModel(DatabaseDependency databaseDependency);
    }
}