using AB.Data.DapperClient.Abstract;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Repositories
{
    public class ApplicationEnvironmentRepository : BaseDevPortalRepository, IApplicationEnvironmentRepository
    {
        #region ctor

        readonly IDataClient dataClient;

        new readonly IApplicationDataRequestFactory dataRequestFactory;

        public ApplicationEnvironmentRepository(
            IDataClient dataClient,
            IApplicationDataRequestFactory dataRequestFactory,
            ISettings settings)
          : base(dataClient, dataRequestFactory, settings)
        {
            this.dataClient = dataClient;
            this.dataRequestFactory = dataRequestFactory;
        }

        #endregion

        public ICollection<ApplicationEnvironment> GetApplicationEnvironments(int applicationId)
        {
            var dataRequest = dataRequestFactory.GetApplicationEnvironments(applicationId);
            var defaultReturnValue = new List<ApplicationEnvironment>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public ICollection<Environment> GetEnvironmentsDoesNotExistByApplicationId(int applicationId)
        {
            var dataRequest = dataRequestFactory.GetEnvironmentsDoesNotExistByApplicationId(applicationId);
            var defaultReturnValue = new List<Environment>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public bool AddApplicationEnvironment(ApplicationEnvironment applicationEnvironment)
        {
            var dataRequest = dataRequestFactory.AddApplicationEnvironment(applicationEnvironment);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public ApplicationEnvironment GetApplicationEnvironmentById(int applicationEnvironmentId)
        {
            var dataRequest = dataRequestFactory.GetApplicationEnvironmentById(applicationEnvironmentId);
            const ApplicationEnvironment defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public ApplicationEnvironment GetApplicationEnvironmentByEnvironmentId(int applicationId, int environmentId)
        {
            var dataRequest = dataRequestFactory.GetApplicationEnvironmentByEnvironmentId(applicationId, environmentId);
            const ApplicationEnvironment defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public bool UpdateApplicationEnvironment(ApplicationEnvironment applicationEnvironment)
        {
            var dataRequest = dataRequestFactory.UpdateApplicationEnvironment(applicationEnvironment);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public bool DeleteApplicationEnvironment(int environmentId)
        {
            var dataRequest = dataRequestFactory.DeleteApplicationEnvironment(environmentId);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public RecordUpdateInfo GetApplicationEnvironmentUpdateInfo(int environmentId)
        {
            var dataRequest = dataRequestFactory.GetApplicationEnvironmentUpdateInfo(environmentId);
            const RecordUpdateInfo defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }
    }
}