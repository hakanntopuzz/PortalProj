using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Abstract
{
    public interface IDatabaseTypeRepository
    {
        ICollection<DatabaseType> GetDatabaseTypes();

        DatabaseType GetDatabaseTypeByName(string name);

        bool AddDatabaseType(DatabaseType databaseType);

        DatabaseType GetDatabaseTypeById(int id);

        RecordUpdateInfo GetDatabaseTypeUpdateInfo(int id);

        bool UpdateDatabaseType(DatabaseType databaseType);

        int GetDatabaseCountByDatabaseTypeId(int databaseTypeId);

        bool DeleteDatabaseType(int databaseTypeId);
    }
}