using DevPortal.Model;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Abstract.Factories
{
    public interface IDatabaseViewModelFactory
    {
        DatabasesViewModel CreateDatabasesViewModel(ICollection<DatabaseGroup> databaseGroups, ICollection<Database> databases);

        DatabaseListModel CreateDatabaseListModel(IEnumerable<Database> databaseList);

        DatabaseViewModel CreateDatabase(Database database);

        EditDatabaseViewModel CreateEditDatabase(Database database, ICollection<DatabaseGroup> databaseGroups, ICollection<DatabaseType> databaseTypes);

        DatabaseViewModel CreateAddViewModel(ICollection<DatabaseType> databaseTypes, ICollection<DatabaseGroup> databaseGroups);
    }
}