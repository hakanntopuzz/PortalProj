using AB.Data.DapperClient.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;

namespace DevPortal.Data.Repositories
{
    public class ApplicationDependencyRepository : BaseDevPortalRepository, IApplicationDependencyRepository
    {
        #region ctor

        readonly IDataClient dataClient;

        public ApplicationDependencyRepository(
            IDataClient dataClient,
            IApplicationDataRequestFactory dataRequestFactory,
            ISettings settings)
            : base(dataClient, dataRequestFactory, settings)
        {
            this.dataClient = dataClient;
        }

        #endregion

        public bool AddApplicationDependency(ApplicationDependency applicationDependency)
        {
            var dataRequest = dataRequestFactory.AddApplicationDependency(applicationDependency);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public ApplicationDependency GetApplicationDependencyById(int applicationId)
        {
            var dataRequest = dataRequestFactory.GetApplicationDependencyById(applicationId);
            const ApplicationDependency defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        #region get applicaiton dependency update info

        public RecordUpdateInfo GetApplicationDependencyUpdateInfo(int id)
        {
            var dataRequest = dataRequestFactory.GetApplicationDependencyUpdateInfo(id);
            const RecordUpdateInfo defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        #endregion

        public bool UpdateApplicationDependency(ApplicationDependency applicationDependency)
        {
            var dataRequest = dataRequestFactory.UpdateApplicationDependency(applicationDependency);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public bool DeleteApplicationDependency(int applicationDependencyId)
        {
            var dataRequest = dataRequestFactory.DeleteApplicationDependency(applicationDependencyId);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }
    }
}