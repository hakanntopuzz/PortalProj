using AB.Data.DapperClient.Abstract;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Repositories
{
    public class ApplicationSonarqubeProjectRepository : BaseDevPortalRepository, IApplicationSonarqubeProjectRepository
    {
        #region ctor

        readonly IDataClient dataClient;

        new readonly IApplicationDataRequestFactory dataRequestFactory;

        public ApplicationSonarqubeProjectRepository(
            IDataClient dataClient,
            IApplicationDataRequestFactory dataRequestFactory,
            ISettings settings)
          : base(dataClient, dataRequestFactory, settings)
        {
            this.dataClient = dataClient;
            this.dataRequestFactory = dataRequestFactory;
        }

        #endregion

        public ICollection<SonarqubeProject> GetSonarqubeProjects(int applicationId)
        {
            var dataRequest = dataRequestFactory.GetSonarqubeProjects(applicationId);
            var defaultReturnValue = new List<SonarqubeProject>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public SonarqubeProject GetApplicationSonarQubeProjectById(int projectId)
        {
            var dataRequest = dataRequestFactory.GetApplicationSonarQubeProjectById(projectId);
            const SonarqubeProject defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public ICollection<SonarQubeProjectType> GetSonarQubeProjectTypes()
        {
            var dataRequest = dataRequestFactory.GetSonarQubeProjectTypes();
            var defaultValue = new List<SonarQubeProjectType>();

            return dataClient.GetCollection(dataRequest, defaultValue);
        }

        public bool AddApplicationSonarQubeProject(SonarqubeProject project)
        {
            var dataRequest = dataRequestFactory.AddApplicationSonarQubeProject(project);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public bool DeleteApplicationSonarQubeProject(int projectId)
        {
            var dataRequest = dataRequestFactory.DeleteApplicationSonarQubeProject(projectId);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public bool UpdateApplicationSonarQubeProject(SonarqubeProject project)
        {
            var dataRequest = dataRequestFactory.UpdateApplicationSonarQubeProject(project);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public RecordUpdateInfo GetApplicationSonarQubeProjectUpdateInfo(int projectId)
        {
            var dataRequest = dataRequestFactory.GetApplicationSonarQubeProjectUpdateInfo(projectId);
            const RecordUpdateInfo defaultReturnValue = null;

            return DataClient.GetItem(dataRequest, defaultReturnValue);
        }
    }
}