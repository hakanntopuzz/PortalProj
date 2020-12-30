using AB.Data.DapperClient.Abstract;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;

namespace DevPortal.Data.Repositories
{
    public class GeneralSettingsRepository : BaseDevPortalRepository, IGeneralSettingsRepository
    {
        #region ctor

        readonly IDataClient dataClient;

        readonly IApplicationDataRequestFactory applicationDataRequestFactory;

        public GeneralSettingsRepository(
            IDataClient dataClient,
            IApplicationDataRequestFactory applicationDataRequestFactory,
            ISettings settings)
            : base(dataClient, applicationDataRequestFactory, settings)
        {
            this.dataClient = dataClient;
            this.applicationDataRequestFactory = applicationDataRequestFactory;
        }

        #endregion

        public GeneralSettings GetGeneralSettings()
        {
            var dataRequest = applicationDataRequestFactory.GetGeneralSettings();
            var defaultReturnValue = new GeneralSettings();

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public bool UpdateGeneralSettings(GeneralSettings generalSettings)
        {
            var dataRequest = dataRequestFactory.UpdateGeneralSettings(generalSettings);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }
    }
}