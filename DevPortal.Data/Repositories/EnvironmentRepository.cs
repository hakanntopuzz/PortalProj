using AB.Data.DapperClient.Abstract;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Data.Factories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Repositories
{
    public class EnvironmentRepository : BaseDevPortalRepository, IEnvironmentRepository
    {
        #region ctor

        readonly IDataClient dataClient;

        public EnvironmentRepository(IDataClient dataClient,
             IApplicationDataRequestFactory dataRequestFactory,
            ISettings settings)
            : base(dataClient, dataRequestFactory, settings)
        {
            this.dataClient = dataClient;
        }

        #endregion

        public ICollection<Environment> GetEnvironments()
        {
            var dataRequest = dataRequestFactory.GetEnvironments();
            var defaultReturnValue = new List<Environment>();

            return dataClient.GetCollection<Environment, RecordUpdateInfo, Environment>(
                           dataRequest,
                           DataClientMapFactory.EnvironmentsMap,
                           defaultReturnValue,
                           dataRequest.SplitOnParameters);
        }

        public Environment GetEnvironmentById(int id)
        {
            var dataRequest = dataRequestFactory.GetEnvironmentById(id);
            const Environment defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public RecordUpdateInfo GetEnvironmentUpdateInfo(int id)
        {
            var dataRequest = dataRequestFactory.GetEnvironmentUpdateInfo(id);
            const RecordUpdateInfo defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public Environment GetEnvironmentByName(string name)
        {
            var dataRequest = dataRequestFactory.GetEnvironmentByName(name);
            const Environment defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public int AddEnvironment(Environment environment)
        {
            var dataRequest = dataRequestFactory.AddEnvironment(environment);
            const int defaultReturnValue = 0;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public bool UpdateEnvironment(Environment environment)
        {
            var dataRequest = dataRequestFactory.UpdateEnvironment(environment);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public bool DeleteEnvironment(int environmentId)
        {
            var dataRequest = dataRequestFactory.DeleteEnvironment(environmentId);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public int GetApplicationEnvironmentCountByEnvironmentId(int environmentId)
        {
            var dataRequest = dataRequestFactory.GetApplicationEnvironmentCountByEnvironmentId(environmentId);
            const int defaultReturnValue = 0;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }
    }
}