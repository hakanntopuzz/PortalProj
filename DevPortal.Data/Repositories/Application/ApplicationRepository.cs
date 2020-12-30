using AB.Data.DapperClient.Abstract;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Data.Factories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.Data.Repositories
{
    public class ApplicationRepository : BaseDevPortalRepository, IApplicationRepository
    {
        #region ctor

        readonly IDataClient dataClient;

        new readonly IApplicationDataRequestFactory dataRequestFactory;

        public ApplicationRepository(
            IDataClient dataClient,
            IApplicationDataRequestFactory dataRequestFactory,
            ISettings settings)
            : base(dataClient, dataRequestFactory, settings)
        {
            this.dataClient = dataClient;
            this.dataRequestFactory = dataRequestFactory;
        }

        #endregion

        public ICollection<ApplicationGroup> GetApplicationGroups()
        {
            var dataRequest = dataRequestFactory.GetApplicationGroups();
            var defaultReturnValue = new List<ApplicationGroup>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public ICollection<ApplicationType> GetApplicationTypes()
        {
            var dataRequest = dataRequestFactory.GetApplicationTypes();
            var defaultReturnValue = new List<ApplicationType>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public ICollection<Application> GetApplicationsByApplicationGroupId(int applicationGroupId)
        {
            var dataRequest = dataRequestFactory.GetApplicationsByApplicationGroupId(applicationGroupId);
            var defaultReturnValue = new List<Application>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public int AddApplication(Application application)
        {
            var dataRequest = dataRequestFactory.AddApplication(application);
            const int defaultReturnValue = 0;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public ICollection<ApplicationListItem> GetApplications()
        {
            var dataRequest = dataRequestFactory.GetApplications();
            var defaultReturnValue = new List<ApplicationListItem>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public ICollection<ApplicationListItem> GetApplicationsByGroupIdAndName(int applicationGroupId, string applicationName)
        {
            var dataRequest = dataRequestFactory.GetApplicationsByGroupIdAndName(applicationGroupId, applicationName);
            var defaultReturnValue = new List<ApplicationListItem>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public ICollection<ApplicationListItem> GetApplicationsByGroupId(int applicationGroupId)
        {
            var dataRequest = dataRequestFactory.GetApplicationsByGroupId(applicationGroupId);
            var defaultReturnValue = new List<ApplicationListItem>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public ICollection<ApplicationListItem> GetApplicationsByApplicationName(string applicationName)
        {
            var dataRequest = dataRequestFactory.GetApplicationsByApplicationName(applicationName);
            var defaultReturnValue = new List<ApplicationListItem>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public Application GetApplication(int applicationId)
        {
            var dataRequest = dataRequestFactory.GetApplication(applicationId);
            var defaultReturnValue = new Application();

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public ICollection<SvnRepository> GetSvnRepositories(int applicationId)
        {
            var dataRequest = dataRequestFactory.GetSvnRepositories(applicationId);
            var defaultReturnValue = new List<SvnRepository>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public ICollection<JenkinsJob> GetJenkinsJobs(int applicationId)
        {
            var dataRequest = dataRequestFactory.GetJenkinsJobs(applicationId);
            var defaultReturnValue = new List<JenkinsJob>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public bool UpdateApplication(Application application)
        {
            var dataRequest = dataRequestFactory.UpdateApplication(application);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public ICollection<ApplicationStatus> GetApplicationStatusList()
        {
            var dataRequest = dataRequestFactory.GetApplicationStatusList();
            var defaultReturnValue = new List<ApplicationStatus>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public bool DeleteApplication(int applicationId)
        {
            var dataRequest = dataRequestFactory.DeleteApplication(applicationId);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public Application GetApplicationByApplicationName(string name)
        {
            var dataRequest = dataRequestFactory.GetApplicationByApplicationName(name);
            const Application defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public int GetApplicationCount()
        {
            var dataRequest = dataRequestFactory.GetApplicationCount();
            const int defaultReturnValue = 0;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public int GetApplicationGroupCount()
        {
            var dataRequest = dataRequestFactory.GetApplicationGroupCount();
            const int defaultReturnValue = 0;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public int GetJenkinsJobCount()
        {
            var dataRequest = dataRequestFactory.GetJenkinsJobCount();
            const int defaultReturnValue = 0;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public ICollection<ApplicationCountByTypeModel> GetApplicationCountByType()
        {
            var dataRequest = dataRequestFactory.GetApplicationCountByType();
            var defaultReturnValue = new List<ApplicationCountByTypeModel>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public ICollection<JenkinsJobCountByTypeModel> GetJenkinsJobCountByType()
        {
            var dataRequest = dataRequestFactory.GetJenkinsJobCountByType();
            var defaultReturnValue = new List<JenkinsJobCountByTypeModel>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public int GetSvnRepositoryCount()
        {
            var dataRequest = dataRequestFactory.GetSvnRepositoryCount();
            const int defaultReturnValue = 0;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public int GetSonarQubeProjectCount()
        {
            var dataRequest = dataRequestFactory.GetSonarQubeProjectCount();
            const int defaultReturnValue = 0;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public ICollection<SonarQubeProjectCountByTypeModel> GetSonarQubeProjectCountByType()
        {
            var dataRequest = dataRequestFactory.GetSonarQubeProjectCountByType();
            var defaultReturnValue = new List<SonarQubeProjectCountByTypeModel>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public int GetApplicationRedmineProjectCount()
        {
            var dataRequest = dataRequestFactory.GetApplicationRedmineProjectCount();
            const int defaultReturnValue = 0;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public int GetDatabaseRedmineProjectCount()
        {
            var dataRequest = dataRequestFactory.GetDatabaseRedmineProjectCount();
            const int defaultReturnValue = 0;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public int GetNugetPackageCount()
        {
            var dataRequest = dataRequestFactory.GetNugetPackageCount();
            const int defaultReturnValue = 0;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public ICollection<ApplicationListItem> GetLastUpdatedApplications()
        {
            var dataRequest = dataRequestFactory.GetLastUpdatedApplications();
            var defaultReturnValue = new List<ApplicationListItem>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public async Task<ICollection<Application>> GetFilteredApplicationListAsync(int skip, int take, string orderBy, string orderDir, string searchText, int applicationGroupId)
        {
            var dataRequest = dataRequestFactory.GetFilteredApplicationList(skip, take, orderBy, orderDir, searchText, applicationGroupId);
            var defaultReturnValue = new List<Application>();

            return await dataClient.GetCollectionAsync<Application, RecordUpdateInfo, Application>(
                dataRequest,
                DataClientMapFactory.ApplicationsMap,
                defaultReturnValue,
                dataRequest.SplitOnParameters);
        }

        public Application GetApplicationByJenkinsJobName(string name)
        {
            var dataRequest = dataRequestFactory.GetApplicationByJenkinsJobName(name);
            const Application defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public RecordUpdateInfo GetApplicationUpdateInfo(int applicationId)
        {
            var dataRequest = dataRequestFactory.GetApplicationUpdateInfo(applicationId);
            const RecordUpdateInfo defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public ICollection<Application> GetApplicationsWithLogByApplicationGroup(int applicationGroupId)
        {
            var dataRequest = dataRequestFactory.GetApplicationsWithLogByApplicationGroup(applicationGroupId);
            var defaultReturnValue = new List<Application>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public ICollection<NugetPackageDependency> GetNugetPackageDependenciesByApplicationId(int applicationId)
        {
            var dataRequest = dataRequestFactory.GetNugetPackageDependenciesByApplicationId(applicationId);
            var defaultReturnValue = new List<NugetPackageDependency>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public ICollection<ApplicationDependency> GetApplicationDependenciesByApplicationId(int applicationId)
        {
            var dataRequest = dataRequestFactory.GetApplicationDependenciesByApplicationId(applicationId);
            var defaultReturnValue = new List<ApplicationDependency>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public ICollection<ExternalDependency> GetExternalDependenciesByApplicationId(int applicationId)
        {
            var dataRequest = dataRequestFactory.GetExternalDependenciesByApplicationId(applicationId);
            var defaultReturnValue = new List<ExternalDependency>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public ICollection<ApplicationDependenciesExportListItem> GetApplicationDependencies(int applicationId)
        {
            var dataRequest = dataRequestFactory.GetApplicationDependencies(applicationId);
            var defaultReturnValue = new List<ApplicationDependenciesExportListItem>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public ICollection<ExternalDependenciesExportListItem> GetFullExternalDependenciesByApplicationId(int applicationId)
        {
            var dataRequest = dataRequestFactory.GetFullExternalDependenciesByApplicationId(applicationId);
            var defaultReturnValue = new List<ExternalDependenciesExportListItem>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public ApplicationType GetApplicationTypeByApplicationId(int applicationId)
        {
            var dataRequest = dataRequestFactory.GetApplicationTypeByApplicationId(applicationId);
            var defaultReturnValue = new ApplicationType();

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public async Task<ICollection<RedmineProject>> GetFilteredApplicationRedmineProjectListAsync(int skip, int take, string orderBy, string orderDir, string searchText, int applicationGroupId)
        {
            var dataRequest = dataRequestFactory.GetFilteredApplicationRedmineProjectList(skip, take, orderBy, orderDir, searchText, applicationGroupId);
            var defaultReturnValue = new List<RedmineProject>();

            return await dataClient.GetCollectionAsync(dataRequest, defaultReturnValue);
        }

        public ICollection<DatabaseDependenciesExportListItem> GetDatabaseDependencies(int applicationId)
        {
            var dataRequest = dataRequestFactory.GetDatabaseDependencies(applicationId);
            var defaultReturnValue = new List<DatabaseDependenciesExportListItem>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public ICollection<NugetPackageDependenciesExportListItem> GetNugetPackageDependencies(int applicationId)
        {
            var dataRequest = dataRequestFactory.GetNugetPackageDependencies(applicationId);
            var defaultReturnValue = new List<NugetPackageDependenciesExportListItem>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }
    }
}