using DevPortal.Model;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Abstract
{
    public interface IDatabaseGroupViewModelFactory
    {
        DatabaseGroupViewModel CreateDatabaseGroupViewModel(ICollection<DatabaseGroup> databaseGroups);

        DatabaseGroupViewModel CreateDatabaseGroupAddView();

        DatabaseGroupViewModel CreateDatabaseGroupDetailViewModel(DatabaseGroup databaseGroup, ICollection<Database> databases);

        DatabaseGroupViewModel CreateDatabaseGroupEditViewModel(DatabaseGroup databaseGroup);
    }
}