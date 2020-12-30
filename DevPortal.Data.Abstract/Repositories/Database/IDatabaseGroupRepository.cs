using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Abstract
{
    public interface IDatabaseGroupRepository
    {
        ICollection<DatabaseGroup> GetDatabaseGroups();

        int AddDatabaseGroup(DatabaseGroup databaseGroup);

        DatabaseGroup GetDatabaseGroupByName(string name);

        DatabaseGroup GetDatabaseGroupById(int id);

        RecordUpdateInfo GetDatabaseGroupUpdateInfo(int id);

        bool UpdateDatabaseGroup(DatabaseGroup databaseGroup);

        int GetDatabaseCountByDatabaseGroupId(int databaseGroupId);

        bool DeleteDatabaseGroup(int databaseGroupId);
    }
}