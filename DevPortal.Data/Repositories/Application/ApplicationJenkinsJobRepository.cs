using AB.Data.DapperClient.Abstract;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Repositories
{
    public class ApplicationJenkinsJobRepository : BaseDevPortalRepository, IApplicationJenkinsJobRepository
    {
        #region ctor

        readonly IDataClient dataClient;

        new readonly IApplicationDataRequestFactory dataRequestFactory;

        public ApplicationJenkinsJobRepository(
            IDataClient dataClient,
            IApplicationDataRequestFactory dataRequestFactory,
            ISettings settings)
            : base(dataClient, dataRequestFactory, settings)
        {
            this.dataClient = dataClient;
            this.dataRequestFactory = dataRequestFactory;
        }

        #endregion

        public ICollection<JenkinsJobType> GetJenkinsJobTypes()
        {
            var dataRequest = dataRequestFactory.GetJenkinsJobTypes();
            var defaultValue = new List<JenkinsJobType>();

            return dataClient.GetCollection(dataRequest, defaultValue);
        }

        public bool AddApplicationJenkinsJob(JenkinsJob applicationJenkinsJob)
        {
            var dataRequest = dataRequestFactory.AddApplicationJenkinsJob(applicationJenkinsJob);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public JenkinsJob GetApplicationJenkinsJobById(int jenkinsJobId)
        {
            var dataRequest = dataRequestFactory.GetApplicationJenkinsJobById(jenkinsJobId);
            const JenkinsJob defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public bool UpdateApplicationJenkinsJob(JenkinsJob jenkinsJob)
        {
            var dataRequest = dataRequestFactory.UpdateApplicationJenkinsJob(jenkinsJob);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public bool DeleteApplicationJenkinsJob(int jenkinsJobId)
        {
            var dataRequest = dataRequestFactory.DeleteApplicationJenkinsJob(jenkinsJobId);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public RecordUpdateInfo GetApplicationJenkinsJobUpdateInfo(int jenkinsJobId)
        {
            var dataRequest = dataRequestFactory.GetApplicationJenkinsJobUpdateInfo(jenkinsJobId);
            const RecordUpdateInfo defaultReturnValue = null;

            return DataClient.GetItem(dataRequest, defaultReturnValue);
        }
    }
}