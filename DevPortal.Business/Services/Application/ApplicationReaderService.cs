using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevPortal.Business.Services
{
    public class ApplicationReaderService : IApplicationReaderService
    {
        #region ctor

        readonly IApplicationRepository applicationRepository;

        readonly IGeneralSettingsService generalSettingsService;

        public ApplicationReaderService(
            IApplicationRepository applicationRepository,
            IGeneralSettingsService generalSettingsService)
        {
            this.applicationRepository = applicationRepository;
            this.generalSettingsService = generalSettingsService;
        }

        #endregion

        public async Task<ICollection<RedmineProject>> GetFilteredApplicationRedmineProjectListAsync(RedmineTableParam tableParam)
        {
            if (tableParam == null)
            {
                return new List<RedmineProject>();
            }

            var projects = await applicationRepository.GetFilteredApplicationRedmineProjectListAsync(
                tableParam.start,
                tableParam.length,
                tableParam.SortColumn,
                tableParam.order.FirstOrDefault()?.dir,
                tableParam.SearchText,
                tableParam.ApplicationGroupId);

            foreach (var project in projects)
            {
                project.ProjectUrl = generalSettingsService.GetRedmineProjectUrl(project.ProjectName).ToString();
                project.RepositoryUrl = $"{project.ProjectUrl}/repository";
            }

            return projects;
        }

        public Application GetApplication(int applicationId)
        {
            var application = applicationRepository.GetApplication(applicationId);

            if (application == null)
            {
                return null;
            }

            application.RecordUpdateInfo = applicationRepository.GetApplicationUpdateInfo(applicationId);

            SetApplicationRedmineUrl(application);

            return application;
        }

        void SetApplicationRedmineUrl(Application application)
        {
            application.RedmineProjectUrl = generalSettingsService.GetRedmineProjectUrl(application.RedmineProjectName).ToString();
        }

        #region filter applications

        public ICollection<ApplicationListItem> FilterApplications(int applicationGroupId, string applicationName)
        {
            if (FilterByApplicationGroup(applicationGroupId, applicationName))
            {
                return GetApplicationsByGroupId(applicationGroupId);
            }

            if (FilterByApplicationName(applicationGroupId, applicationName))
            {
                return GetApplicationsByApplicationName(applicationName);
            }

            if (FilterByApplicationGroupAndName(applicationGroupId, applicationName))
            {
                return GetApplicationsByGroupIdAndName(applicationGroupId, applicationName);
            }

            return GetApplications();
        }

        static bool FilterByApplicationGroup(int applicationGroupId, string applicationName)
        {
            return applicationGroupId != 0 && string.IsNullOrEmpty(applicationName);
        }

        static bool FilterByApplicationName(int applicationGroupId, string applicationName)
        {
            return applicationGroupId == 0 && !string.IsNullOrEmpty(applicationName);
        }

        static bool FilterByApplicationGroupAndName(int applicationGroupId, string applicationName)
        {
            return applicationGroupId != 0 && !string.IsNullOrEmpty(applicationName);
        }

        #endregion

        public ICollection<ApplicationListItem> GetApplications()
        {
            return applicationRepository.GetApplications();
        }

        public ICollection<ApplicationListItem> GetApplicationsByGroupId(int applicationGroupId)
        {
            return applicationRepository.GetApplicationsByGroupId(applicationGroupId);
        }

        public ICollection<ApplicationListItem> GetApplicationsByApplicationName(string applicationName)
        {
            return applicationRepository.GetApplicationsByApplicationName(applicationName);
        }

        public ICollection<ApplicationListItem> GetApplicationsByGroupIdAndName(int applicationGroupId, string applicationName)
        {
            return applicationRepository.GetApplicationsByGroupIdAndName(applicationGroupId, applicationName);
        }

        public ICollection<Application> GetApplicationsWithLogByApplicationGroup(int applicationGroupId)
        {
            return applicationRepository.GetApplicationsWithLogByApplicationGroup(applicationGroupId);
        }

        public ApplicationType GetApplicationTypeByApplicationId(int applicationId)
        {
            return applicationRepository.GetApplicationTypeByApplicationId(applicationId);
        }

        public ICollection<ApplicationGroup> GetApplicationGroups()
        {
            return applicationRepository.GetApplicationGroups();
        }

        public ICollection<Application> GetApplicationsByApplicationGroupId(int applicationGroupId)
        {
            return applicationRepository.GetApplicationsByApplicationGroupId(applicationGroupId);
        }

        public ICollection<ApplicationType> GetApplicationTypes()
        {
            return applicationRepository.GetApplicationTypes();
        }

        public async Task<IEnumerable<Application>> GetFilteredApplicationListAsync(ApplicationTableParam tableParam)
        {
            if (tableParam == null)
            {
                return new List<Application>();
            }

            return await applicationRepository.GetFilteredApplicationListAsync(
                tableParam.start,
                tableParam.length,
                tableParam.SortColumn,
                tableParam.order.FirstOrDefault()?.dir,
                tableParam.SearchText,
                tableParam.ApplicationGroupId);
        }

        public ICollection<ApplicationStatus> GetApplicationStatusList()
        {
            return applicationRepository.GetApplicationStatusList();
        }

        public ICollection<ApplicationListItem> GetLastUpdatedApplications()
        {
            return applicationRepository.GetLastUpdatedApplications();
        }

        #region svn

        public ICollection<SvnRepository> GetSvnRepositories(int applicationId)
        {
            var svnRepoList = applicationRepository.GetSvnRepositories(applicationId);

            SetFullSvnUrls(svnRepoList);

            return svnRepoList;
        }

        void SetFullSvnUrls(ICollection<SvnRepository> svnRepoList)
        {
            var svnUrl = generalSettingsService.GetSvnUrl();
            svnRepoList.ToList().ForEach(q => q.SvnUrl = svnUrl + q.Name);
        }

        #endregion

        #region jenkins

        public ICollection<JenkinsJob> GetJenkinsJobs(int applicationId)
        {
            var jenkinsJobList = applicationRepository.GetJenkinsJobs(applicationId);

            jenkinsJobList.ToList().ForEach(q => q.JobUrl = generalSettingsService.GetJenkinsJobUrl(q.JenkinsJobName).ToString());

            return jenkinsJobList;
        }

        public Application GetApplicationByJenkinsJobName(string name)
        {
            return applicationRepository.GetApplicationByJenkinsJobName(name);
        }

        #endregion
    }
}