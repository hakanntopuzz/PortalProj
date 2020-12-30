using AB.Data.DapperClient.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;

namespace DevPortal.Data.Repositories
{
    public class ApplicationBuildSettingsRepository : BaseDevPortalRepository, IApplicationBuildSettingsRepository
    {
        #region ctor

        readonly IDataClient dataClient;

        public ApplicationBuildSettingsRepository(
            IDataClient dataClient,
            IApplicationDataRequestFactory dataRequestFactory,
            ISettings settings)
            : base(dataClient, dataRequestFactory, settings)
        {
            this.dataClient = dataClient;
        }

        #endregion

        public ApplicationBuildSettings GetApplicationBuildSettings(int applicationId)
        {
            var dataRequest = dataRequestFactory.GetApplicationBuildSettings(applicationId);
            const ApplicationBuildSettings defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public bool AddApplicationBuildSettings(ApplicationBuildSettings buildSettings)
        {
            var dataRequest = dataRequestFactory.AddApplicationBuildSettings(buildSettings);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public bool UpdateApplicationBuildSettings(ApplicationBuildSettings buildSettings)
        {
            var dataRequest = dataRequestFactory.UpdateApplicationBuildSettings(buildSettings);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public RecordUpdateInfo GetApplicationBuildSettingsUpdateInfo(int applicationId)
        {
            var dataRequest = dataRequestFactory.GetApplicationBuildSettingsUpdateInfo(applicationId);
            const RecordUpdateInfo defaultReturnValue = null;

            return DataClient.GetItem(dataRequest, defaultReturnValue);
        }
    }
}
