using AB.Data.DapperClient.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Factories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Repositories
{
    public class DatabaseTypeRepository : BaseDevPortalRepository, IDatabaseTypeRepository
    {
        #region ctor

        readonly IDataClient dataClient;

        public DatabaseTypeRepository(
           IDataClient dataClient,
           IApplicationDataRequestFactory dataRequestFactory,
           ISettings settings) : base(dataClient, dataRequestFactory, settings)
        {
            this.dataClient = dataClient;
        }

        #endregion

        #region get database types

        public ICollection<DatabaseType> GetDatabaseTypes()
        {
            var dataRequest = dataRequestFactory.GetDatabaseTypes();
            var defaultReturnValue = new List<DatabaseType>();

            return dataClient.GetCollection<DatabaseType, RecordUpdateInfo, DatabaseType>(
                           dataRequest,
                           DataClientMapFactory.DatabaseTypesMap,
                           defaultReturnValue,
                           dataRequest.SplitOnParameters);
        }

        #endregion

        #region add database type

        public bool AddDatabaseType(DatabaseType databaseType)
        {
            var dataRequest = dataRequestFactory.AddDatabaseType(databaseType);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        #endregion

        #region update database type

        public bool UpdateDatabaseType(DatabaseType databaseType)
        {
            var dataRequest = dataRequestFactory.UpdateDatabaseType(databaseType);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        #endregion

        #region delete database type

        public bool DeleteDatabaseType(int databaseTypeId)
        {
            var dataRequest = dataRequestFactory.DeleteDatabaseType(databaseTypeId);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        #endregion

        #region get database type by name

        public DatabaseType GetDatabaseTypeByName(string name)
        {
            var dataRequest = dataRequestFactory.GetDatabaseTypeByName(name);
            const DatabaseType defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        #endregion

        #region get database type by id

        public DatabaseType GetDatabaseTypeById(int id)
        {
            var dataRequest = dataRequestFactory.GetDatabaseTypeById(id);
            const DatabaseType defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        #endregion

        #region get database type update info

        public RecordUpdateInfo GetDatabaseTypeUpdateInfo(int id)
        {
            var dataRequest = dataRequestFactory.GetDatabaseTypeUpdateInfo(id);
            const RecordUpdateInfo defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        #endregion

        #region get database count by database type id

        public int GetDatabaseCountByDatabaseTypeId(int databaseTypeId)
        {
            var dataRequest = dataRequestFactory.GetDatabaseCountByDatabaseTypeId(databaseTypeId);
            const int defaultReturnValue = 0;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        #endregion
    }
}