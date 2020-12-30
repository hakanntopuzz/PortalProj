using DevPortal.Model;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Abstract
{
    public interface IApplicationViewModelFactory
    {
        #region application

        ApplicationViewModel CreateApplication(Application application);

        EditApplicationViewModel CreateEditApplication(
            Application application,
            ICollection<ApplicationGroup> applicationGroups,
            ICollection<ApplicationType> applicationTypes,
            ICollection<ApplicationStatus> applicationStatusList);

        ApplicationsViewModel CreateApplicationsViewModel(ICollection<ApplicationGroup> applicationGroups, ICollection<ApplicationListItem> applications);

        AddApplicationViewModel CreateApplicationAddViewModel(
            ICollection<ApplicationGroup> applicationGroups,
            ICollection<ApplicationType> applicationTypes,
            ICollection<ApplicationStatus> applicationStatus);

        ApplicationsViewModel CreateApplicationsViewModelWithFilter(
            ICollection<ApplicationGroup> applicationGroups,
            ICollection<ApplicationListItem> applications,
            int applicationGroupId,
            string applicationName);

        ApplicationListModel CreateApplicationListModel(IEnumerable<Application> applicationList);

        ApplicationFullModel CreateApplicationFullModel(Application application,
            ICollection<ApplicationEnvironment> applicationEnvironments,
            ICollection<SvnRepository> svnRepositories,
            ICollection<JenkinsJob> jenkinsJobs,
            ICollection<SonarqubeProject> sonarqubeProjects,
            ICollection<ApplicationNugetPackage> applicationNugetPackages);

        #endregion
    }
}