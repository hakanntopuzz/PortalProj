using DevPortal.Business.Abstract;
using DevPortal.Model;
using System.Collections.Generic;
using System.Linq;

namespace DevPortal.Business.Factories
{
    public class ApplicationFactory : IApplicationFactory
    {
        public ApplicationFullModel CreateApplicationFullModel(
            Application application,
            ICollection<ApplicationEnvironment> environments,
            ICollection<JenkinsJob> jenkinsJobs,
            ICollection<SonarqubeProject> sonarqubeProjects,
            ICollection<SvnRepository> svnRepositories,
            ICollection<ApplicationDependency> applicationDependencies,
            ICollection<DatabaseDependency> databaseDependencies,
            ICollection<ExternalDependency> externalDependencies,
            ICollection<NugetPackageDependency> nugetPackageDependencies)
        {
            return new ApplicationFullModel
            {
                Application = application,
                ApplicationEnvironments = environments,
                JenkinsJobs = jenkinsJobs,
                SonarqubeProjects = sonarqubeProjects,
                SvnRepositories = svnRepositories,
                ApplicationDependencies = applicationDependencies,
                DatabaseDependencies = databaseDependencies,
                ExternalDependencies = externalDependencies,
                NugetPackageDependencies = nugetPackageDependencies
            };
        }

        public ApplicationFullModel CreateNugetApplicationFullModel(
            Application application,
            ICollection<ApplicationNugetPackage> nugetPackages,
            ICollection<JenkinsJob> jenkinsJobs,
            ICollection<SonarqubeProject> sonarqubeProjects,
            ICollection<SvnRepository> svnRepositories,
            ICollection<ApplicationDependency> applicationDependencies,
            ICollection<DatabaseDependency> databaseDependencies,
            ICollection<ExternalDependency> externalDependencies,
            ICollection<NugetPackageDependency> nugetPackageDependencies)
        {
            return new ApplicationFullModel
            {
                Application = application,
                ApplicationNugetPackages = nugetPackages,
                JenkinsJobs = jenkinsJobs,
                SonarqubeProjects = sonarqubeProjects,
                SvnRepositories = svnRepositories,
                ApplicationDependencies = applicationDependencies,
                DatabaseDependencies = databaseDependencies,
                ExternalDependencies = externalDependencies,
                NugetPackageDependencies = nugetPackageDependencies
            };
        }

        public DependenciesExportListItem CreateDependencyFullModel(
            ICollection<ApplicationDependenciesExportListItem> applicationDependenciesExportListItems,
            ICollection<DatabaseDependenciesExportListItem> databaseDependenciesExportListItems,
            ICollection<ExternalDependenciesExportListItem> externalDependenciesExportListItems)
        {
            return new DependenciesExportListItem
            {
                ApplicationDependenciesExportListItem = applicationDependenciesExportListItems,
                DatabaseDependenciesExportListItem = databaseDependenciesExportListItems,
                ExternalDependenciesExportListItem = externalDependenciesExportListItems
            };
        }

        public ICollection<ApplicationExportListItem> CreateApplicationExportListItems(ICollection<ApplicationListItem> applications)
        {
            return applications.Select(x =>
                new ApplicationExportListItem
                {
                    Name = x.Name,
                    ApplicationGroupName = x.ApplicationGroupName,
                    ApplicationTypeName = x.ApplicationTypeName,
                    Status = x.Status
                }).ToList();
        }

        public ICollection<ApplicationWikiExportListItem> CreateApplicationWikiExportListItems(ICollection<ApplicationListItem> applications)
        {
            return applications.Select(x =>
                new ApplicationWikiExportListItem
                {
                    Name = x.Name,
                    ApplicationGroupName = x.ApplicationGroupName,
                    ApplicationTypeName = x.ApplicationTypeName,
                    Status = x.Status,
                    RedmineProjectName = x.RedmineProjectName
                }).ToList();
        }
    }
}