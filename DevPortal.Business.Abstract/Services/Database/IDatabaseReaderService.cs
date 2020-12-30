using DevPortal.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.Business.Abstract
{
    public interface IDatabaseReaderService
    {
        ICollection<Database> GetDatabases();

        Database GetDatabase(int databaseId);

        ICollection<DatabaseGroup> GetDatabaseGroups();

        ICollection<DatabaseType> GetDatabaseTypes();

        Task<IEnumerable<Database>> GetFilteredDatabaseListAsync(DatabaseTableParam tableParam);

        Task<ICollection<RedmineProject>> GetFilteredDatabaseRedmineProjectListAsync(DatabaseRedmineProjectTableParam tableParam);

        ICollection<Database> GetDatabaseByDatabaseTypeId(int id);

        ICollection<Database> GetDatabasesByDatabaseGroupId(int databaseGroupId);

        ICollection<Database> GetDatabasesByDatabaseName(string databaseName);

        ICollection<Database> GetDatabasesByGroupIdAndName(int databaseGroupId, string databaseName);

        ICollection<Database> FilterDatabases(int databaseGroupId, string databaseName);
    }
}