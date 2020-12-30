using DevPortal.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.Business.Abstract
{
    public interface IApplicationReaderService
    {
        Task<ICollection<RedmineProject>> GetFilteredApplicationRedmineProjectListAsync(RedmineTableParam tableParam);

        Application GetApplication(int applicationId);

        ICollection<ApplicationListItem> FilterApplications(int applicationGroupId, string applicationName);

        ICollection<ApplicationListItem> GetApplications();

        ICollection<ApplicationListItem> GetApplicationsByGroupIdAndName(int applicationGroupId, string applicationName);

        ICollection<ApplicationListItem> GetApplicationsByApplicationName(string applicationName);

        ICollection<ApplicationListItem> GetApplicationsByGroupId(int applicationGroupId);

        ApplicationType GetApplicationTypeByApplicationId(int applicationId);

        ICollection<ApplicationGroup> GetApplicationGroups();

        ICollection<Application> GetApplicationsByApplicationGroupId(int applicationGroupId);

        ICollection<Application> GetApplicationsWithLogByApplicationGroup(int applicationGroupId);

        ICollection<ApplicationType> GetApplicationTypes();

        ICollection<ApplicationStatus> GetApplicationStatusList();

        ICollection<ApplicationListItem> GetLastUpdatedApplications();

        Task<IEnumerable<Application>> GetFilteredApplicationListAsync(ApplicationTableParam tableParam);

        #region svn

        ICollection<SvnRepository> GetSvnRepositories(int applicationId);

        #endregion

        #region jenkins

        ICollection<JenkinsJob> GetJenkinsJobs(int applicationId);

        Application GetApplicationByJenkinsJobName(string name);

        #endregion
    }
}