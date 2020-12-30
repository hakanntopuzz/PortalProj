using System.Collections.Generic;

namespace DevPortal.Model
{
    public class ApplicationFullModel
    {
        public ApplicationFullModel()
        {
            this.ApplicationEnvironments = new List<ApplicationEnvironment>();
            this.ApplicationNugetPackages = new List<ApplicationNugetPackage>();
            this.JenkinsJobs = new List<JenkinsJob>();
            this.SonarqubeProjects = new List<SonarqubeProject>();
            this.SvnRepositories = new List<SvnRepository>();
            this.ApplicationDependencies = new List<ApplicationDependency>();
            this.DatabaseDependencies = new List<DatabaseDependency>();
            this.ExternalDependencies = new List<ExternalDependency>();
            this.NugetPackageDependencies = new List<NugetPackageDependency>();
        }

        public Application Application { get; set; }

        public ICollection<ApplicationEnvironment> ApplicationEnvironments { get; set; }

        public ICollection<SvnRepository> SvnRepositories { get; set; }

        public ICollection<JenkinsJob> JenkinsJobs { get; set; }

        public ICollection<SonarqubeProject> SonarqubeProjects { get; set; }

        public ICollection<ApplicationNugetPackage> ApplicationNugetPackages { get; set; }

        public ICollection<ApplicationDependency> ApplicationDependencies { get; set; }

        public ICollection<DatabaseDependency> DatabaseDependencies { get; set; }

        public ICollection<ExternalDependency> ExternalDependencies { get; set; }

        public ICollection<NugetPackageDependency> NugetPackageDependencies { get; set; }

        public bool HasApplicationEnvironments
        {
            get
            {
                return this.ApplicationEnvironments.Count > 0;
            }
        }

        public bool HasSvnRepositories
        {
            get
            {
                return this.SvnRepositories.Count > 0;
            }
        }

        public bool HasJenkinsJobs
        {
            get
            {
                return this.JenkinsJobs.Count > 0;
            }
        }

        public bool HasSonarqubeProjects
        {
            get
            {
                return this.SonarqubeProjects.Count > 0;
            }
        }

        public bool HasApplicationNugetPackages
        {
            get
            {
                return this.ApplicationNugetPackages.Count > 0;
            }
        }

        public bool HasApplicationDependencies
        {
            get
            {
                return this.ApplicationDependencies.Count > 0;
            }
        }

        public bool HasDatabaseDependencies
        {
            get
            {
                return this.DatabaseDependencies.Count > 0;
            }
        }

        public bool HasExternalDependencies
        {
            get
            {
                return this.ExternalDependencies.Count > 0;
            }
        }

        public bool HasNugetPackageDependencies
        {
            get
            {
                return this.NugetPackageDependencies.Count > 0;
            }
        }

        public bool IsNugetPackage
        {
            get
            {
                return Application.ApplicationTypeId == (int)ApplicationTypeEnum.NugetPackage;
            }
        }
    }
}