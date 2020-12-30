using AB.Data.DapperClient.Abstract;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Data.Factories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.Data.Repositories
{
    public class DatabaseRepository : BaseDevPortalRepository, IDatabaseRepository
    {
        #region ctor

        readonly IDataClient dataClient;

        public DatabaseRepository(
            IDataClient dataClient,
            IApplicationDataRequestFactory dataRequestFactory,
            ISettings settings) : base(dataClient, dataRequestFactory, settings)
        {
            this.dataClient = dataClient;
        }

        #endregion

        public ICollection<Database> GetDatabases()
        {
            var dataRequest = dataRequestFactory.GetDatabases();
            var defaultReturnValue = new List<Database>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public ICollection<DatabaseGroup> GetDatabaseGroups()
        {
            var dataRequest = dataRequestFactory.GetDatabaseGroups();
            var defaultReturnValue = new List<DatabaseGroup>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public ICollection<DatabaseType> GetDatabaseTypes()
        {
            var dataRequest = dataRequestFactory.GetDatabaseTypes();
            var defaultReturnValue = new List<DatabaseType>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public Database GetDatabase(int databaseId)
        {
            var dataRequest = dataRequestFactory.GetDatabase(databaseId);
            var defaultReturnValue = new Database();

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public async Task<ICollection<Database>> GetFilteredDatabaseListAsync(int skip, int take, string orderBy, string orderDir, string searchText, int databaseGroupId)
        {
            var dataRequest = dataRequestFactory.GetFilteredDatabaseList(skip, take, orderBy, orderDir, searchText, databaseGroupId);
            var defaultReturnValue = new List<Database>();

            return await dataClient.GetCollectionAsync<Database, RecordUpdateInfo, Database>(
                dataRequest,
                DataClientMapFactory.DatabasesMap,
                defaultReturnValue,
                dataRequest.SplitOnParameters);
        }

        public RecordUpdateInfo GetDatabaseUpdateInfo(int databaseId)
        {
            var dataRequest = dataRequestFactory.GetDatabaseUpdateInfo(databaseId);
            const RecordUpdateInfo defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public bool UpdateDatabase(Database database)
        {
            var dataRequest = dataRequestFactory.UpdateDatabase(database);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public Database GetDatabaseByDatabaseName(string name)
        {
            var dataRequest = dataRequestFactory.GetDatabaseByDatabaseName(name);
            const Database defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public int AddDatabase(Database database)
        {
            var dataRequest = dataRequestFactory.AddDatabase(database);
            const int defaultReturnValue = 0;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public ICollection<Database> GetDatabaseByDatabaseTypeId(int id)
        {
            var dataRequest = dataRequestFactory.GetDatabaseByDatabaseTypeId(id);
            var defaultReturnValue = new List<Database>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public bool DeleteDatabase(int databaseId)
        {
            var dataRequest = dataRequestFactory.DeleteDatabase(databaseId);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public ICollection<Database> GetDatabasesByDatabaseGroupId(int databaseGroupId)
        {
            var dataRequest = dataRequestFactory.GetDatabasesByDatabaseGroupId(databaseGroupId);
            var defaultReturnValue = new List<Database>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public ICollection<Database> GetDatabasesByDatabaseName(string databaseName)
        {
            var dataRequest = dataRequestFactory.GetDatabasesByDatabaseName(databaseName);
            var defaultReturnValue = new List<Database>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public ICollection<Database> GetDatabasesByGroupIdAndName(int databaseGroupId, string databaseName)
        {
            var dataRequest = dataRequestFactory.GetDatabasesByGroupIdAndName(databaseGroupId, databaseName);
            var defaultReturnValue = new List<Database>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public async Task<ICollection<RedmineProject>> GetFilteredDatabaseRedmineProjectListAsync(int skip, int take, string orderBy, string orderDir, string searchText, int databaseGroupId)
        {
            var dataRequest = dataRequestFactory.GetFilteredDatabaseRedmineProjectList(skip, take, orderBy, orderDir, searchText, databaseGroupId);
            var defaultReturnValue = new List<RedmineProject>();

            return await dataClient.GetCollectionAsync(dataRequest, defaultReturnValue);
        }
    }
}