using AB.Data.DapperClient.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Repositories
{
    public class DatabaseDependencyRepository : BaseDevPortalRepository, IDatabaseDependencyRepository
    {
        #region ctor

        readonly IDataClient dataClient;

        public DatabaseDependencyRepository(
            IDataClient dataClient,
            IApplicationDataRequestFactory dataRequestFactory,
            ISettings settings)
            : base(dataClient, dataRequestFactory, settings)
        {
            this.dataClient = dataClient;
        }

        #endregion

        #region get database dependency by id

        public DatabaseDependency GetDatabaseDependencyById(int id)
        {
            var dataRequest = dataRequestFactory.GetDatabaseDependencyById(id);
            const DatabaseDependency defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        #endregion

        #region get database dependency update info

        public RecordUpdateInfo GetDatabaseDependencyUpdateInfo(int id)
        {
            var dataRequest = dataRequestFactory.GetDatabaseDependencyUpdateInfo(id);
            const RecordUpdateInfo defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        #endregion

        #region add database dependency

        public bool AddDatabaseDependency(DatabaseDependency databaseDependency)
        {
            var dataRequest = dataRequestFactory.AddDatabaseDependency(databaseDependency);

            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        #endregion

        #region update database dependency

        public bool UpdateDatabaseDependency(DatabaseDependency databaseDependency)
        {
            var dataRequest = dataRequestFactory.UpdateDatabaseDependency(databaseDependency);

            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        #endregion

        #region delete database dependency

        public bool DeleteDatabaseDependency(int databaseDependencyId)
        {
            var dataRequest = dataRequestFactory.DeleteDatabaseDependency(databaseDependencyId);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        #endregion

        public ICollection<DatabaseDependency> GetDatabaseDependenciesByApplicationId(int applicationId)
        {
            var dataRequest = dataRequestFactory.GetDatabaseDependenciesByApplicationId(applicationId);
            var defaultReturnValue = new List<DatabaseDependency>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public ICollection<DatabaseDependenciesExportListItem> GetFullDatabaseDependenciesByApplicationId(int applicationId)
        {
            var dataRequest = dataRequestFactory.GetFullDatabaseDependenciesByApplicationId(applicationId);
            var defaultReturnValue = new List<DatabaseDependenciesExportListItem>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }
    }
}