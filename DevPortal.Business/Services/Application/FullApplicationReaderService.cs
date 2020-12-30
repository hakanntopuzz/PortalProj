using DevPortal.Business.Abstract;
using DevPortal.Framework.Extensions;
using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Business.Services
{
    public class FullApplicationReaderService : IFullApplicationReaderService
    {
        #region ctor

        readonly IApplicationReaderService applicationReaderService;

        readonly IApplicationDependencyReaderService applicationDependencyReaderService;

        readonly IApplicationEnvironmentService applicationEnvironmentService;

        readonly IApplicationSonarqubeProjectService applicationSonarqubeProjectService;

        readonly IDatabaseDependencyService databaseDependencyService;

        readonly IApplicationNugetPackageService applicationNugetPackageService;

        readonly IApplicationFactory applicationFactory;

        public FullApplicationReaderService(
            IApplicationReaderService applicationReaderService,
            IApplicationDependencyReaderService applicationDependencyReaderService,
            IApplicationEnvironmentService applicationEnvironmentService,
            IApplicationSonarqubeProjectService applicationSonarqubeProjectService,
            IDatabaseDependencyService databaseDependencyService,
            IApplicationNugetPackageService applicationNugetPackageService,
            IApplicationFactory applicationFactory)
        {
            this.applicationReaderService = applicationReaderService;
            this.applicationDependencyReaderService = applicationDependencyReaderService;
            this.applicationEnvironmentService = applicationEnvironmentService;
            this.applicationSonarqubeProjectService = applicationSonarqubeProjectService;
            this.databaseDependencyService = databaseDependencyService;
            this.applicationNugetPackageService = applicationNugetPackageService;
            this.applicationFactory = applicationFactory;
        }

        #endregion

        public ApplicationFullModel GetApplicationWithAllMembers(int applicationId)
        {
            var application = applicationReaderService.GetApplication(applicationId);
            var svnRepositories = applicationReaderService.GetSvnRepositories(applicationId);
            var jenkinsJobs = applicationReaderService.GetJenkinsJobs(applicationId);
            var sonarqubeProjects = applicationSonarqubeProjectService.GetSonarqubeProjects(applicationId);
            var applicationDependencies = applicationDependencyReaderService.GetApplicationDependencies(applicationId);
            var databaseDependencies = databaseDependencyService.GetDatabaseDependenciesByApplicationId(applicationId);
            var externalDependencies = applicationDependencyReaderService.GetExternalDependencies(applicationId);
            var nugetDependencies = applicationDependencyReaderService.GetNugetPackageDependencies(applicationId);

            if (IsApplicationTypeEqualToNugetPackage(application))
            {
                return CreateNugetApplicationFullModel(
                    applicationId,
                    application,
                    svnRepositories,
                    jenkinsJobs,
                    sonarqubeProjects,
                    applicationDependencies,
                    databaseDependencies,
                    externalDependencies,
                    nugetDependencies);
            }

            return CreateApplicationFullModel(
                applicationId,
                application,
                svnRepositories,
                jenkinsJobs,
                sonarqubeProjects,
                applicationDependencies,
                databaseDependencies,
                externalDependencies,
                nugetDependencies);
        }

        static bool IsApplicationTypeEqualToNugetPackage(Application application)
        {
            return application.ApplicationTypeId == ApplicationTypeEnum.NugetPackage.ToInt32();
        }

        ApplicationFullModel CreateNugetApplicationFullModel(int applicationId, Application application, ICollection<SvnRepository> svnRepositories, ICollection<JenkinsJob> jenkinsJobs, ICollection<SonarqubeProject> sonarqubeProjects, ICollection<ApplicationDependency> applicationDependencies, ICollection<DatabaseDependency> databaseDependencies, ICollection<ExternalDependency> externalDependencies, ICollection<NugetPackageDependency> nugetDependencies)
        {
            var nugetPackages = applicationNugetPackageService.GetNugetPackages(applicationId);

            return applicationFactory.CreateNugetApplicationFullModel(
                 application,
                 nugetPackages,
                 jenkinsJobs,
                 sonarqubeProjects,
                 svnRepositories,
                 applicationDependencies,
                 databaseDependencies,
                 externalDependencies,
                 nugetDependencies);
        }

        ApplicationFullModel CreateApplicationFullModel(int applicationId, Application application, ICollection<SvnRepository> svnRepositories, ICollection<JenkinsJob> jenkinsJobs, ICollection<SonarqubeProject> sonarqubeProjects, ICollection<ApplicationDependency> applicationDependencies, ICollection<DatabaseDependency> databaseDependencies, ICollection<ExternalDependency> externalDependencies, ICollection<NugetPackageDependency> nugetDependencies)
        {
            var environments = applicationEnvironmentService.GetApplicationEnvironmentsHasLog(applicationId);

            return applicationFactory.CreateApplicationFullModel(
               application,
               environments,
               jenkinsJobs,
               sonarqubeProjects,
               svnRepositories,
               applicationDependencies,
               databaseDependencies,
               externalDependencies,
               nugetDependencies);
        }
    }
}