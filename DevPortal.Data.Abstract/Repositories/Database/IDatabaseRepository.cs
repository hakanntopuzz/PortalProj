using DevPortal.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.Data.Abstract.Repositories
{
    public interface IDatabaseRepository
    {
        ICollection<Database> GetDatabases();

        Database GetDatabase(int databaseId);

        ICollection<DatabaseGroup> GetDatabaseGroups();

        ICollection<DatabaseType> GetDatabaseTypes();

        Task<ICollection<Database>> GetFilteredDatabaseListAsync(int skip, int take, string orderBy, string orderDir, string searchText, int databaseGroupId);

        RecordUpdateInfo GetDatabaseUpdateInfo(int databaseId);

        bool UpdateDatabase(Database database);

        Database GetDatabaseByDatabaseName(string name);

        int AddDatabase(Database database);

        ICollection<Database> GetDatabaseByDatabaseTypeId(int id);

        bool DeleteDatabase(int databaseId);

        ICollection<Database> GetDatabasesByDatabaseGroupId(int databaseGroupId);

        ICollection<Database> GetDatabasesByDatabaseName(string databaseName);

        ICollection<Database> GetDatabasesByGroupIdAndName(int databaseGroupId, string databaseName);

        Task<ICollection<RedmineProject>> GetFilteredDatabaseRedmineProjectListAsync(int skip, int take, string orderBy, string orderDir, string searchText, int databaseGroupId);
    }
}