using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Business.Abstract
{
    public interface IApplicationFactory
    {
        ApplicationFullModel CreateApplicationFullModel(
            Application application,
            ICollection<ApplicationEnvironment> environments,
            ICollection<JenkinsJob> jenkinsJobs,
            ICollection<SonarqubeProject> sonarqubeProjects,
            ICollection<SvnRepository> svnRepositories,
            ICollection<ApplicationDependency> applicationDependencies,
            ICollection<DatabaseDependency> databaseDependencies,
            ICollection<ExternalDependency> externalDependencies,
            ICollection<NugetPackageDependency> nugetPackageDependencies);

        ApplicationFullModel CreateNugetApplicationFullModel(
            Application application,
            ICollection<ApplicationNugetPackage> nugetPackages,
            ICollection<JenkinsJob> jenkinsJobs,
            ICollection<SonarqubeProject> sonarqubeProjects,
            ICollection<SvnRepository> svnRepositories,
            ICollection<ApplicationDependency> applicationDependencies,
            ICollection<DatabaseDependency> databaseDependencies,
            ICollection<ExternalDependency> externalDependencies,
            ICollection<NugetPackageDependency> nugetPackageDependencies);

        DependenciesExportListItem CreateDependencyFullModel(
              ICollection<ApplicationDependenciesExportListItem> applicationDependenciesExportListItems,
              ICollection<DatabaseDependenciesExportListItem> databaseDependenciesExportListItems,
              ICollection<ExternalDependenciesExportListItem> externalDependenciesExportListItems);

        ICollection<ApplicationExportListItem> CreateApplicationExportListItems(ICollection<ApplicationListItem> applications);

        ICollection<ApplicationWikiExportListItem> CreateApplicationWikiExportListItems(ICollection<ApplicationListItem> applications);
    }
}