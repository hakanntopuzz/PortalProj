using DevPortal.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.Data.Abstract.Repositories
{
    public interface IApplicationRepository
    {
        ICollection<Application> GetApplicationsByApplicationGroupId(int applicationGroupId);

        ICollection<ApplicationGroup> GetApplicationGroups();

        ICollection<ApplicationType> GetApplicationTypes();

        ICollection<ApplicationListItem> GetApplications();

        ICollection<ApplicationListItem> GetApplicationsByGroupIdAndName(int applicationGroupId, string applicationName);

        ICollection<ApplicationListItem> GetApplicationsByGroupId(int applicationGroupId);

        ICollection<ApplicationListItem> GetApplicationsByApplicationName(string applicationName);

        int AddApplication(Application application);

        Application GetApplication(int applicationId);

        ICollection<SvnRepository> GetSvnRepositories(int applicationId);

        ICollection<JenkinsJob> GetJenkinsJobs(int applicationId);

        bool UpdateApplication(Application application);

        ICollection<ApplicationStatus> GetApplicationStatusList();

        bool DeleteApplication(int applicationId);

        int GetApplicationCount();

        int GetApplicationGroupCount();

        int GetJenkinsJobCount();

        int GetSvnRepositoryCount();

        int GetSonarQubeProjectCount();

        int GetApplicationRedmineProjectCount();

        int GetDatabaseRedmineProjectCount();

        int GetNugetPackageCount();

        Application GetApplicationByApplicationName(string name);

        ICollection<ApplicationCountByTypeModel> GetApplicationCountByType();

        ICollection<JenkinsJobCountByTypeModel> GetJenkinsJobCountByType();

        ICollection<SonarQubeProjectCountByTypeModel> GetSonarQubeProjectCountByType();

        ICollection<ApplicationListItem> GetLastUpdatedApplications();

        Task<ICollection<Application>> GetFilteredApplicationListAsync(int skip, int take, string orderBy, string orderDir, string searchText, int applicationGroupId);

        Application GetApplicationByJenkinsJobName(string name);

        RecordUpdateInfo GetApplicationUpdateInfo(int applicationId);

        ICollection<Application> GetApplicationsWithLogByApplicationGroup(int applicationGroupId);

        ICollection<ApplicationDependency> GetApplicationDependenciesByApplicationId(int applicationId);

        ICollection<ExternalDependency> GetExternalDependenciesByApplicationId(int applicationId);

        ICollection<ApplicationDependenciesExportListItem> GetApplicationDependencies(int applicationId);

        ICollection<ExternalDependenciesExportListItem> GetFullExternalDependenciesByApplicationId(int applicationId);

        ApplicationType GetApplicationTypeByApplicationId(int applicationId);

        Task<ICollection<RedmineProject>> GetFilteredApplicationRedmineProjectListAsync(int skip, int take, string orderBy, string orderDir, string searchText, int applicationGroupId);

        ICollection<NugetPackageDependency> GetNugetPackageDependenciesByApplicationId(int applicationId);

        ICollection<DatabaseDependenciesExportListItem> GetDatabaseDependencies(int applicationId);

        ICollection<NugetPackageDependenciesExportListItem> GetNugetPackageDependencies(int applicationId);
    }
}