using AB.Data.DapperClient.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Factories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Repositories
{
    public class DatabaseGroupRepository : BaseDevPortalRepository, IDatabaseGroupRepository
    {
        #region ctor

        readonly IDataClient dataClient;

        public DatabaseGroupRepository(
           IDataClient dataClient,
           IApplicationDataRequestFactory dataRequestFactory,
           ISettings settings) : base(dataClient, dataRequestFactory, settings)
        {
            this.dataClient = dataClient;
        }

        #endregion

        #region get database groups

        public ICollection<DatabaseGroup> GetDatabaseGroups()
        {
            var dataRequest = dataRequestFactory.GetDatabaseGroups();
            var defaultReturnValue = new List<DatabaseGroup>();

            return dataClient.GetCollection<DatabaseGroup, RecordUpdateInfo, DatabaseGroup>(
                           dataRequest,
                           DataClientMapFactory.DatabaseGroupsMap,
                           defaultReturnValue,
                           dataRequest.SplitOnParameters);
        }

        #endregion

        #region add database group

        public int AddDatabaseGroup(DatabaseGroup databaseGroup)
        {
            var dataRequest = dataRequestFactory.AddDatabaseGroup(databaseGroup);
            const int defaultReturnValue = 0;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        #endregion

        #region get database group by name

        public DatabaseGroup GetDatabaseGroupByName(string name)
        {
            var dataRequest = dataRequestFactory.GetDatabaseGroupByName(name);
            const DatabaseGroup defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        #endregion

        #region get database group by id

        public DatabaseGroup GetDatabaseGroupById(int id)
        {
            var dataRequest = dataRequestFactory.GetDatabaseGroupById(id);
            const DatabaseGroup defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        #endregion

        #region get database group update info

        public RecordUpdateInfo GetDatabaseGroupUpdateInfo(int id)
        {
            var dataRequest = dataRequestFactory.GetDatabaseGroupUpdateInfo(id);
            const RecordUpdateInfo defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        #endregion

        #region update database group

        public bool UpdateDatabaseGroup(DatabaseGroup databaseGroup)
        {
            var dataRequest = dataRequestFactory.UpdateDatabaseGroup(databaseGroup);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        #endregion

        #region get database count by database group id

        public int GetDatabaseCountByDatabaseGroupId(int databaseGroupId)
        {
            var dataRequest = dataRequestFactory.GetDatabaseCountByDatabaseGroupId(databaseGroupId);
            const int defaultReturnValue = 0;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        #endregion

        #region delete database group

        public bool DeleteDatabaseGroup(int databaseGroupId)
        {
            var dataRequest = dataRequestFactory.DeleteDatabaseGroup(databaseGroupId);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        #endregion
    }
}