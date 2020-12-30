using DevPortal.Model;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Business.Abstract
{
    public interface IDatabaseGroupService
    {
        ICollection<DatabaseGroup> GetDatabaseGroups();

        ServiceResult AddDatabaseGroup(DatabaseGroup databaseGroup);

        DatabaseGroup GetDatabaseGroup(int id);

        ServiceResult UpdateDatabaseGroup(DatabaseGroup databaseGroup);

        RedirectableClientActionResult DeleteDatabaseGroup(int databaseGroupId);
    }
}