using AB.Data.DapperClient.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Repositories
{
    public class ExternalDependencyRepository : BaseDevPortalRepository, IExternalDependencyRepository
    {
        #region ctor

        readonly IDataClient dataClient;

        public ExternalDependencyRepository(
            IDataClient dataClient,
            IApplicationDataRequestFactory dataRequestFactory,
            ISettings settings)
            : base(dataClient, dataRequestFactory, settings)
        {
            this.dataClient = dataClient;
        }

        #endregion

        #region  get external dependency by id

        public ExternalDependency GetExternalDependencyById(int id)
        {
            var dataRequest = dataRequestFactory.GetExternalDependencyById(id);
            const ExternalDependency defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        #endregion

        #region get external dependency update info

        public RecordUpdateInfo GetExternalDependencyUpdateInfo(int id)
        {
            var dataRequest = dataRequestFactory.GetExternalDependencyUpdateInfo(id);
            const RecordUpdateInfo defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        #endregion

        #region add external dependency

        public int AddExternalDependency(ExternalDependency externalDependency)
        {
            var dataRequest = dataRequestFactory.AddExternalDependency(externalDependency);
            const int defaultReturnValue = 0;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        #endregion

        #region update external dependency

        public bool UpdateExternalDependency(ExternalDependency externalDependency)
        {
            var dataRequest = dataRequestFactory.UpdateExternalDependency(externalDependency);

            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        #endregion

        #region delete external dependency

        public bool DeleteExternalDependency(ExternalDependency externalDependency)
        {
            var dataRequest = dataRequestFactory.DeleteExternalDependency(externalDependency);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        #endregion

        #region  get external dependencies

        public ICollection<ExternalDependenciesExportListItem> GetExternalDependencies(int applicationId)
        {
            var dataRequest = dataRequestFactory.GetExternalDependencies(applicationId);
            var defaultReturnValue = new List<ExternalDependenciesExportListItem>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        #endregion
    }
}