using DevPortal.Model;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Business.Abstract
{
    public interface IDatabaseTypeService
    {
        ICollection<DatabaseType> GetDatabaseTypes();

        ServiceResult AddDatabaseType(DatabaseType databaseType);

        ServiceResult UpdateDatabaseType(DatabaseType databaseType);

        DatabaseType GetDatabaseType(int id);

        RedirectableClientActionResult DeleteDatabaseType(int databaseTypeId);
    }
}