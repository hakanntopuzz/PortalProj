using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevPortal.Business.Services
{
    public class DatabaseReaderService : IDatabaseReaderService
    {
        #region ctor

        readonly IDatabaseRepository databaseRepository;

        readonly IGeneralSettingsService generalSettingsService;

        public DatabaseReaderService(IDatabaseRepository databaseRepository, IGeneralSettingsService generalSettingsService)
        {
            this.databaseRepository = databaseRepository;
            this.generalSettingsService = generalSettingsService;
        }

        #endregion

        public ICollection<Database> GetDatabases()
        {
            return databaseRepository.GetDatabases();
        }

        public ICollection<DatabaseGroup> GetDatabaseGroups()
        {
            return databaseRepository.GetDatabaseGroups();
        }

        public ICollection<DatabaseType> GetDatabaseTypes()
        {
            return databaseRepository.GetDatabaseTypes();
        }

        public async Task<IEnumerable<Database>> GetFilteredDatabaseListAsync(DatabaseTableParam tableParam)
        {
            if (tableParam == null)
            {
                return new List<Database>();
            }

            return await databaseRepository.GetFilteredDatabaseListAsync(
                tableParam.start,
                tableParam.length,
                tableParam.SortColumn,
                tableParam.order.FirstOrDefault()?.dir,
                tableParam.SearchText,
                tableParam.DatabaseGroupId);
        }

        public Database GetDatabase(int databaseId)
        {
            var database = databaseRepository.GetDatabase(databaseId);

            if (database == null)
            {
                return null;
            }

            database.RecordUpdateInfo = databaseRepository.GetDatabaseUpdateInfo(databaseId);
            SetApplicationRedmineUrl(database);

            return database;
        }

        void SetApplicationRedmineUrl(Database database)
        {
            database.RedmineProjectUrl = generalSettingsService.GetRedmineProjectUrl(database.RedmineProjectName).ToString();
        }

        public async Task<ICollection<RedmineProject>> GetFilteredDatabaseRedmineProjectListAsync(DatabaseRedmineProjectTableParam tableParam)
        {
            if (tableParam == null)
            {
                return new List<RedmineProject>();
            }

            var projects = await databaseRepository.GetFilteredDatabaseRedmineProjectListAsync(
                tableParam.start,
                tableParam.length,
                tableParam.SortColumn,
                tableParam.order.FirstOrDefault()?.dir,
                tableParam.SearchText,
                tableParam.DatabaseGroupId);

            foreach (var project in projects)
            {
                project.ProjectUrl = generalSettingsService.GetRedmineProjectUrl(project.ProjectName).ToString();
                project.RepositoryUrl = $"{project.ProjectUrl}/repository";
            }

            return projects;
        }

        public ICollection<Database> GetDatabaseByDatabaseTypeId(int id)
        {
            return databaseRepository.GetDatabaseByDatabaseTypeId(id);
        }

        public ICollection<Database> GetDatabasesByDatabaseGroupId(int databaseGroupId)
        {
            return databaseRepository.GetDatabasesByDatabaseGroupId(databaseGroupId);
        }

        #region filter databases

        public ICollection<Database> FilterDatabases(int databaseGroupId, string databaseName)
        {
            if (FilterByDatabaseGroup(databaseGroupId, databaseName))
            {
                return GetDatabasesByDatabaseGroupId(databaseGroupId);
            }

            if (FilterByDatabaseName(databaseGroupId, databaseName))
            {
                return GetDatabasesByDatabaseName(databaseName);
            }

            if (FilterByDatabaseGroupAndName(databaseGroupId, databaseName))
            {
                return GetDatabasesByGroupIdAndName(databaseGroupId, databaseName);
            }

            return GetDatabases();
        }

        static bool FilterByDatabaseGroup(int databaseGroupId, string databaseName)
        {
            return databaseGroupId != 0 && string.IsNullOrEmpty(databaseName);
        }

        static bool FilterByDatabaseName(int databaseGroupId, string databaseName)
        {
            return databaseGroupId == 0 && !string.IsNullOrEmpty(databaseName);
        }

        static bool FilterByDatabaseGroupAndName(int databaseGroupId, string databaseName)
        {
            return databaseGroupId != 0 && !string.IsNullOrEmpty(databaseName);
        }

        #endregion

        public ICollection<Database> GetDatabasesByDatabaseName(string databaseName)
        {
            return databaseRepository.GetDatabasesByDatabaseName(databaseName);
        }

        public ICollection<Database> GetDatabasesByGroupIdAndName(int databaseGroupId, string databaseName)
        {
            return databaseRepository.GetDatabasesByGroupIdAndName(databaseGroupId, databaseName);
        }
    }
}