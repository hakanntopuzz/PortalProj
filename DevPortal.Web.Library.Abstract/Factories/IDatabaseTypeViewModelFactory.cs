using DevPortal.Model;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Abstract
{
    public interface IDatabaseTypeViewModelFactory
    {
        DatabaseTypeViewModel CreateDatabaseTypeViewModel(ICollection<DatabaseType> databaseTypes);

        DatabaseTypeViewModel CreateDatabaseTypeAddView();

        DatabaseTypeViewModel CreateDatabaseTypeDetailViewModel(DatabaseType databaseType, ICollection<Database> databases);

        DatabaseTypeViewModel CreateDatabaseTypeEditViewModel(DatabaseType databaseType);
    }
}